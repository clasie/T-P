using DevExpress.Xpo;
using System;
using System.Linq;
using TP.Entities.TPFact;
using TP.UnitTests.Business.TPFact.Builders;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Utils;
using TP.UnitTests.Utils.SingletonBase;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// TPFact Entête, pas d'accès DB.
    /// </summary>
    public partial class TPTests
    {
        /// <summary>
        /// Test méthode ValidateBL false
        /// </summary>
        [Fact, Priority(1)]
        public void TPFact_FactureEntete_TestFalse_ValidateBL_PasDeConditionDePaiement()
        {
            try
            {
                #region Arrange
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName);
                #endregion

                #region Act
                bool testsValue = factureEntete.ValidateBL();
                #endregion

                #region Assert
                Assert.False(testsValue);
                #endregion

                #region Clear DB
                factureEntete.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
        /// <summary>
        /// Test méthode ValidateBL true
        /// </summary>
        [Fact, Priority(2)]
        public void TPFact_FactureEntete_TestTrue_ValidateBL()
        {
            try
            {
                #region Arrange
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());              
                #endregion

                #region Act
                bool testsValue = factureEntete.ValidateBL();
                #endregion

                #region Assert
                Assert.True(testsValue);
                #endregion

                #region Clear DB
                factureEntete.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}