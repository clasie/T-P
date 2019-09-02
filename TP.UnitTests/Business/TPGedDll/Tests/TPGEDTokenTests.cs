using GED.Access;
using GED.Access.CustomExceptions;
using GED.Access.Utils;
using System;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Utils;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    public partial class TPTests
    {

        /// <summary>
        /// Teste si avec des credentials valable nous reçevons bien un token.
        /// On regarde si une date accompagne ce token.
        /// Pour l'instant impossible de faire un test sur la date c'est la date du jour avec
        /// une heure de retard sur l'heure courante.
        /// </summary>
        [Fact, Priority(0)]
        public void TPGED_Token_TestTrue()
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
                Assert.True(!string.IsNullOrEmpty(answerToken.ExpirationDate));
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