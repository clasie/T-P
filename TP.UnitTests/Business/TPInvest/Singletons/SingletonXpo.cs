using DevExpress.Xpo;
using Models.ComptaPlusModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.UnitTests.Utils.SingletonBase
{
    public partial class SingletonXpo
    {

        /// <summary>
        /// Retourne une nouvelle instance "Investissement"
        /// </summary>
        /// <returns></returns>
        public Investissement GetInvestissement()
        {
            return new Investissement(Sess);
        }

        /// <summary>
        /// Retourne une nouvelle instance "Investissement" sur base de son identifiant
        /// </summary>
        /// <param name="id">Id investissement</param>
        /// <returns></returns>
        public Investissement GetInvestissementById(Guid id)
        {
            return (from inv in new XPQuery<Investissement>(Instance.Sess)
                    where inv.Id == id
                    select inv).FirstOrDefault();
        }


        /// <summary>
        /// Retourne une nouvelle instance "Amortissements"
        /// </summary>
        /// <returns></returns>
        public Amortissements GetAmortissement()
        {
            return new Amortissements(Sess);
        }
    }
}