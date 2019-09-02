//using DevExpress.Xpo.DB;
//using Models.ComptaPlusModel;
using ComptaPlusConfig;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Models.GTPModel;
using PriorityQueue.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Constants;
using TokenHandler.Utils;

namespace PriorityQueue.Model
{
    /// <summary>
    /// Link with DB 
    /// </summary>
    public class GTPAccessWs
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion

        public static void InsertAccessWs(
            Guid transactionGuid, 
            string token, 
            GTPUser gtpUser, 
            string commentaire,
            BusinessConfig businessConfig)
        {
            try {
                //ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = businessConfig.GetSession(ComptaPlusConfig.Constants.GTPKey);
                //Session session = new Session(XpoDefault.DataLayer);
                {
                    User user = new XPQuery<User>(session).SingleOrDefault(q => q.Id.CompareTo(gtpUser.Id) == 0);
                    var access = new Models.GTPModel.Access(session);
                    access.DtCrea = DateTime.Now;
                    access.Id = transactionGuid;
                    access.Comment = commentaire;
                    access.Token = token;
                    access.User = user;
                    access.IsActive = true;
                    access.Save();
                    //session.DataLayer.Dispose();
                    //businessConfig.DisposeSessions();
                }
                /*
                BusinessConfig config = new BusinessConfig();
                ConnectionHelper.Connect(AutoCreateOption.None);
                //Session session = new Session(XpoDefault.DataLayer);
                Session session = config.GetSession("001");
                {
                    User user = new XPQuery<User>(session).SingleOrDefault(q => q.Id.CompareTo(gtpUser.Id) == 0);
                    var access = new Models.GTPModel.Access(session);
                    access.DtCrea = DateTime.Now;
                    access.Id = transactionGuid;
                    access.Description = commentaire;
                    access.Token = token;
                    access.User = user;
                    access.IsActive = true;
                    access.Save();
                    session.DataLayer.Dispose();
                }
                config.DisposeSessions();
                 */
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPAccessWs",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
        }
        /// <summary>
        /// Abandonned methods, will be managed by DB jobs and DB backups.
        /// </summary>
        /// <param name="dayAmount"></param>
        private static void DeleteAccessWs(int dayAmount)
        {
            try
            {
                //var oldDateFromWhichToDelete = DateTime.Now.AddDays(-dayAmount);
                //ConnectionHelper.Connect(AutoCreateOption.None);
                //Session session = new Session(XpoDefault.DataLayer);
                //{
                //    IQueryable<Access> ps = new XPQuery<Access>(session).Where(q => q.DtCrea < oldDateFromWhichToDelete);
                //    foreach (Access p in ps)
                //    {
                //        //p.Delete();
                //    }
                //    session.DataLayer.Dispose();
                //}
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPAccessWs",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
        }
    }
}
