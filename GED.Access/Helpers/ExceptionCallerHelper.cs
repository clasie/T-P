using GED.Access.Const;
using GED.Access.CustomExceptions;
using GED.Access.GEDJsonClasses;
using GED.Access.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GED.Access.Helpers
{
    public class ExceptionCallerHelper
    {
        #region External exception
        /// <summary>
        /// Used to manage the exceptions to throw to the user
        /// </summary>
        private static List<string> ExternalErrorMessagesUploadFile = new List<string>{
            GedConstants.MissingOrInvalidToken,
            GedConstants.UnknownDocType,
            GedConstants.MissingFile,
            GedConstants.MissingDocType,
            GedConstants.MissingBarCode,
            GedConstants.MissingProvider,
            GedConstants.MissingMajorVersion,
            GedConstants.MissingMinorVersion,
        };
        #endregion

        #region Internal Exception
        /// <summary>
        /// Used to manage the exceptions inside the dll
        /// </summary>
        private static List<string> InternalErrorMessagesUploadFile = new List<string>{
            GedConstants.MissingOrInvalidToken,
        };
        #endregion

        #region Public Method Analyse GED Answer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="responsebody"></param>
        /// <returns></returns>
        public static GedTokenAnswer TokenTestExecute(string responsebody)
        {
            GEDJsonClassRequestResponse gedJsonClassTokenRequest;
            try
            {
                //Convert json to object
                gedJsonClassTokenRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<GEDJsonClassRequestResponse>(responsebody);
            }
            catch (Exception ex)
            {
                throw new GEDConnectionUnknownException(
                    $"{TP.Resources.Common.CommonResources.Erreur_TentativeParsingWebAnswer} [ {ex.ToString()} ]");
            }
            //We got no token nor date
            if (gedJsonClassTokenRequest.error != null)
            {
                //Wrong creds
                if (gedJsonClassTokenRequest.error.Equals(GedConstants.WrongCreds))
                {
                    throw new GEDInvalidCredentialsException(gedJsonClassTokenRequest.error);
                }
                //Too many attemps
                else if (gedJsonClassTokenRequest.error.Equals(GedConstants.TooManyAttempts))
                {
                    throw new GEDUserLockedTooMuchFailingAttempsToConnectException(gedJsonClassTokenRequest.error);
                }
                //Unknown answer
                else
                {
                    throw new GEDConnectionUnknownException(gedJsonClassTokenRequest.error);
                }
            }
            return new GedTokenAnswer() { Token = gedJsonClassTokenRequest.token, ExpirationDate = gedJsonClassTokenRequest.expiration };
        }
        #endregion

        #region Public Method Analyse GED Answer
        /// <summary>
        /// Generates custom exceptions to the user or to the internal dll
        /// </summary>
        /// <param name="responsebody"></param>
        /// <returns>GEDAnswer</returns>
        public static GEDJsonClassRequestResponse UploadFileTestExecute(string responsebody, int bytesOrinal = GedConstants.MinSizeExpected)
        {
            //Convert json to object
            GEDJsonClassRequestResponse gedJsonClassTokenRequest =
                Newtonsoft.Json.JsonConvert.DeserializeObject<GEDJsonClassRequestResponse>(responsebody);
            //Afin de testing
            gedJsonClassTokenRequest.sizeExpected = bytesOrinal;

            //Something wrong happened when uploading file on the GED
            //bool x = gedJsonClassTokenRequest.error != null;
            //bool x2 = gedJsonClassTokenRequest.error != "null";
            if(gedJsonClassTokenRequest.error != null && gedJsonClassTokenRequest.error != "null")
            {
                //MissingOrInvalidToken -> retry to refresh the token till 
                //
                //   -> TooManyAttempts exception:
                //
                //          This exception will be catch internally because the dll have to manage a retry with a new token
                //          by itself -> if at the end the user is blocked another exception is responsible 
                //          to alert the user of this dll.
                //          Anyway the token calls are alloed maximum 2 time according to the folloing constant:
                //           -> GedConstants.LoopGetTokenMaxConterValue.
                //
                if (InternalErrorMessagesUploadFile.Any(s => s.Contains(gedJsonClassTokenRequest.error)))
                {
                    throw new GEDTokenException(gedJsonClassTokenRequest.error);
                }
                //UploadFiles file exceptions to the final user
                else if (ExternalErrorMessagesUploadFile.Any(s => s.Contains(gedJsonClassTokenRequest.error)))
                {
                    throw new GEDUploadFileException(gedJsonClassTokenRequest.error);
                }
                //Unknown error
                else
                {
                    throw new GEDUploadFileUnknownException(gedJsonClassTokenRequest.error);
                }
            }
            return gedJsonClassTokenRequest; // new GedUploadAnswer() { RawResponseBody = responsebody };
        }
        #endregion
    }
}