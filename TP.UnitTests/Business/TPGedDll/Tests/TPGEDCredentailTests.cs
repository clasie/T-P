using GED.Access;
using GED.Access.CustomExceptions;
using GED.Access.Utils;
using System;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Utils;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TPTests
    {

        /// <summary>
        /// Teste si avec des credentials valable nous reçevons bien un token.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_Credentials_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GedTokenAnswer answerToken = new GedTokenAnswer();
                bool passed = false;
                var gedAccess = new Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPasswordWEB,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                #endregion

                #region Act: Aller chercher le token
                try
                {
                    answerToken = gedAccess.GetConnectionToken();
                    //Tous s'est bien passé
                    passed = true;
                }
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
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                
                #endregion

                #region Assert
                //Pas d'exceptions levées
                Assert.True(passed);
                //Token non vide nécessaire
                Assert.True(!string.IsNullOrEmpty(answerToken.Token));
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
        /// Teste si avec de mauvais credentials une custom exception est bien levée.
        /// Attention à ne pas juste tester un mauvais pw avec un bon user afin d'éviter
        /// de le locker pour le reste des tests.
        /// </summary>
        [Fact, Priority(1)]
        public void TPGED_Credentials_TestFalse()
        {
            try
            {
                #region Arrange: Init instance
                GedTokenAnswer answerToken = new GedTokenAnswer();
                bool passed = false;
                var gedAccess = new GED.Access.Ged(
                        ConstantsTPGed.GedWrongUserLogin,
                        ConstantsTPGed.GedWrongUserPassword,
                        ConstantsTPGed.GedUrlToTestWEB
                    );
                #endregion

                #region Act: Aller chercher le token
                try
                {
                    answerToken = gedAccess.GetConnectionToken();
                    //Tous s'est bien passé
                    passed = false;
                }
                //Mauvais credentials (attention après 2 erreurs le user est locké)
                catch (GEDInvalidCredentialsException invalidCredentialsException)
                {
                    passed = true;
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
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion

                #region Assert
                //Exception attendue levée (test sur un false => !true est ok)
                Assert.False(!passed);
                //Token doit être vide (test sur un false => !true est ok)
                Assert.False(!string.IsNullOrEmpty(answerToken.Token));
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
        /// On essaye d'avoir des creds sur une mauvaise url.
        /// </summary>
        [Fact, Priority(1)]
        public void TPGED_CredentialsWithWrongGedWebUrl_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                GedTokenAnswer answerToken = new GedTokenAnswer();
                bool passed = false;
                var gedAccess = new GED.Access.Ged(
                        ConstantsTPGed.GedUserLogin,
                        ConstantsTPGed.GedUserPassword,
                        ConstantsTPGed.GEDWronWebServiceUrl
                    );
                #endregion

                #region Act: Aller chercher le token
                try
                {
                    answerToken = gedAccess.GetConnectionToken();
                    //Tous s'est bien passé
                    passed = false;
                }
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
                    passed = true;
                }
                //Erreur Dll GED inconnue
                catch (GEDGlobalUnknownException gEDGlobalUnknownException)
                {
                    passed = false;
                }
                #endregion

                #region Assert
                //Exception attendue levée
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
    }
}