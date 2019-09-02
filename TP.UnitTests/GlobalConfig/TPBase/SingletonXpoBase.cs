using System;
using System.Linq;
using DevExpress.Xpo;
using TP.Entities.GTP;
using TP.Entities.TPFact;

namespace TP.UnitTests.Utils.SingletonBase
{
    public sealed partial class SingletonXpo
    {       
        public Session Sess = null;

        #region Singleton mecanique
        private static readonly Lazy<SingletonXpo> lazy = new Lazy<SingletonXpo>(() => new SingletonXpo());
        public static SingletonXpo Instance { get { return lazy.Value; } }
        #endregion
        /// <summary>
        /// Constructeur private
        /// </summary>
        private SingletonXpo()
        {
            Sess = TP.SessionManager.XPOController.SessionPrincipale;
        }
        /// <summary>
        /// Commit ou rollback une transaction
        /// </summary>
        /// <param name="objet">Object utilisé lors des tests (FactureEntete ... )</param>
        /// <param name="isRollback">Détermine si un rollback doit être effectué</param>
        public void CommitTransaction(Object objet, bool isRollback = false)
        {
            dynamic obj = objet;

            obj.Session.BeginTransaction();
            obj.Save();

            if (isRollback)
            {
                obj.Session.RollbackTransaction();
            }
            else
            {
                obj.Session.CommitTransaction();
            }
        }
    }
}