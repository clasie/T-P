using TP.Entities.GTP;
using TP.UnitTests.Utils;
using TP.UnitTests.Utils.SingletonBase;

namespace TP.UnitTests.Business.TPAchat.Builders
{
    /// <summary>
    /// FactureBuilder
    /// </summary>
    public static class GroupeFournisseurBuilder
    {
        /// <summary>
        /// Methode pour générer un groupe de fournisseur
        /// </summary>
        /// <param name="code"> Code du groupe fournisseur</param>
        /// <param name="estActif">groupe actif ?</param>
        /// <param name="libelle">Libelle du groupe fournisseur</param>
        /// <returns>Instance de FactureEntetes</returns>
        public static GroupeFournisseur GenererGroupeFournisseur(bool estActif = false)
        {

            var groupeFournisseur = SingletonXpo.Instance.GetGroupeFournisseur();
            groupeFournisseur.EstActif = estActif;
            return groupeFournisseur;
        }
        /// <summary>
        /// Methode pour générer un groupe de fournisseur
        /// </summary>
        /// <param name="code"></param>
        /// <param name="libelle"></param>
        /// <param name="estActif"></param>
        /// <returns></returns>
        public static GroupeFournisseur GenererGroupeFournisseur(string code, string libelle, bool estActif = false)
        {

            var groupeFournisseur = SingletonXpo.Instance.GetGroupeFournisseur();
            groupeFournisseur.Code = code;
            groupeFournisseur.Libelle = libelle;
            groupeFournisseur.EstActif = estActif;
            return groupeFournisseur;
        }
    }
}