using GED.Access.CustomExceptions;
using GED.Access.Enums;
using GED.Access.GEDJsonClasses;
using System;
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
        /// Teste la récupération d'une facture
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_DownloadAFacturePdf_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedUploadAnswerAccess = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswer = new GEDJsonClassRequestResponse();
                bool passed = false;
                var gedAccess = new GED.Access.Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPasswordWEB,
                        ConstantsTPGed.GedUrlToTestWEB 
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Pousser la facture
                    gedUploadAnswerAccess =
                        gedAccess.UploadFile(
                            barCode,
                            fileUri,
                            GedService.DocTypesEnum.FAC);

                    //Récupérer la facture
                    gedDownloadAnswer = 
                        gedAccess.DownloadFile(
                            barCode,
                            GedService.DocTypesEnum.FAC);
                    //GED.Access.Utils.Files.DisplayTheFile(gedDownloadAnswer.);

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
                //On a bien des bytes pour le fichier
                Assert.True(gedDownloadAnswer != null);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedDownloadAnswer.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedDownloadAnswer.name));
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
        /// Teste la récupération d'une annexe
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_DownloadAFactureAnnexPdf_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GEDJsonClassRequestResponse gedAnswerUploadAnnex = new GEDJsonClassRequestResponse();
                GEDJsonClassRequestResponse gedDownloadAnswer = new GEDJsonClassRequestResponse();
                bool passed = false;
                var gedAccess = new GED.Access.Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPasswordWEB,
                        ConstantsTPGed.GedUrlToTestWEB //GedUrlToTestDVP //GedUrlToTestWEB
                    );
                var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
                var barCode = Guid.NewGuid().ToString();
                #endregion

                #region Act: Uploader le fichier
                try
                {
                    //Pousser la facture
                    gedAccess.UploadFile(
                        barCode,
                        fileUri,
                        GedService.DocTypesEnum.FAC);

                    //Upload the associated annex
                    gedAnswerUploadAnnex = gedAccess.UploadFile(
                        barCode,
                        fileUri,////!!! un autre file
                        GedService.DocTypesEnum.AFAC);

                    //Récupérer la facture
                    gedDownloadAnswer =
                        gedAccess.DownloadFile(
                            barCode,
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
                //On a bien des bytes pour le fichier
                Assert.True(gedDownloadAnswer != null);
                //Pas d'erreur retournée
                Assert.True(string.IsNullOrEmpty(gedDownloadAnswer.error));
                //Nom de fichiers non vide
                Assert.True(!string.IsNullOrEmpty(gedDownloadAnswer.name));
                //Taille du fichier non nulle pour le test
                Assert.True(gedAnswerUploadAnnex.sizeExpected > GED.Access.Const.GedConstants.MinSizeExpected);
                //Taille du fichier minimale attendue en retour
                Assert.True(gedDownloadAnswer.file.Length >= gedAnswerUploadAnnex.sizeExpected);
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