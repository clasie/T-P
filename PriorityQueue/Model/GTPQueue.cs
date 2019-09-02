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
    /// Link Queue with DB 
    /// </summary>
    public class GTPQueue
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion

        #region fields
        public Guid Id { get; set; }
        public string Interface { get; set; }
        public DateTime DateCreation { get; set; }
        public double PriorityValue { get; set; }
        public string SerizdIn { get; set; }
        public string SerizdOut { get; set; }
        public string ConcatIdsObjects { get; set; }
        public int Status { get; set; }
        public QueueStatesEnum StatusEnum { get; set; }
        public string StatusEnumToString { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{Interface} ({Id.ToString()}) {PriorityValue} [{StatusEnum}] - {DateCreation}";
        }

        /// <summary>
        /// Load the que according to the Policy.        
        /// </summary>
        /// <param name="queueLoadingPolicy"></param>
        /// <returns></returns>
        public static List<Queue> GetQueue(QueueLoadingPolicy queueLoadingPolicy = null)
        {
            // TODO: Load the queue according to the Policy -CSI
            ConnectionHelper.Connect(AutoCreateOption.None);
            Session session = new Session(XpoDefault.DataLayer);
            {              
                //Default loading
                var resp = new XPQuery<Queue>(session).Where(x=>x.Status != ((int)QueueStatesEnum.Traite)).ToList();
                session.DataLayer.Dispose();
                return resp;
            }
        }
        /// <summary>
        /// Load the queue according to the Policy.        
        /// </summary>
        /// <param name="queueLoadingPolicy"></param>
        /// <returns>List<GTPQueue></returns>
        /// Called from Loader
        public static List<GTPQueue> GetAllGTPQueues(QueueLoadingPolicy queueLoadingPolicy = null)
        {
            //var value = (int)Enum.Parse(typeof(ContractEnum), contractPriority.ContractName);
            QueueStatesEnum contractEnum = (QueueStatesEnum)System.Enum.ToObject(typeof(QueueStatesEnum), 1);

            List<GTPQueue> listItemQueues = new List<GTPQueue>();
            try { 
                ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = new Session(XpoDefault.DataLayer);
                {
                    var queueItems =  new XPQuery<Queue>(session).OrderBy(x=>x.Priority).ToList();
                    foreach (var queuItem in queueItems)
                    {
                        var enumValue = (QueueStatesEnum)System.Enum.ToObject(typeof(QueueStatesEnum), queuItem.Status);
                        try
                        {
                            listItemQueues.Add(new GTPQueue()
                            {
                                DateCreation = queuItem.DtCrea,
                                Id = queuItem.Id,
                                PriorityValue = queuItem.Priority,
                                Interface = queuItem.Interface,
                                SerizdIn = queuItem.ObjectIn,
                                SerizdOut = queuItem.ObjectOut,
                                ConcatIdsObjects = queuItem.ObjectsIds,
                                Status = queuItem.Status,
                                StatusEnum = enumValue,
                                StatusEnumToString = enumValue.ToString()
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(FormatMessages.getLogMessage(
                                "GTPQueue",
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                TokenKey.NoMethodParams,
                                ex.ToString()));
                        }
                    }
                    session.DataLayer.Dispose();
                }
                return listItemQueues;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPQueue",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return listItemQueues;
            }
        }
        /// <summary>
        /// Add a new QueuItem.
        /// </summary>
        /// <param name="queueItem"></param>
        public static void AddIntoQueue(QueueItem queueItem, BusinessConfig businessConfig)
        {
            try
            {
                log.Info("IN AddIntoQueue 1.0");
                queueItem.Message = "Ok from AddIntoQueue";
                //ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = businessConfig.GetSession(ComptaPlusConfig.Constants.GTPKey);
                //Session session = new Session(XpoDefault.DataLayer);
                {
                    var queue = new Queue(session);
                    queue.DtCrea = DateTime.Now;
                    queue.Id = queueItem.IdQueue;
                    queue.RecIdERP = queueItem.IdQueue;
                    queue.ObjectIn = queueItem.FichierObjetIn;
                    queue.ObjectsIds = queueItem.ListeDesIdsAzureSeparesParUnPipe;
                    queue.Status = (int)QueueStatesEnum.PasEncoreTraite;
                    queue.Interface = queueItem.TPInterfaceID;
                    queue.IsActive = true;
                    queue.Priority = queueItem.Priority;
                    queue.Save();
                    //session.DataLayer.Dispose();
                    //businessConfig.DisposeSessions();
                }
                log.Info("IN AddIntoQueue 1.3");
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPQueue",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
        }
        /// <summary>
        /// Get the status of a QueueItem
        /// </summary>
        /// <param name="queueItem"></param>
        public static void GetStatusQueueItem(QueueItem queueItem)
        {
            try
            {
                ConnectionHelper.Connect(AutoCreateOption.None);
                using (Session session = new Session(XpoDefault.DataLayer))
                {
                    Queue r = new XPQuery<Queue>(session).SingleOrDefault(q => q.Id.CompareTo(queueItem.IdQueue) == 0);
                    queueItem.FichierObjetIn = r.ObjectIn;
                    queueItem.FichierObjetOut = r.ObjectOut;
                    queueItem.ListeDesIdsAzureSeparesParUnPipe = r.ObjectsIds;
                    queueItem.Etat = (QueueStatesEnum)r.Status;
                    session.DataLayer.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPQueue",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                queueItem.Etat = QueueStatesEnum.EnErreur;
                queueItem.Message = "Record not found";
            }
        }
        /// <summary>
        /// Update object out.
        /// </summary>
        /// <param name="queueItem"></param>
        /// Called from Loader
        public static void UpdateQueueItemStatusAndPriority(GTPQueue gTPQueue)
        {
            //queueItem.Message = "Ok from UpdateQueueItemObjectOutAndStatus";

            ConnectionHelper.Connect(AutoCreateOption.None);
            Session session = new Session(XpoDefault.DataLayer);
            {
                Queue r = new XPQuery<Queue>(session).SingleOrDefault(q => q.Id.CompareTo(gTPQueue.Id) == 0);
                r.Status = gTPQueue.Status;
                r.Priority = gTPQueue.PriorityValue;
                r.Save();
                session.DataLayer.Dispose();
            }
        }
        /// <summary>
        /// Delete one item
        /// </summary>
        /// <param name="queueItem"></param>
        /// Called from loader
        public static void DeleteQueueItem(GTPQueue gTPQueue, bool deleteAll=false)
        {
            try { 
                ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = new Session(XpoDefault.DataLayer);
                {
                    if (deleteAll) {
                        IQueryable<Queue> ps = new XPQuery<Queue>(session);
                        foreach (Queue p in ps)
                        {
                            p.Delete();
                        }
                    }
                    else
                    {
                        IQueryable<Queue> ps = new XPQuery<Queue>(session).Where(q => q.Id.CompareTo(gTPQueue.Id) == 0);
                        foreach (Queue p in ps)
                        {
                            p.Delete();
                        }
                    }
                    session.DataLayer.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPQueue",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
        }
        /// <summary>
        /// Update object out.
        /// </summary>
        /// <param name="queueItem"></param>
        public static void UpdateQueueItemObjectOutAndStatus(QueueItem queueItem)
        {
            queueItem.Message = "Ok from UpdateQueueItemObjectOutAndStatus";

            ConnectionHelper.Connect(AutoCreateOption.None);
            Session session = new Session(XpoDefault.DataLayer);
            {
                Queue r = new XPQuery<Queue>(session).SingleOrDefault(q => q.Id.CompareTo(queueItem.IdQueue) == 0);
                r.ObjectOut = queueItem.FichierObjetOut;
                r.Status = (int)queueItem.Etat;
                r.Save();
                session.DataLayer.Dispose();
            }
        }
        /// <summary>
        /// Update the queueItem status.
        /// </summary>
        /// <param name="queueItem"></param>
        public static void UpdateQueueItemStatus(QueueItem queueItem)
        {
            ConnectionHelper.Connect(AutoCreateOption.None);
            Session session = new Session(XpoDefault.DataLayer);
            {
                Queue r = new XPQuery<Queue>(session).SingleOrDefault(q => q.Id.CompareTo(queueItem.IdQueue) == 0);
                r.Status = (int)queueItem.Etat;
                r.Save();
                session.DataLayer.Dispose();
            }
        }
        #endregion
    }
}
