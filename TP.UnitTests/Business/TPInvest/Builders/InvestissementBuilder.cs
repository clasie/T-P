using Models.ComptaPlusModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TP.UnitTests.Utils.SingletonBase;

namespace TP.UnitTests.Business.TPInvest.Builders
{
    public static class InvestissementBuilder
    {

        public static Investissement GenererInvestissement(string name, string company)
        {
            var investissement = SingletonXpo.Instance.GetInvestissement();
            investissement.Name = name;
            investissement.Company = company;
            return investissement;
        }

    }
}
