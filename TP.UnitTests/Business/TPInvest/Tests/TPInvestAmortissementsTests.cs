using Models.ComptaPlusModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TP.UnitTests.Business.TPInvest.Builders;
using Xunit;


namespace TP.UnitTests.Business.TPTestUnitaires
{
    public partial class TPTests
    {

        [Fact]
        public void TPInvest_Amortissement_TestFalse_VerificationProprieteIsDeletedCreationAmortissement()
        {
            try
            {
                var amortissement = AmortissementBuilder.GenererAmortissement("Side Construction", "20190826-4785692");

                Assert.False(amortissement.IsDeleted);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

    }
}
