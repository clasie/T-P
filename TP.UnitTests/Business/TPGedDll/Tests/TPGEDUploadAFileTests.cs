using GED.Access;
using GED.Access.CustomExceptions;
using GED.Access.Enums;
using GED.Access.GEDJsonClasses;
using System;
using System.Collections.Generic;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Util;
using TP.UnitTests.Utils;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// Upload facture
    /// </summary>
    public partial class TPTests
    {

        /// <summary>
        /// Teste si avec des credentials valable nous sommes capable d'uploader 
        /// une nouvelle facture pdf.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadANewPdfFacture_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedUploadAnswer = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswer;

                bool passed = true;
                bool passedDoubleCheck = true;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();

                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Upload a file
                    gedUploadAnswer =
                        gedAccess.UploadFile(
                            barCode,
                            fileUri,
                            GedService.DocTypesEnum.FAC);

                    //Double verification que tout s'est bien passé:
                    //Uploader fichier -> fait
                    //downloader fichier -> 
                    gedDownloadAnswer =
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.FAC);
                    //sauver localement -> 
                    //réouvrir le fichier local -> 
                    //vérifier que sa taille est bien celle d'origine avant upload.
                    if (!gedAccess.DoubleCheckUploadedFile(gedDownloadAnswer, gedUploadAnswer.sizeExpected))
                    {
                        passedDoubleCheck = false;
                    }
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = false;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                //Pas d'exceptions levées
                Assert.True(passed);
                //test du barcode
                Assert.Equal(barCode, gedUploadAnswer.barCode);
                //Le type est bien gardé
                Assert.Equal(GedService.DocTypesEnum.FAC.ToString(), gedUploadAnswer.docType);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedUploadAnswer.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswer.name));
                //Url GET de download non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswer.url));
                //Version mineure minimum car nouvelle facture
                Assert.Equal(ConstantsTPGed.GedMinorVersionMinimum, gedUploadAnswer.minorVersion);
                //Version majeure minimum car nouvelle facture
                Assert.Equal(ConstantsTPGed.GedMajorVersionMinimum, gedUploadAnswer.majorVersion);
                //Double verification tailles avant et après sur disque
                Assert.True(passedDoubleCheck);
                
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Teste l'upload d'un facture avec une liste d'url d'annexes
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadAFactureWithListOfAnnexes_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedDownloadAnswer;

                bool passed = true;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();
                //Annexes list urls
                List<string> urlAnnexeList = new List<string>() {
                    ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfAnnexe1_ResourceFileName),
                    ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfAnnexe2_ResourceFileName) };
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    
                    //Upload a new facture with annexes.
                    List<GEDJsonClassRequestResponse> listResponsesNewFacture =
                        gedAccess.UploadFactureWithAnnexes(
                            barCode,
                            urlAnnexeList,
                            fileUri);

                    foreach (var resp in listResponsesNewFacture)
                    {
                        if (resp.error != null) {
                            passed = false;
                        }
                    }

                    //Update an existing facture with annexes.
                    List<GEDJsonClassRequestResponse> listResponsesExistingFacture =
                        gedAccess.UploadFactureWithAnnexes(
                            barCode,
                            urlAnnexeList);

                    foreach (var resp in listResponsesExistingFacture)
                    {
                        if (resp.error != null)
                        {
                            passed = false;
                        }
                    }
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = false;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                //Pas d'exceptions levées ni d'erreurs business
                Assert.True(passed);
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Teste upload d'un fichier qui n'existe pas.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadANewPdfFactureWithWrongFilePath_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedUploadAnswer = new GEDJsonClassRequestResponse();

                bool passed = true;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                //Wrong path
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfFileDoNotExists_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();

                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Upload a file
                        gedAccess.UploadFile(
                            barCode,
                            fileUri,
                            GedService.DocTypesEnum.FAC);
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = true;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }

                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                Assert.True(passed);
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Teste upload d'un fichier qui n'est pas un pdf
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadANewPdfFactureThatExistsWithWrongExtensionTxt_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                bool passed = true;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                //Wrong extension not pdf
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfFileExistsWithWrongExtensionTxt_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();

                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Upload a file
                    gedAccess.UploadFile(
                        barCode,
                        fileUri,
                        GedService.DocTypesEnum.FAC);
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = true;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }

                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                Assert.True(passed);
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Teste si avec des credentials valable nous sommes capable d'uploader 
        /// une nouvelle facture pdf deux fois et voir si 
        /// les versions majeurs ont bien changé.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadANewPdfFactureTwiceToCheckVersions_TestFalse()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedUploadAnswerSecondAccess = new GEDJsonClassRequestResponse();
                bool passed = false;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Upload a file once
                    gedAccess.UploadFile(
                        barCode,
                        fileUri,
                        GedService.DocTypesEnum.FAC);

                    //Upload the same file a second time
                    gedUploadAnswerSecondAccess =
                        gedAccess.UploadFile(
                            barCode,
                            fileUri,
                            GedService.DocTypesEnum.FAC);

                    passed = true;
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = false;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                //Pas d'exceptions levées
                Assert.True(passed);
                //test du barcode
                Assert.Equal(barCode, gedUploadAnswerSecondAccess.barCode);
                //Le type est bien gardé
                Assert.Equal(GedService.DocTypesEnum.FAC.ToString(), gedUploadAnswerSecondAccess.docType);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedUploadAnswerSecondAccess.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswerSecondAccess.name));
                //Url GET de download non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswerSecondAccess.url));
                //Version majeure NON minimum car même facture
                Assert.NotEqual(ConstantsTPGed.GedMajorVersionMinimum, gedUploadAnswerSecondAccess.majorVersion);
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Va rechercher une annexe.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_UploadOnePdfFactureAnnex_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedUploadAnswerSecondAccess = new GEDJsonClassRequestResponse();
                bool passed = false;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Upload a file once
                    gedAccess.UploadFile(
                        barCode,
                        fileUri,
                        GedService.DocTypesEnum.FAC);

                    //Upload the associated annex
                    gedUploadAnswerSecondAccess =
                        gedAccess.UploadFile(
                            barCode,
                            fileUri,///!!! un autre file?
                            GedService.DocTypesEnum.AFAC);

                    passed = true;
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = false;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                //Pas d'exceptions levées
                Assert.True(passed);
                //test du barcode
                Assert.Equal(barCode, gedUploadAnswerSecondAccess.barCode);
                //Le type est bien gardé
                Assert.Equal(GedService.DocTypesEnum.AFAC.ToString(), gedUploadAnswerSecondAccess.docType);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedUploadAnswerSecondAccess.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswerSecondAccess.name));
                //Url GET de download non vide
                Assert.True(!string.IsNullOrEmpty(gedUploadAnswerSecondAccess.url));
                //Url GET de download doit contenir la string "annexe=true"
                Assert.True(gedUploadAnswerSecondAccess.url.Contains(
                    ConstantsTPGed.ServiceDonloadUrlReceivedForAnnexMustContain));
                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Teste la concaténation des annexes avec le versionning
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_ConcatenateFacuresAndConcatenateAnnexes_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                //Upload
                GEDJsonClassRequestResponse gedAnswerUploadFact1 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedAnswerUploadFact2 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedAnswerUploadAnnex1 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedAnswerUploadAnnex2 = new GEDJsonClassRequestResponse();
                //Download
                GEDJsonClassRequestResponse gedDownloadAnswerFacture1 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswerFacture2V10 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswerFacture2V00 = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswerAnnexeConcatenee = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswerAnnexeOriginalev00 = new GEDJsonClassRequestResponse();
                
                bool passed = true;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPasswordWEB,
                        ConstantsTPGed.GedUrlToTestWEB 
                    );
                var barCode = Guid.NewGuid().ToString();
                //Elements mesures upload
                int sizeGedAnswerUploadFact = ConstantsTPGed.MinSize;
                int sizeGedAnswerUploadAnnex1 = ConstantsTPGed.MinSize;
                int sizeGedAnswerUploadAnnex2 = ConstantsTPGed.MinSize;
                //Elements mesures download
                int sizeGedDownloadAnswerFacture1 = ConstantsTPGed.MinSize;
                int sizeGedDownloadAnswerFacture2 = ConstantsTPGed.MinSize;
                int sizeGedDownloadAnswerAnnexeConcatenee = ConstantsTPGed.MinSize;
                int sizegedDownloadAnswerAnnexeOriginalev00 = ConstantsTPGed.MinSize;
                int sizegedDownloadAnswerFacture2v10 = ConstantsTPGed.MinSize;
                int sizegedDownloadAnswerFacture2v00 = ConstantsTPGed.MinSize;
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Pousser la facture
                    gedAnswerUploadFact1 = gedAccess.UploadFile(
                        barCode,
                        ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName),
                        GedService.DocTypesEnum.FAC);

                    sizeGedAnswerUploadFact = gedAnswerUploadFact1.sizeExpected;

                    //Upload the associated annex 1
                    gedAnswerUploadAnnex1 = gedAccess.UploadFile(
                        barCode,
                        ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfAnnexe1_ResourceFileName),
                        GedService.DocTypesEnum.AFAC);

                    sizeGedAnswerUploadAnnex1 = gedAnswerUploadAnnex1.sizeExpected;

                    //Upload the associated annex 2
                    gedAnswerUploadAnnex2 = gedAccess.UploadFile(
                        barCode,
                        ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdfAnnexe2_ResourceFileName),
                        GedService.DocTypesEnum.AFAC);

                    sizeGedAnswerUploadAnnex2 = gedAnswerUploadAnnex2.sizeExpected;

                    //Récupérer la facture
                    gedDownloadAnswerFacture1 =
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.FAC);

                    sizeGedDownloadAnswerFacture1 = gedDownloadAnswerFacture1.file.Length;

                    if (gedAccess.SaveDownloadedFile(gedDownloadAnswerFacture1).StatusFileCopiedLocally)
                    {
                        Console.WriteLine(ConstantsTPGed.ConsolePassed);
                    }
                    else
                    {
                        passed = false;
                        Console.WriteLine(ConstantsTPGed.ConsoleNotPassed);
                    }

                    //Récupérer l'annexe concatenee, elle doit être plus lourde que la première fois.
                    gedDownloadAnswerAnnexeConcatenee =
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.AFAC);

                    sizeGedDownloadAnswerAnnexeConcatenee = gedDownloadAnswerAnnexeConcatenee.file.Length;

                    //souver localement la facture originale
                    if (gedAccess.SaveDownloadedFile(gedDownloadAnswerAnnexeConcatenee).StatusFileCopiedLocally)
                    {
                        Console.WriteLine(ConstantsTPGed.ConsolePassed);
                    }
                    else
                    {
                        passed = false;
                        Console.WriteLine(ConstantsTPGed.ConsoleNotPassed);
                    }

                    //Récupérer la première annexe v0.0
                    gedDownloadAnswerAnnexeOriginalev00 =
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.AFAC,
                            0,0);

                    sizegedDownloadAnswerAnnexeOriginalev00 = gedDownloadAnswerAnnexeOriginalev00.file.Length;

                    //L'annexe originale
                    if (gedAccess.SaveDownloadedFile(gedDownloadAnswerAnnexeOriginalev00).StatusFileCopiedLocally)
                    {
                        Console.WriteLine(ConstantsTPGed.ConsolePassed);
                    }
                    else
                    {
                        passed = false;
                        Console.WriteLine(ConstantsTPGed.ConsoleNotPassed);
                    }

                    //Tester le versionning des Factures.
                    //On ajoute une nouvelle facture avec le même code barre
                    gedAnswerUploadFact2 = gedAccess.UploadFile(
                        barCode,
                        ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf2_ResourceFileName),
                        GedService.DocTypesEnum.FAC);

                    sizeGedDownloadAnswerFacture2 = gedAnswerUploadFact2.sizeExpected;

                    ///
                    /// PAS ENCORE GERE PAR GED : version des factures
                    /// 
                    //Récupérer la dernière version facture 1.0
                    //gedDownloadAnswerFacture2V10 =
                    //    gedAccess.DownloadFile(
                    //        barCode,
                    //        GedService.DocTypesEnum.FAC);

                    //sizegedDownloadAnswerFacture2v10 = gedDownloadAnswerFacture2V10.file.Length;

                    //if (gedAccess.SaveDownloadedFile(gedDownloadAnswerFacture2V10).StatusFileCopiedLocally)
                    //{
                    //    Console.WriteLine(ConstantsTPGed.ConsolePassed);
                    //}
                    //else
                    //{
                    //    passed = false;
                    //    Console.WriteLine(ConstantsTPGed.ConsoleNotPassed);
                    //}

                    ///
                    /// PAS ENCORE GERE PAR GED : version des annexes
                    /// 

                    //Récupérer la première version 0.0
                    gedDownloadAnswerFacture2V00 =
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.FAC,0,0);

                    //sizegedDownloadAnswerFacture2v00 = gedDownloadAnswerFacture2V00.file.Length
                    if (gedAccess.SaveDownloadedFile(gedDownloadAnswerFacture2V00).StatusFileCopiedLocally)
                    {
                        Console.WriteLine(ConstantsTPGed.ConsolePassed);
                    }
                    else
                    {
                        passed = false;
                        Console.WriteLine(ConstantsTPGed.ConsoleNotPassed);
                    }
                }

                #region Gestion exception liées au credentials
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = false;
                }
                //User locké
                catch (GEDUserLockedTooMuchFailingAttempsToConnectException userLockedTooMuchFailingAttempsToConnectException)
                {
                    passed = false;
                }
                //Erreur de connexion inconnue
                catch (GEDConnectionUnknownException connectionUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées au credentials

                #region Gestion exception liées à l'upload
                //Erreur gérée lors de l'upload
                catch (GEDUploadFileException uploadFileException)
                {
                    passed = false;
                }
                //Erreur inattendue lors de l'upload
                catch (GEDUploadFileUnknownException uploadFileUnknownException)
                {
                    passed = false;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion Gestion exception liées à l'upload

                #endregion Act: Uploader le fichier

                #region Asserts
                //Pas d'exceptions levées
                Assert.True(passed);

                ///
                /// Facture
                /// 
                //On a bien des bytes
                Assert.True(sizeGedAnswerUploadFact > ConstantsTPGed.MinSize);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedDownloadAnswerFacture1.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedDownloadAnswerFacture1.name));
                //La taille de la facture uploadé doit être la même que le downloadé
                Assert.True(sizeGedDownloadAnswerFacture1.Equals(sizeGedAnswerUploadFact));

                ///
                /// Annexe 1
                /// 
                //On a bien des bytes 
                Assert.True(sizeGedAnswerUploadAnnex1 > ConstantsTPGed.MinSize);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedAnswerUploadAnnex1.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedAnswerUploadAnnex1.name));

                ///
                /// Annexe 2
                /// 
                //On a bien des bytes 
                Assert.True(sizeGedAnswerUploadAnnex2 > ConstantsTPGed.MinSize);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedAnswerUploadAnnex2.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedAnswerUploadAnnex2.name));

                ///
                /// Annexe 1 et 2
                /// 
                //La concatenation des 2 doit être supérieure ou égale à chacune des 2 prises séparément
                Assert.True(sizeGedDownloadAnswerAnnexeConcatenee >= sizeGedAnswerUploadAnnex1);
                Assert.True(sizeGedDownloadAnswerAnnexeConcatenee >= sizeGedAnswerUploadAnnex2);

                ///
                /// PAS ENCORE GERE PAR GED : version des annexes
                /// 
                /// Récup de l'annexe originale v0.0 
                /// 
                //L'annexe 1 récupée (v0.0) doit avoir la taille de l'annexe 1 originale
                //Gestion version annexes: PAS ENCORE GERE PAR GED -> mis en commentaire
                //Assert.True(sizegedDownloadAnswerAnnexeOriginalev00.Equals(sizeGedAnswerUploadAnnex1));

                ///
                /// PAS ENCORE GERE PAR GED: version des factures
                /// 
                //La facture 1 récupée (v0.0) doit avoir la taille de la facture 1 originale
                //Assert.True(sizegedDownloadAnswerFacture2v00.Equals(sizeGedDownloadAnswerFacture1));

                #endregion

                #region Clear 
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}