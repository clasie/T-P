using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Entities.GTP;
using TP.Entities.TPFact;
using TP.SessionManager;
using TP.SSOHelper;
using TP.UnitTests.Utils;
using Xunit;

namespace TP.UnitTests.Tests
{
    [TestCaseOrderer("TP.UnitTests.Utils.PriorityOrderer", "TP.UnitTests.UITests")]
    public class FactureEnteteTest
    {
        [Fact, Priority(1)]
        public void TestValidateBlIsFalse()//PQ False?
        {
            try
            {
                #region Arrange
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes;
                var client = ConstantsTPFact.ClientValidName;
                factureEntetes.EstActif = false;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = ConstantsTPFact.FactureEntetesMontantHTVA;
                #endregion

                #region Act
                bool testsValue = factureEntetes.ValidateBL();
                #endregion

                #region Assert
                Assert.True(testsValue);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        [Fact]
        public void TestSaveAndGetBackFactureEnteteDb()
        {
            try
            {
                #region Arrange
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes;
                var client = ConstantsTPFact.ClientValidName;
                var conditionPaiement = SingletonTPFact.Instance.ConditionPaiement;
                factureEntetes.IdConditionPaiement = conditionPaiement;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = (decimal)10.5;
                factureEntetes.Session.BeginTransaction();
                factureEntetes.Save();
                factureEntetes.Session.CommitTransaction();
                var idEntFAct = factureEntetes.Id;
                #endregion

                #region Act
                var factureEntetesBack =
                    (from pc in new XPQuery<FactureEntetes>(SingletonTPFact.Instance.Sess)
                     where pc.Id == idEntFAct
                     select pc).FirstOrDefault();
                #endregion

                #region Assert
                Assert.Equal(idEntFAct, factureEntetesBack.Id);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
        [Fact]
        public void TestSaveAndGetBackValidFactureEnteteDbBecauseClientNameIsStringEmptyInserted()
        {
            try
            {
                #region Arrange
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes;
                var client = string.Empty;//the value tested
                var conditionPaiement = SingletonTPFact.Instance.ConditionPaiement;
                factureEntetes.IdConditionPaiement = conditionPaiement;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = (decimal)10.5;
                factureEntetes.Session.BeginTransaction();
                factureEntetes.Save();
                factureEntetes.Session.CommitTransaction();
                var idEntFAct = factureEntetes.Id;
                #endregion

                #region Act
                var factureEntetesBack =
                    (from pc in new XPQuery<FactureEntetes>(SingletonTPFact.Instance.Sess)
                     where pc.Id == idEntFAct
                     select pc).FirstOrDefault();
                #endregion

                #region Assert
                Assert.True(factureEntetesBack.ValidateBL());
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        [Fact]
        public void TestSaveAndGetBackNotValidInDbValidFactureEnteteBecauseClientNameIsNullInserted()
        {
            try
            {
                #region Arrange 
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes; //new FactureEntetes(Sess);
                string client = null;
                var conditionPaiement = SingletonTPFact.Instance.ConditionPaiement;
                factureEntetes.IdConditionPaiement = conditionPaiement;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = (decimal)10.5;
                factureEntetes.Session.BeginTransaction();
                factureEntetes.Save();
                factureEntetes.Session.CommitTransaction();
                var idEntFAct = factureEntetes.Id;
                #endregion

                #region Act
                var factureEntetesBack =
                    (from pc in new XPQuery<FactureEntetes>(SingletonTPFact.Instance.Sess)
                     where pc.Id == idEntFAct
                     select pc).FirstOrDefault();
                #endregion

                #region Assert
                Assert.True(factureEntetesBack.ValidateBL());
                Assert.Equal(ConstantsTPFact.ClientInvalidSystemName, factureEntetesBack.ClientNom);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        [Fact]
        public void TestValidateBlIsTrue()
        {
            try
            {
                #region Arrange
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes;
                var client = ConstantsTPFact.ClientValidName;
                var conditionPaiement = SingletonTPFact.Instance.ConditionPaiement;

                factureEntetes.IdConditionPaiement = conditionPaiement;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = (decimal)10.5;
                #endregion

                #region Act
                bool testsValue = factureEntetes.ValidateBL();
                #endregion

                #region Assert
                Assert.True(testsValue);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }

    [TestCaseOrderer("TP.UnitTests.Utils.PriorityOrderer", "TP.UnitTests.UITests")]
    public class FactureEntete2Test
    {
        [Fact]
        public void TestValidateBlIsTrue()
        {
            try
            {
                #region Arrange
                var factureEntetes = SingletonTPFact.Instance.FactureEntetes;
                var client = ConstantsTPFact.ClientValidName;
                var conditionPaiement = SingletonTPFact.Instance.ConditionPaiement;
                factureEntetes.IdConditionPaiement = conditionPaiement;
                factureEntetes.ClientNom = client;
                factureEntetes.MontantHTVA = ConstantsTPFact.FactureEntetesMontantHTVA;
                #endregion

                #region Act
                bool testsValue = factureEntetes.ValidateBL();
                #endregion

                #region Assert
                Assert.True(testsValue);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}
