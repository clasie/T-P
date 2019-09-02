using System;
using TP.UnitTests.Business.TPAchat.Builders;
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
        public void TPAchat_GroupeFournisseur_TestFalse_ValidateBL_PasDeCode()
        {
            try
            {
                #region Arrange
                var groupeFournisseur = GroupeFournisseurBuilder.GenererGroupeFournisseur();
                #endregion

                #region Act
                bool testsValue = groupeFournisseur.ValidateBL();
                #endregion

                #region Assert
                Assert.False(testsValue);
                #endregion

                #region Clear DB
                groupeFournisseur.Delete();
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}