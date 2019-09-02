using DevExpress.Xpo;
using System;
using TP.Entities.Garage.Centralisation;
using TP.Entities.GTP;
using TP.Entities.TPFact;
using TP.UnitTests.Utils;
using TP.UnitTests.Utils.SingletonBase;

namespace TP.UnitTests.Business.TPFact.Builders
{
    /// <summary>
    /// FactureBuilder
    /// </summary>
    public static class FactureBuilder
    {
        /// <summary>
        /// Méthode pour générer une instance de factureEntete
        /// </summary>
        /// <param name="nomClient">Nom du client</param>
        /// <param name="estActif">estActif toujours à false par défaut</param>
        /// <returns>Instance de FactureEntetes</returns>
        public static FactureEntetes GenererFactureEntete(string nomClient, bool estActif = false)
        {
            var factureEntete = SingletonXpo.Instance.GetFactureEntete();
            factureEntete.EstActif = estActif;
            factureEntete.ClientNom = nomClient;
            factureEntete.MontantHTVA = 0;
            return factureEntete;
        }

        /// <summary>
        /// Méthode pour générer une instance de factureEntete avec condition de paiement
        /// </summary>
        /// <param name="nomClient">Nom du client</param>
        /// <param name="conditionPaiement">Condition de paiement</param>
        /// <param name="estActif">estActif toujours à false par défaut</param>
        /// <returns>Instance de FactureEntetes</returns>
        public static FactureEntetes GenererFactureEntete(string nomClient, ConditionPaiement conditionPaiement, bool estActif = false)
        {
            var factureEntete = SingletonXpo.Instance.GetFactureEntete();
            factureEntete.EstActif = estActif;
            factureEntete.ClientNom = nomClient;
            factureEntete.IdConditionPaiement = conditionPaiement;
            return factureEntete;
        }

        /// <summary>
        /// Méthode pour générer une instance de FactureDetails
        /// </summary>
        /// <param name="createArticle">Détermine si un article doit être lié au détail</param>
        /// <param name="isNewRecord">Détermine si c'est un NewRecord</param>
        /// <returns></returns>
        public static FactureDetails GenererFactureDetails(bool createArticle = true, bool isNewRecord = true)
        {
            var factureDetail = SingletonXpo.Instance.GetFactureDetail();

            if (createArticle)
            {
                factureDetail.IdArticle = GenererFactureArticle();
            }
            
            factureDetail.IsNewRecord = isNewRecord;
            return factureDetail;
        }

        /// <summary>
        /// Méthode pour générer une instance de FactureDetails
        /// </summary>
        /// <param name="montantHtva">Montant Htva</param>
        /// <param name="isNewRecord">Détermine si c'est un NewRecord en DB</param>
        /// <returns></returns>
        public static FactureDetails GenererFactureDetails(decimal montantHtva, bool isNewRecord = true)
        {
            var factureDetail = SingletonXpo.Instance.GetFactureDetail();
            factureDetail.IdArticle = GenererFactureArticle();
            factureDetail.MontantHTVA = montantHtva;            
            factureDetail.IsNewRecord = isNewRecord;
            return factureDetail;
        }

        /// <summary>
        /// Méthode pour générer une instance de FactureDetails
        /// </summary>
        /// <param name="montantHtva">Montant Htva</param>
        /// <param name="quantite">Quantité</param>
        /// <param name="isNewRecord">Détermine si c'est un NewRecord en DB</param>
        /// <returns></returns>
        public static FactureDetails GenererFactureDetails(decimal montantHtva, int quantite, bool isNewRecord = true)
        {
            var factureDetail = SingletonXpo.Instance.GetFactureDetail();
            factureDetail.IdArticle = GenererFactureArticle();
            factureDetail.MontantHTVA = montantHtva;
            factureDetail.Quantité = quantite;            
            factureDetail.IsNewRecord = isNewRecord;
            return factureDetail;
        }

        /// <summary>
        /// Méthode pour générer une instance de FactureDetails
        /// </summary>
        /// <param name="entete">FactureEntete à lier au détail</param>
        /// <param name="montantHtva">Montant Htva</param>
        /// <param name="quantite">Quantité</param>
        /// <param name="isNewRecord">Détermine si c'est un NewRecord en DB</param>
        /// <returns></returns>
        public static FactureDetails GenererFactureDetails(FactureEntetes entete, decimal montantHtva, int quantite, bool isNewRecord = true)
        {
           // var cCout = SingletonXpo.Instance.GetCentreCout().IdXPO;
            var factureDetail = SingletonXpo.Instance.GetFactureDetail();
            factureDetail.IdFactureEntete = entete;
            factureDetail.IdArticle = GenererFactureArticle();            
            factureDetail.MontantHTVA = montantHtva;
            factureDetail.Quantité = quantite;            
            factureDetail.IsNewRecord = isNewRecord;
            factureDetail.IdCentreCout = Guid.NewGuid();
            return factureDetail;
        }

        /// <summary>
        /// Méthode pour générer une instance de ArticlesComptable
        /// </summary>
        /// <returns>Instance de ArticlesComptable</returns>
        public static ArticlesComptable GenererFactureArticle()
        {
            var article = SingletonXpo.Instance.GetArticlesComptable();
            return article;
        }
    }
}