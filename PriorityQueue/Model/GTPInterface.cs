//using DevExpress.Xpo.DB;
//using Models.ComptaPlusModel;
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
    /// Interface element
    /// </summary>
    public class GTPInterface
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion

        #region fields
        public Guid Id { get; set; }
        public double ?PriorityValue { get; set; }
        public Guid ?PriorityId { get; set; }
        public bool FakeGuid { get; set; }
        /// <summary>
        /// Fields from WPF
        /// </summary>
        public string Description { get; set; }
        public string TPCode { get; set; }
        public string SIDECode { get; set; }
        public string NSICode { get; set; }
        public string Frequency { get; set; }
        public string Name { get; set; }
        public string ContractName { get; set; }
        public bool IsActif { get; set; }
        #endregion

        public override string ToString() {
            return $"{ContractName} - {Name}, PriorityValue: {PriorityValue}";
        }
        public static List<PriorityQueue.Model.GTPInterface> GetInterfaces() {
            List<GTPInterface> lisiInterfacess = new List<GTPInterface>();
            try { 
                ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = new Session(XpoDefault.DataLayer);
                {
                    var interfaces = new XPQuery<Models.GTPModel.Interface>(session).OrderBy(x => x.Name).ToList();
                    foreach (var interf in interfaces)
                    {
                        try
                        {
                            lisiInterfacess.Add(new GTPInterface()
                            {
                                Name = interf.Name,
                                Id = interf.Id,
                                PriorityId = (interf.Priority != null) ? interf.Priority.Id : new Guid(),
                                PriorityValue = (interf.Priority != null) ? interf.Priority.Code : -1,
                                FakeGuid = (interf.Priority != null) ? false : true,
                                ContractName = interf.ContractName
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(FormatMessages.getLogMessage(
                                "GTPInterface",
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                TokenKey.NoMethodParams,
                                ex.ToString()));
                        }
                    }
                    session.DataLayer.Dispose();
                }
                return lisiInterfacess;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPInterface",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return lisiInterfacess;
            }
            
        }
        public static void UpdateWithPriority(GTPInterface gTPInterface, GTPPriority gTPPriority)
        {
            ConnectionHelper.Connect(AutoCreateOption.None);
            Session session = new Session(XpoDefault.DataLayer);
            {
                Priority p = new XPQuery<Priority>(session).SingleOrDefault(q => q.Id.CompareTo(gTPPriority.Id) == 0);
                Interface r = new XPQuery<Interface>(session).SingleOrDefault(q => q.Id.CompareTo(gTPInterface.Id) == 0);
                r.Priority = p;
                r.Save();
                session.DataLayer.Dispose();
            }
        }
        public static string InsertInterface(GTPInterface gTPInterface)
        {
            try { 
                ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = new Session(XpoDefault.DataLayer);
                {
                    var interfaceInstance = new Models.GTPModel.Interface(session);
                    interfaceInstance.Id = Guid.NewGuid();
                    //Values from WPF
                    interfaceInstance.Description = gTPInterface.Description;
                    interfaceInstance.TPCode = gTPInterface.TPCode;
                    interfaceInstance.SIDECode = gTPInterface.SIDECode;
                    interfaceInstance.NSICode = gTPInterface.NSICode;
                    interfaceInstance.Frequency = gTPInterface.Frequency;
                    interfaceInstance.Name = gTPInterface.Name;
                    interfaceInstance.ContractName = gTPInterface.ContractName;
                    interfaceInstance.IsActive = gTPInterface.IsActif;
                    interfaceInstance.DtCrea = DateTime.Now;
                    interfaceInstance.Save();
                }
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPInterface",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return ex.ToString();
            }
            return string.Empty;
        }
    }
}
