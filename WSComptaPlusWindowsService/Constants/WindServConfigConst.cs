using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSComptaPlusWindowsService.Constants
{
    /// <summary>
    /// //Do not change those strings!!!
    /// </summary>
    public static class WindServConfigConst
    {
        public const int DwWaitHint = 100000;
        public const string Source = "WSComptaPlusWindowsServiceSource"; 
        public const string SecureLogonProcessDll = "advapi32.dll";

        /// <summary>
        /// Used to parse data from config:
        /// Add a line with 'WebServiceToPing_1' ou 'WebServiceToPing_10' etc
        /// Add the '1' (ot '10' etc...) in the piped string in 'WebServiceToPing_AsConcatStringPipeSeparated'
        /// If the '1' is not in the piped string it won't be taken into account.
        /// This permits to deactivate urls into the config and reactivate it without removing the url
        /// string itself, just remove the number associated with it.
        /// </summary>
        public const char PipeSep = '|';

        //creds
        public const string UserAllowedToUseServiceLogin = "UserAllowedToUseServiceLogin";
        public const string UserAllowedToUseServicePassword = "UserAllowedToUseServicePassword";
        public const string WsToPing = "WebServiceToPing_";
        public const string WebServiceToPingAsConcatStringPipeSeparated = "WebServiceToPing_AsConcatStringPipeSeparated";
        //Mails
        public const string PingLoopDelayInMilliSeconds = "PingLoopDelayInMilliSeconds";
        public const string MailSmtpServer = "MailSmtpServer";
        public const string MailNoReplyAddress = "MailNoReplyAddress";
        public const string MailAlertDestinataire = "MailAlertDestinataire";
    }
}
