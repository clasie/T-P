using System;
using System.Linq;
using DevExpress.Xpo;
using TP.Entities.GTP;
using TP.Entities.TPFact;

namespace TP.UnitTests.Utils.SingletonBase
{
    public partial class SingletonXpo
    {        
        /// <summary>
        /// Retourne une nouvelle instance FactureEntetes
        /// </summary>
        /// <returns>FactureEntetes</returns>
        public FactureEntetes GetFactureEntete()
        {
            return new FactureEntetes(Sess);
        }

        /// <summary>
        /// Retourne la FactureEntête trouvée sur base de son Id
        /// </summary>
        /// <param name="id">Id de la facture entête</param>
        /// <returns>FactureEntetes</returns>
        public FactureEntetes GetFactureEnteteById(Guid id)
        {
            return (from pc in new XPQuery<FactureEntetes>(Instance.Sess)
                    where pc.Id == id
                    select pc).FirstOrDefault();
        }

        /// <summary>
        /// Retourne une nouvelle instance FactureDetails
        /// </summary>
        /// <returns>FactureDetails</returns>
        public FactureDetails GetFactureDetail()
        {
            return new FactureDetails(Sess);
        }

        /// <summary>
        /// Retourne la FactureDetails trouvée sur base de son Id
        /// </summary>
        /// <param name="id">Id de la FactureDetails</param>
        /// <returns>FactureDetails</returns>
        public FactureDetails GetFactureDetailById(Guid id)
        {
            return (from pc in new XPQuery<FactureDetails>(Instance.Sess)
                    where pc.Id == id
                    select pc).FirstOrDefault();
        }

        /// <summary>
        /// Retourne une nouvelle instance FactureDetails
        /// </summary>
        /// <returns></returns>
        public ArticlesComptable GetArticlesComptable()
        {
            return (from pc in new XPQuery<ArticlesComptable>(Instance.Sess)
                    select pc).FirstOrDefault();
        }

        /// <summary>
        /// Retourne la première condition de paiement trouvée
        /// </summary>
        /// <returns></returns>
        public ConditionPaiement GetConditionPaiement()
        {
            //            var CompteurBrouillonValue = new XPQuery<FactureEntetes>(Sess, true).
            //OrderByDescending(f => f.CompteurBrouillon).Select(f => f.CompteurBrouillon)
            //.FirstOrDefault() + 1;
            //            CompteurBrouillonValue++;

            //var y = (from pc in new XPQuery<ConditionPaiement>(Sess) select pc).FirstOrDefault();
            //var x2 = (from pc in new XPQuery<FactureDetails>(Sess) select pc).FirstOrDefault();
            //var x1 = (from pc in new XPQuery<FactureEntetes>(Sess) select pc).FirstOrDefault();
            
            

            return (from pc in new XPQuery<ConditionPaiement>(Sess) select pc).FirstOrDefault();
        }

        
    }
}