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
    public class GTPUser
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion
        public GTPUser()
        {
            Token = TokenKey.NoTokenCreatedLabel;
            Exists = false;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public bool Exists { get; set; }
        public string Token { get; set; }
        public override string ToString() {
            return $"Name: {Name}, PassWord: {PassWord}";
        }
        public static List<PriorityQueue.Model.GTPUser> GetUsers(BusinessConfig businessConfig) {
            List<GTPUser> listUsers = new List<GTPUser>();
            try { 
                //ConnectionHelper.Connect(AutoCreateOption.SchemaOnly);
                Session session = businessConfig.GetSession(ComptaPlusConfig.Constants.GTPKey);
                //Session session = new Session(XpoDefault.DataLayer);
                {
                    var users = new XPQuery<Models.GTPModel.User>(session).ToList();
                    foreach (var user in users)
                    {
                        try
                        {
                            listUsers.Add(new GTPUser()
                            {
                                Name = user.Name,
                                Id = user.Id,
                                PassWord = TokenHandler.Utils.Md5Encryption.DecryptString(user.Password, Security.ToEncryptEncryptPw)
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(FormatMessages.getLogMessage(
                                "GTPUser",
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                TokenKey.NoMethodParams,
                                ex.ToString()));
                        }
                    }
                    //session.DataLayer.Dispose();
                    //businessConfig.DisposeSessions();
                }
                return listUsers;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "GTPUser",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return listUsers;
            }
            
        }
    }
}
