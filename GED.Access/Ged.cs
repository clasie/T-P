using GED.Access.Const;
using GED.Access.CustomExceptions;
using GED.Access.Enums;
using GED.Access.GEDJsonClasses;
using GED.Access.Helpers;
using GED.Access.Interfaces;
using GED.Access.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GED.Access
{
    /// <summary>
    /// DLL qui permet de pousser/chercher des fichiers de type PDF 
    /// vers et depuis le web service de la GED.
    /// 
    /// Comment utiliser la dll
    /// -----------------------
    /// 
    /// 1- Créer une instance ->
    /// 
    ///                Ged gedAccess = new GED.Access.Ged(
    ///                    monUserLoginSurLaGed,
    ///                    monUserPwSurKLaGed,
    ///                    urlGedQueJeVeuxUtiliser
    ///                );
    /// 
    ///  2- Tester la connection en demandant un token ->
    /// 
    ///             GedTokenAnswer answerToken = gedAccess.GetConnectionToken();
    /// 
    ///  3- Pousser une facture ->
    ///  
    ///                 GEDJsonClassRequestResponse gedUploadAnswer =
    ///                             gedAccess.UploadFile(
    ///                                  barCode,
    ///                                  fileUri,
    ///                                  GedService.DocTypesEnum.FAC);
    ///   
    ///  4- Pousser une annexe ->
    ///  
    ///                 GEDJsonClassRequestResponse gedUploadAnswer =
    ///                             gedAccess.UploadFile(
    ///                                  barCode,
    ///                                  fileUri,
    ///                                  GedService.DocTypesEnum.AFAC);
    ///   
    /// 
    ///  5- Pousser une nouvelle facture avec plusieurs annexes ->
    ///  
    ///            List<GEDJsonClassRequestResponse> listResponsesNewFacture =
    ///                gedAccess.UploadFactureWithAnnexes(
    ///                    barCode,
    ///                    urlAnnexeList,
    ///                    fileUri);
    ///   
    ///  6- Pousser plusieurs annexes sur une facture existante ->
    ///  
    ///            List<GEDJsonClassRequestResponse> listResponsesNewFacture =
    ///                gedAccess.UploadFactureWithAnnexes(
    ///                    barCode,
    ///                    urlAnnexeList);
    ///        
    /// 
    ///  7- Pour Vérifier si la taille du fichier uploadé puis downloadé puis 
    ///     sauvé sur le disque local puis rechargé pour en extraire sa taille 
    ///     concorde avec la taille à l'upload.
    ///     Toutes les info updatées sont retournées dans l'instance en paramètre.
    ///     
    ///             gedAccess.DoubleCheckUploadedFile(gedDownloadAnswer, tailleAttendueAvantUpload))
    /// 
    /// 
    ///   8- Si l'on veut sauver en local le download d'une facture ou d'une annexe 
    ///      pour vérifier le visu du contenu du fichier->
    ///   
    ///                if (gedAccess.SaveDownloadedFile(gedDownloadAnswerFacture1).StatusFileCopiedLocally)
    ///                {
    ///                    //Fichier bien sauvegardé 
    ///                    //ici: string pfcl = gedDownloadAnswerFacture1.PathFileCopiedLocally;
    ///                }
    ///                else
    ///                {
    ///                     //Fichier non sauver, voir l'erreur dans 
    ///                }
    /// 
    /// 
    /// 
    /// Il est conseillé de capturer TOUTES custom exceptions lors 
    /// des appels quels qu'ils soient en finissant par une Exception finale.
    /// 
    /// Les custom exceptions utilisables sont:
    /// 
    ///     GEDConnectionUnknownException
    ///     GEDGlobalUnknownException
    ///     GEDInvalidCredentialsException
    ///     GEDTokenException
    ///     GEDUploadFileException
    ///     GEDUploadFileUnknownException
    ///     GEDUserLockedTooMuchFailingAttempsToConnectException
    ///     
    /// 
    /// Voir tous les autres exemples d'utilisation dans les tests unitaires->
    ///  
    ///    TPGED_xxx_yyy
    /// 
    /// 
    /// </summary>
    public class Ged : IGED
    {
        #region Props/Fields

        #region File type
        public string HttpParamContentType { get; set; }
        #endregion

        #region Creds Properties
        public string UserName { get; set; }
        public string UserPassWord { get; set; }
        public string Token { get; set; }
        #endregion

        #region URL Properties
        public string GEDUrl { get; set; }
        #endregion

        #endregion Props/Fields

        #region Constr
        /// <summary>
        /// Crée une instance avec les creds et l'url.
        /// Exemple d'url : http://35.241.194.111/GEDServerV2WS/WebService/document. Sans barre oblique à la fin.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassWord"></param>
        /// <param name="gEDUrl"></param>
        /// <param name="httpParamContentType"></param>
        public Ged(string userName, string userPassWord, string gEDUrl, string httpParamContentApplicationType = GedConstants.StringEmpty)
        {
            //store the creds
            UserName = userName;
            UserPassWord = userPassWord;
            GEDUrl = gEDUrl;
            HttpParamContentType = (string.IsNullOrWhiteSpace(httpParamContentApplicationType)) ? GedConstants.HttpParamContentTypeAppliPdf : httpParamContentApplicationType;
        }
        #endregion

        #region Method GetConnectionToken
        /// <summary>
        /// This method is made to test the connection, it returns the GEDToken instance
        /// class (containing a token and a date) from the GED web service 
        /// OR throws the possible following exceptions:
        /// GEDInvalidCredentialsException OR 
        /// GEDUserLockedTooMuchFailingAttempsToConnectException OR 
        /// GEDConnectionUnknownException.
        /// </summary>
        /// <returns>GEDToken</returns>
        public GedTokenAnswer GetConnectionToken()
        {
            return ExceptionCallerHelper.TokenTestExecute(
                TokenWebHelper.GetNewToken(UserName, UserPassWord, GEDUrl));
        }
        #endregion

        #region Method UploadFile()
        /// <summary>
        /// UploadFile according to the the doc type
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="entirePathFileToUpload"></param>
        /// <param name="enumDocType"></param>
        /// <returns></returns>
        public GEDJsonClassRequestResponse UploadFile(
            string barcode,
            string entirePathFileToUpload,
            GedService.DocTypesEnum enumDocType,
            bool doubleCheckTheUpload = false
        )
        {
            //File Validation before going further
            FileValidationHelper.ValidateFile(entirePathFileToUpload);

            //GED Web call preparation
            string requestURL = $"{GEDUrl}{GedConstants.WebSeparator}{GedConstants.ServiceUpload}";
            string fileName = entirePathFileToUpload;
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(fileName); //in case the file is on another server.
            wc.Dispose();
            
            string userAgent = GedConstants.WebUserAgent;
            string returnResponseText = string.Empty;

            int loopsMax = GedConstants.LoopGetTokenMinConterValue;
            while (loopsMax++ < GedConstants.LoopGetTokenMaxConterValue)
            {
                try
                {
                    //Prepare parameters
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add(GedConstants.ParameterFile, new UploadFileHelper.FileParameter(bytes, Path.GetFileName(fileName), HttpParamContentType));
                    postParameters.Add(GedConstants.ParameterToken, Token);
                    postParameters.Add(GedConstants.ParameterDocType, enumDocType.ToString());
                    postParameters.Add(GedConstants.ParameterBarCode, barcode);
                    postParameters.Add(GedConstants.ParameterProvider, string.Empty);
                    //Prepare call the service
                    HttpWebResponse webResponse =
                        UploadFileHelper.MultipartFormPost(
                            requestURL,
                            userAgent,
                            postParameters,
                            GedConstants.HeaderCacheParam,
                            GedConstants.HeaderCacheValue
                         );
                    //Call the service 
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    returnResponseText = responseReader.ReadToEnd();
                    //Analyse the response
                    return ExceptionCallerHelper.UploadFileTestExecute(returnResponseText, bytes.Length);
                }
                //The token has expired, we try again just once
                catch (GEDTokenException gedTokenException)
                {
                    Token = GetConnectionToken().Token;
                }
            }
            return new GEDJsonClassRequestResponse() { error = $"{GedConstants.UnknownDEGDllError}, {this.GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}" };

        }
        #endregion

        #region Methode DoubleCheckUploadedFile
        /// <summary>
        /// Vérifie si la taille du fichier uploadé puis downloadé puis 
        /// sauvé sur le dique local puis rechargé pour en extraire sa taille 
        /// concorde avec la taille à l'upload.
        /// Toutes les info updatées sont retournées dans
        /// l'instance en paramètre.
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <returns></returns>
        public bool DoubleCheckUploadedFile(GEDJsonClassRequestResponse jsonDownloadResponse,int sizeExpected)
        {
            bool passed = true;
            //File correctly copied locally
            if (SaveDownloadedFile(jsonDownloadResponse).StatusFileCopiedLocally)
            {
                FileInfo info = new FileInfo(jsonDownloadResponse.PathFileCopiedLocally);
                var sizeOnDisk = (int) info.Length;
                if (!sizeExpected.Equals(sizeOnDisk)) {
                    passed = false;
                    jsonDownloadResponse.doubleCheckLocalSizeStatusError =
                        $"{TP.Resources.Common.CommonResources.Erreur_SizeExpected} {sizeExpected} {TP.Resources.Common.CommonResources.Erreur_SizeOnDisk} {sizeOnDisk}";
                }
            }
            else
            {
                passed = false;
            }
            return passed;
        }
        #endregion

        #region Method DownloadFile()
        /// <summary>
        /// DownloadFile
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="enumDocType"></param>
        /// <param name="majorVersion">Pas pris en compte par la GED actuellement</param>
        /// <param name="minorVersion">Pas pris en compte par la GED actuellement</param>
        /// <returns></returns>
        public GEDJsonClassRequestResponse DownloadFile(
            string barcode,
            GedService.DocTypesEnum enumDocType,
            int majorVersion = GedConstants.MaxVersionMinimal,
            int minorVersion = GedConstants.MinVersionMinimal
        )
        {
            string requestURL = $"{GEDUrl}{GedConstants.WebSeparator}{GedConstants.ServiceDownload}";
            int loopsMax = GedConstants.LoopGetTokenMinConterValue;
            while (loopsMax < GedConstants.LoopGetTokenMaxConterValue)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        //Construire l'url GET
                        Uri url = new Uri(requestURL).
                            AddQuery(GedConstants.ParameterRequestType, GedConstants.ParameterRequestTypeJson).
                            AddQuery(GedConstants.ParameterToken, Token).
                            AddQuery(GedConstants.ParameterDocType, enumDocType.ToString()).
                            AddQuery(GedConstants.ParameterBarCode, barcode).
                            //Gestion des versions
                            AddQuery(GedConstants.ParameterMajorVersion, (GedConstants.MaxVersionMinimal.Equals(majorVersion)) ? string.Empty: majorVersion.ToString()).
                            AddQuery(GedConstants.ParameterMinorVersion, (GedConstants.MinVersionMinimal.Equals(minorVersion)) ? string.Empty : minorVersion.ToString()).
                            //Demande d'annexe
                            AddQuery(GedConstants.ParameterAnnexe, (GedService.DocTypesEnum.AFAC.Equals(enumDocType))?true.ToString().ToLower():false.ToString().ToLower());

                        //Ask the GED
                        byte[] responsebytes = client.DownloadData(url.ToString());
                        string responsebody = Encoding.UTF8.GetString(responsebytes);

                        //Analyse the response                        
                        return ExceptionCallerHelper.UploadFileTestExecute(responsebody);
                    }
                }
                catch (GEDTokenException gedTokenException)
                {
                    //The token has expired or is empty, try again but just once
                    Token = GetConnectionToken().Token;
                }
                //Avoid end loop if GED problems
                loopsMax++;
            }
            return new GEDJsonClassRequestResponse() { error = $"{GedConstants.UnknownDEGDllError}, {this.GetType().Name}.{System.Reflection.MethodBase.GetCurrentMethod().Name}" };

        }
        #endregion

        #region Method SaveDownloadedFile()
        /// <summary>
        /// For test purposes. After uploaded and downloaded a file
        /// to and from the GED this method creates locally the downloaded file
        /// providing an easy way to compare the source and the 
        /// downladed.
        /// </summary>
        /// <param name="gedDownloadAnswer"></param>
        /// <param name="completeFilePath">If empty the file is saved in your local temp folder</param>
        /// <returns></returns>
        public GEDJsonClassRequestResponse SaveDownloadedFile(
            GEDJsonClassRequestResponse gedDownloadAnswer,
            string completeFilePath = GedConstants.StringEmpty
        )
        {
            try
            {
                //Obtimistic
                gedDownloadAnswer.StatusFileCopiedLocally = true;
                //Automatic construction of file path
                if (string.IsNullOrWhiteSpace(completeFilePath))
                {
                    completeFilePath = Path.Combine(Path.GetTempPath(), gedDownloadAnswer.name);
                }
                //Set the target file path.
                gedDownloadAnswer.PathFileCopiedLocally = completeFilePath;
                byte[] unsigned = (byte[])(Array)gedDownloadAnswer.file;
                //Copy the file
                using (var fs = new FileStream(completeFilePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(unsigned, 0, unsigned.Length);
                }
            }
            catch (Exception ex)
            {
                gedDownloadAnswer.StatusFileCopiedLocally = false;
                gedDownloadAnswer.error = ex.ToString();
            }
            return gedDownloadAnswer;
        }
        #endregion

        #region UploadFactureWithAnnexes
        /// <summary>
        /// Cette méthode ajoute une nouvelle facture et ses annexes. 
        /// Si l'url du fichier de la facture est vide on considère 
        /// juste le bare code de la facture (sensée déjà exister sur la GED)
        /// et on lui associe une à une les annexes.
        /// Au final une facture ne sera jamais associée qu'à une annexe concaténée.
        /// </summary>
        /// <param name="factureBarCode"></param>
        /// <param name="entireAnnexeFilePaths"></param>
        /// <param name="entireFilePath"></param>
        /// <returns></returns>
        public List<GEDJsonClassRequestResponse> UploadFactureWithAnnexes(
                string factureBarCode,
                List<string> entireAnnexeFilePathsList,
                string entireFilePath = GedConstants.StringEmpty
        )
        {
            //Prepare return values
            List<GEDJsonClassRequestResponse> listGEDJsonClassRequestResponse = new List<GEDJsonClassRequestResponse>();
            GEDJsonClassRequestResponse gEDJsonClassRequestResponse;
            //Nouvelle facture
            if (!string.IsNullOrEmpty(entireFilePath))
            {
                //Upload the Facture
                gEDJsonClassRequestResponse = UploadFile(factureBarCode, entireFilePath, GedService.DocTypesEnum.FAC);
                //Save into the returned list
                listGEDJsonClassRequestResponse.Add(gEDJsonClassRequestResponse);
            }
            //Loop on the annexes
            foreach (string entireAnexePath in entireAnnexeFilePathsList)
            {
                listGEDJsonClassRequestResponse.Add(
                    UploadFile(factureBarCode, entireAnexePath, GedService.DocTypesEnum.AFAC));
            }
            //All done
            return listGEDJsonClassRequestResponse;
        }
        #endregion
    }
}