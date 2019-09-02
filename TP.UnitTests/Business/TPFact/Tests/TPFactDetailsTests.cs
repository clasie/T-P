using System;
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
        /// Test méthode ValidateBL False - Montant négatif
        /// </summary>
        [Theory, Priority(6)]
        [InlineData(-1)]
        [InlineData(-3915.37)]
        public void TPFact_FactureDetails_TestFalse_ValidateBLMontantNegatif(decimal montantHtva)
        {
            try
            {
                #region Arrange

                var factureDetail = FactureBuilder.GenererFactureDetails(montantHtva);

                #endregion

                #region Act
                bool val = factureDetail.ValidateBL();
                #endregion

                #region Assert
                Assert.False(val);
                #endregion

                #region Clear DB
                factureDetail.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Test méthode ValidateBL True - Montant valide
        /// </summary>
        [Theory, Priority(7)]
        [InlineData(158,9)]
        [InlineData(348751.35,3)]
        public void TPFact_FactureDetails_TestTrue_ValidateBLMontantValide(decimal montantHtva, int quantite)
        {
            try
            {
                #region Arrange

                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());
                var factureDetail = FactureBuilder.GenererFactureDetails(factureEntete, montantHtva, quantite);

                #endregion

                #region Act
                bool val = factureDetail.ValidateBL();
                #endregion

                #region Assert
                Assert.True(val);
                #endregion

                #region Clear DB
                factureDetail.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Test méthode ValidateBL False - Pas de FactureEntete
        /// </summary>
        [Fact, Priority(8)]
        public void TPFact_FactureDetails_TestFalse_ValidateBLSansFactureEntete()
        {
            try
            {
                #region Arrange

                var factureDetail = FactureBuilder.GenererFactureDetails();

                #endregion

                #region Act
                bool val = factureDetail.ValidateBL();
                #endregion

                #region Assert
                Assert.False(val);
                #endregion

                #region Clear DB
                factureDetail.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Test méthode ValidateBL True - FactureEntete valide
        /// </summary>
        [Fact, Priority(9)]
        public void TPFact_FactureDetails_TestTrue_ValidateBLAvecFactureEntete()
        {
            try
            {
                #region Arrange

                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());
                var factureDetail = FactureBuilder.GenererFactureDetails(factureEntete, 2485,6);

                #endregion

                #region Act
                bool val = factureDetail.ValidateBL();
                #endregion

                #region Assert
                Assert.True(val);
                #endregion

                #region Clear DB
                factureDetail.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        /// <summary>
        /// Test méthode ValidateBL False - Pas d'article (IdArticle) lié au détail 
        /// </summary>
        [Fact, Priority(17)]
        public void TPFact_FactureDetails_TestFalse_ValidateBLSansArticle()
        {
            try
            {
                #region Arrange

                var factureDetail = FactureBuilder.GenererFactureDetails(false);

                #endregion

                #region Act
                bool val = factureDetail.ValidateBL();
                #endregion

                #region Assert
                Assert.False(val);
                #endregion

                #region Clear DB
                factureDetail.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}