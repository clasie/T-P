using System;
using TP.Entities.TPFact;
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
        /// Test DB: création FactureDetails et vérification récupération de l'entité sur base de l'Id
        /// </summary>
        [Fact, Priority(10)]
        public void TPFact_FactureDetails_TestTrue_CreationDetails()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                SingletonXpo.Instance.CommitTransaction(factureDetail);
                #endregion

                #region Act
                var factureDetailBack = SingletonXpo.Instance.GetFactureDetailById(factureDetail.Id);
                #endregion

                #region Assert
                Assert.Equal(factureDetail.Id, factureDetailBack.Id);
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
        /// Test DB: ajout factureDetail à factureEntete + Vérification calcul montant HTVA
        /// </summary>
        [Fact, Priority(11)]
        public void TPFact_FactureDetails_TestTrue_CalculMontantsHorsTVA()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                factureEntete.MontantHTVA = 0;

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                SingletonXpo.Instance.CommitTransaction(factureDetail);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                var factureDetailBack = SingletonXpo.Instance.GetFactureDetailById(factureDetail.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.Equal(factureDetail.Id, factureDetailBack.Id);
                Assert.Equal(factureEnteteBack.MontantHTVA, (factureDetailBack.MontantHTVA * factureDetailBack.Quantité));
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
        /// Test DB: création 2 FactureDetails + liaison avec FactureEntetes et vérification récupération FactureDetails après création
        /// </summary>
        [Fact, Priority(12)]
        public void xTPFact_FactureDetails_TestTrue_CreationDe2FactureDetails()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail1 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail2 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                //Ajout des details
                factureEntete.Details_Facture.Add(factureDetail1);
                factureEntete.Details_Facture.Add(factureDetail2);

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.Equal(ConstantsTPFact.DetailsAmountExpected, factureEnteteBack.Details_Facture.Count);
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
        /// Test DB: création 3 FactureDetails + liaison avec FactureEntetes et vérification de la correspondance du Total des Details avec le MontantHTVA de l'entête
        /// </summary>
        [Fact, Priority(13)]
        public void TPFact_FactureDetails_TestTrue_CreationFactureDetailsWithCheckTotal()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail1 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail2 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail3 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                factureEntete.MontantHTVA = 0;

                //Ajout des details
                factureEntete.Details_Facture.Add(factureDetail1);
                factureEntete.Details_Facture.Add(factureDetail2);
                factureEntete.Details_Facture.Add(factureDetail3);

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);

                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert

                decimal ? totalDetails = 0;
                foreach (FactureDetails detail in factureEnteteBack.Details_Facture)
                {
                    totalDetails += detail.MontantHTVA * detail.Quantité;
                }

                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.Equal(factureEnteteBack.MontantHTVA, totalDetails);

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
        /// Création FactureDetails dont montant unitaire est negatif
        /// </summary>
        [Fact, Priority(14)]
        public void TPFact_FactureDetails_TestFalse_CreationFactureDetailsWithNegativeAmount()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail1 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                //Ajout des details
                factureEntete.Details_Facture.Add(factureDetail1);

                factureDetail1.MontantHTVA = ConstantsTPFact.FactureDetailMontantHTVANegatif;
                factureDetail1.Quantité = ConstantsTPFact.FactureDetailQuantite;

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.False(factureDetail1.ValidateBL());
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
        /// Test DB: création 4 FactureDetails + liaison avec FactureEntetes et vérification de la correspondance du Total des Details avec le MontantHTVA de l'entête avec Diverses opérations
        /// </summary>
        [Fact, Priority(15)]
        public void TPFact_FactureDetails_TestTrue_Creation4FactureDetailsWithMultipleOperations()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail1 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail2 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail3 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail4 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                //Ajout des details
                factureEntete.MontantHTVA = 0;

                decimal? totalDetails = 0;
                foreach (Entities.TPFact.FactureDetails detail in factureEntete.Details_Facture)
                {
                    totalDetails += detail.MontantHTVA * detail.Quantité;
                }

                factureDetail3.Delete();

                totalDetails -= factureDetail3.MontantHTVA * factureDetail3.Quantité;

                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.Equal(factureEntete.MontantHTVA, totalDetails);
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
        /// Test DB: création 4 FactureDetails (Montant HardCodé) + liaison avec FactureEntetes et vérification de la correspondance du Total des Details avec le MontantHTVA de l'entête avec Diverses opérations dont un update de détails
        /// </summary>
        [Fact, Priority(16)]
        public void TPFact_FactureDetails_TestTrue_Creation4FactureDetailsWithMultipleOperationsAndUpDateDetails()
        {
            try
            {
                #region Arrange
                //FactureEntete
                var factureEntete = FactureBuilder.GenererFactureEntete(ConstantsTPFact.ClientValidName, SingletonXpo.Instance.GetConditionPaiement());

                //FactureDetail
                var factureDetail1 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail2 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail3 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                var factureDetail4 = FactureBuilder.GenererFactureDetails(factureEntete, ConstantsTPFact.FactureDetailMontantHTVA,
                    ConstantsTPFact.FactureDetailQuantite);

                //Ajout des details
                factureEntete.MontantHTVA = 0;
                factureEntete.Details_Facture.Add(factureDetail1);
                factureEntete.Details_Facture.Add(factureDetail2);
                factureEntete.Details_Facture.Add(factureDetail3);
                factureEntete.Details_Facture.Add(factureDetail4);

                factureDetail1.MontantHTVA = 15.5M;
                factureDetail1.Quantité = 30;

                factureDetail2.MontantHTVA = 200;
                factureDetail2.Quantité = 3;

                factureDetail3.MontantHTVA = 60;
                factureDetail3.Quantité = 60;

                factureDetail4.MontantHTVA = 1000;
                factureDetail4.Quantité = 9;

                factureDetail3.Delete();

                factureDetail2.MontantHTVA = 400;


                //Commit
                SingletonXpo.Instance.CommitTransaction(factureEntete);
                #endregion

                #region Act
                var factureEnteteBack = SingletonXpo.Instance.GetFactureEnteteById(factureEntete.Id);
                #endregion

                #region Assert
                Assert.Equal(factureEntete.Id, factureEnteteBack.Id);
                Assert.Equal(10665, factureEntete.MontantHTVA);
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