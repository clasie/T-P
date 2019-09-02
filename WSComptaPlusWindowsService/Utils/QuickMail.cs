using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSComptaPlusWindowsService.Constants;
using WSComptaPlusWindowsService.Models;

namespace WSComptaPlusWindowsService.Utils
{
    public class QuickMail
    {
        public static void SenRapidMail(string message, string subject= WindServMessagesConst.RapidMailSubject) {
            WSMail.SendMail
                (subject,$"{message}",Config.Instance.MailAlertDestinataire, Config.Instance.MailSmtpServer, Config.Instance.MailNoReplyAddress);
        }
    }
}
