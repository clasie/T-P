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
    public class GTPPriority
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion

        public Guid Id { get; set; }
        public double Code { get; set; }
        public string PriorityValue { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }

        public override string ToString() {
            return $"Priority: {Code} Label: {Libelle} Description: {Description}";
        }
        public static List<PriorityQueue.Model.GTPPriority> GetPriorities() {
            List<GTPPriority> listPriorities = new List<GTPPriority>();
            try { 
                ConnectionHelper.Connect(AutoCreateOption.None);
                Session session = new Session(XpoDefault.DataLayer);
                {
                    var priorities = new XPQuery<Models.GTPModel.Priority>(session).OrderBy(x => x.Code).ToList();
                    foreach (var prio in priorities)
                    {
                        try
                        {
                            listPriorities.Add(new GTPPriority()
                            {
                                Code = prio.Code,
                                Id = prio.Id,
                                Libelle = prio.Label,
                                Description = prio.Description
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(FormatMessages.getLogMessage(
                                "GTPPriority",
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                TokenKey.NoMethodParams,
                                ex.ToString()));
                        }
                    }
                    session.DataLayer.Dispose();
                }
                return listPriorities;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPPriority",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return listPriorities;
            }
            
        }
    }
}
