using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using TP.Entities.GTP;
using TP.Entities.TPFact;

namespace TP.UnitTests.Utils
{
    /// <summary>
    /// Ajouter ici tous les éléments de requête xpo, en effet si ils sont appelés dans 
    /// des classes de test unitaire différentes on a une levée d'exception de double key
    /// qui reste encore mystérieuse.
    /// </summary>
    public class SingletonTPFact
    {
        public DevExpress.Xpo.Session Sess = null;
        public FactureEntetes FactureEntetes = null;
        public ConditionPaiement ConditionPaiement = null;

        #region Singleton meca
        private static readonly Lazy<SingletonTPFact> lazy = new Lazy<SingletonTPFact>(() => new SingletonTPFact());
        public static SingletonTPFact Instance { get { return lazy.Value; } }
        #endregion

        private SingletonTPFact()
        {
            Sess = TP.SessionManager.XPOController.SessionPrincipale;
            FactureEntetes = new FactureEntetes(Sess);
            ConditionPaiement =
               (from pc in new XPQuery<ConditionPaiement>(Sess) select pc).FirstOrDefault();
        }

        /// <summary>
        /// Rafraîchir  la condition de paiement par défaut.
        /// </summary>
        public void RefreshCondPaiement() {
            ConditionPaiement =
                 (from pc in new XPQuery<ConditionPaiement>(Sess) select pc).FirstOrDefault();

        }
    }
}
