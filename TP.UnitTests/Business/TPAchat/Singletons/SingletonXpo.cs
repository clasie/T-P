using System;
using System.Linq;
using DevExpress.Xpo;
using TP.Entities.Garage.Centralisation;
using TP.Entities.GTP;
using TP.SessionManager;

namespace TP.UnitTests.Utils.SingletonBase
{
    public partial class SingletonXpo
    {
        /// <summary>
        /// Retourne une nouvelle instance FactureEntetes
        /// </summary>
        /// <returns>FactureEntetes</returns>
        public GroupeFournisseur GetGroupeFournisseur()
        {
            return new GroupeFournisseur(Sess);
        }

        internal TRES_CENTRECOUT GetCentreCout()
        {
            return null; // (from pc in new XPQuery<TRES_CENTRECOUT>(XPOController.DataSQL_Session) select pc).FirstOrDefault();
        }
    }
}