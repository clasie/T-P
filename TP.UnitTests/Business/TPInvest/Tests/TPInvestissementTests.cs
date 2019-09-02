using Models.ComptaPlusModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TP.UnitTests.Business.TPInvest.Builders;
using Xunit;


namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// TPInvest - Investissements
    /// </summary>
    public partial class TPTests
    {


        [Fact]
        public void TPInvest_Investissement_TestFalse_VerificationProprieteIsDeletedCreationInvestissement()
        {
            try
            {
                var invest = InvestissementBuilder.GenererInvestissement("IMO201908", "Side Construction");

                Assert.False(invest.IsDeleted);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}