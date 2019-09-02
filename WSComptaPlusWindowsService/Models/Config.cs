using System;
using System.Collections.Generic;
using System.Configuration;
using TokenHandler.Constants;
using TokenHandler.Utils;
using WSComptaPlusWindowsService.Constants;

namespace WSComptaPlusWindowsService.Models
{
    class Config
    {
        public List<string> WebServicesToPing = new List<string>();
        public string UserAllowedToUseServiceLogin = string.Empty;
        public string UserAllowedToUseServicePassword = string.Empty;        
        public static Config Instance { get { return lazy.Value; } }
        public string PingLoopDelayInMilliSeconds { get; set; }
        //Mails
        public string MailSmtpServer { get; set; }
        public string MailNoReplyAddress { get; set; }
        public string MailAlertDestinataire { get; set; }
        //Singleton
        private static readonly Lazy<Config> lazy = new Lazy<Config>(() => new Config());

        private Config()
        {
            LoadConfig();
        }
        /// <summary>
        /// Load data from config file
        /// </summary>
        private void LoadConfig() { 
            var appSettings = ConfigurationManager.AppSettings;
            //Ping times
            PingLoopDelayInMilliSeconds = appSettings[WindServConfigConst.PingLoopDelayInMilliSeconds];
            //Mail
            MailSmtpServer = appSettings[WindServConfigConst.MailSmtpServer];
            MailAlertDestinataire = appSettings[WindServConfigConst.MailAlertDestinataire];
            MailNoReplyAddress = appSettings[WindServConfigConst.MailNoReplyAddress];
            //user allowed to ping
            UserAllowedToUseServiceLogin = appSettings[WindServConfigConst.UserAllowedToUseServiceLogin];

            //Password management
            var decryptedPw = Md5Encryption.DecryptString(appSettings[WindServConfigConst.UserAllowedToUseServicePassword], Security.ToEncryptEncryptPw);
            UserAllowedToUseServicePassword = decryptedPw; // appSettings[WindServConfigConst.UserAllowedToUseServicePassword];

            //urls to ping
            var listeToCatch = appSettings[WindServConfigConst.WebServiceToPingAsConcatStringPipeSeparated];
            var listIds = listeToCatch.Split(WindServConfigConst.PipeSep);
            foreach (var key in listIds)
            {
                //get those url to ping
                WebServicesToPing.Add(appSettings[FormatParam(key, WindServConfigConst.WsToPing)]);
            }
        }
        private string FormatParam(string key,string modele) {
            return $"{WindServConfigConst.WsToPing}{key}";
        }
    }
}
