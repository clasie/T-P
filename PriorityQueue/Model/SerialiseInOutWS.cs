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
    public class SerialiseInOutWS
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion
        /// <summary>
        /// Stores inside SerialiseInOutWS table
        /// </summary>
        /// <param name="transactionGuid"></param>
        /// <param name="serializIn"></param>
        /// <param name="serializOut"></param>
        /// <param name="token"></param>
        public static void InsertAccessWs(
            Guid transactionGuid, 
            string serializIn, 
            string serializOut, 
            string token,
            BusinessConfig businessConfig)
        {
            //return;
            try
            {
                //ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = businessConfig.GetSession(ComptaPlusConfig.Constants.GTPKey);
                //Session session = new Session(XpoDefault.DataLayer);
                //{
                var serializedInOut = new Models.GTPModel.SerializedInOut(session);
                    serializedInOut.DtCrea = DateTime.Now;
                    serializedInOut.Id = transactionGuid;
                    serializedInOut.ObjectsIn = serializIn;
                    serializedInOut.ResponsesOut = serializOut;
                    serializedInOut.Token = token;
                    serializedInOut.IsActive = true;
                    serializedInOut.Save();
                    //serializedInOut.Session.DataLayer.Dispose();
                    //session.Disconnect();
                    //session.DataLayer.Dispose();
                    //businessConfig.DisposeSessions();
                //}
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "SerialiseInOutWS",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
        }
    }
}
