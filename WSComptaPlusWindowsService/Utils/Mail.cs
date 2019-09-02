using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using TokenHandler.Constants;
using TokenHandler.Utils;

namespace WSComptaPlusWindowsService.Utils
{
    /// <summary>
    /// This class manage mail process
    /// </summary>
    public class WSMail
    {
        #region Methods
        public static void SendMail(
            string subject, 
            string message, 
            string defaultDestinationMailAlert, 
            string smtpServer, 
            string noReplyMail, 
            string destinataireMailAdress=""
            ) {
            try {
                //Default 'to mail' address from config if .
                destinataireMailAdress = 
                    (string.IsNullOrEmpty(destinataireMailAdress)) ?
                        defaultDestinationMailAlert : destinataireMailAdress;

                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.Host = smtpServer; 
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("user", "Password");//not needed
                objeto_mail.From = new MailAddress(noReplyMail);
                objeto_mail.To.Add(new MailAddress(destinataireMailAdress));
                objeto_mail.Subject = subject;
                objeto_mail.Body = message;
                client.Send(objeto_mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
} 