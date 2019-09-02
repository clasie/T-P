using System;
using TP.UnitTests.Business.TPFact.Builders;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Utils;
using TP.UnitTests.Utils.SingletonBase;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// TPFact Entête, avec accès DB.
    /// </summary>
    public partial class TPTests
    {
        /// <summary>
        /// Test ajout facture DB
        /// </summary>
        [Fact, Priority(3)]
        public void TPFact_FactureEntete_TestTrue_CreationFacture()
        {
            try
            {
                #region Arrange

                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
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
        /// Test ajout facture DB avec nomClient = string.Empty + test méthode ValidateBL
        /// </summary>
        [Fact, Priority(4)]
        public void TPFact_FactureEntete_TestTrue_CreationFactureAvecNomClientStringEmpty()
        {
            try
            {
                #region Arrange

                var factureEntete = FactureBuilder.GenererFactureEntete(string.Empty, SingletonXpo.Instance.GetConditionPaiement());

                SingletonXpo.Instance.CommitTransaction(factureEntete);
                
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.True(factureEnteteBack.ValidateBL());
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
        /// Test ajout facture DB quand clientNom = null + gestion correcte du business (clientNom = ERROR)
        /// </summary>
        [Fact, Priority(5)]
        public void TPFact_FactureEntete_TestTrue_CreationFactureClientNull()
        {
            try
            {
                #region Arrange

                var factureEntete = FactureBuilder.GenererFactureEntete(null, SingletonXpo.Instance.GetConditionPaiement());

                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.True(factureEnteteBack.ValidateBL());
                Assert.Equal(ConstantsTPFact.ClientInvalidSystemName, factureEnteteBack.ClientNom);
                #endregion

                #region Clear DB
                factureEnteteBack.Delete();
                #endregion

            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}