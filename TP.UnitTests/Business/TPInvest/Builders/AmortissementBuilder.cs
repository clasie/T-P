using Models.ComptaPlusModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TP.UnitTests.Utils.SingletonBase;

namespace TP.UnitTests.Business.TPInvest.Builders
{
    public static class AmortissementBuilder
    {

        public static Amortissements GenererAmortissement(string company, string transactionNumber)
        {
            var amortissement = SingletonXpo.Instance.GetAmortissement();
            amortissement.Company = company;
            amortissement.TransactionNumber = transactionNumber;
            return amortissement;
        }


    }
}