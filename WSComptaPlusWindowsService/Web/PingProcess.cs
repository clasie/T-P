using SideWsComptaPlus.Tools;
using System;
using System.IO;
using System.Linq;
using System.Net;
using TokenHandler.Models;
using WSComptaPlusWindowsService.Constants;
using WSComptaPlusWindowsService.Models;
using WSComptaPlusWindowsService.Utils;

namespace WSComptaPlusWindowsService.Web
{
    /// <summary>
    /// This class try to call a webservice with credentials.
    /// If creds are accepted it launches a ping on the queue process hosted by the web service. 
    /// The decision of starting or not the queue process is not here but managed by the queue itself 
    /// on the web service by the class WSComptaPlus.Process.ExcecuteQueuesProcess (QueueLoadingPolicy qp).
    /// </summary>
    class PingProcess
    {
        /// <summary>
        /// This method is called 'Ping()' to keep in mind that his responsibility is just to wakup the web service, 
        /// nothing more than that.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static LoginResponse Ping(string url)
        {
            //Set the request
            LoginRequestQueue loginRequestQueue = new LoginRequestQueue
            {
                Username = Config.Instance.UserAllowedToUseServiceLogin,  
                Password = Config.Instance.UserAllowedToUseServicePassword
            };
            //Get attrubute service
            var attribute = typeof(LoginRequestQueue).GetCustomAttributes(true).OfType<TokenHandler.Attributes.ServiceRequestAttribute>().FirstOrDefault();
            //Call 
            if (attribute == null)
            {
                //--------------------------------------------------------------
                // Erreur dans la réponse
                //--------------------------------------------------------------
                var resp = Activator.CreateInstance<LoginResponse>();
                resp.Code = WindServMessagesConst.AttributeMissingCode;
                resp.Message = $"{WindServMessagesConst.AttributeMissingMessage} {typeof(LoginRequestQueue).Name}";
                resp.ResponseMsg = null;
                return resp;
            }
            try
            {
                #region Timeout management
                //ServicePointManager.ServerCertificateValidationCallback += (se, certificate, chain, sslPolicyErrors) => true;
                //httpWebRequest.Timeout = (httpWebRequest.Timeout * 10);
                //httpWebRequest.Timeout = (120);
                #endregion

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + WindServMessagesConst.HttpWebRequestSeparator + attribute.Url);
                httpWebRequest.ContentType = WindServMessagesConst.HttpWebRequestContentType;
                httpWebRequest.Method = WindServMessagesConst.HttpWebRequestMethod;
                var json = JsonHelp.JsonSerialize(loginRequestQueue);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) { streamWriter.Write(json); }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //Web server do not respond
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    var resp = Activator.CreateInstance<LoginResponse>();
                    resp.Code = WindServMessagesConst.ServerDoNotRespondCode;
                    resp.ResponseMsg = $"{WindServMessagesConst .ServerDoNotRespondMessage} {httpResponse.StatusCode.ToString()}";
                    return resp;
                }
                //Ok wen service answered something
                var result = Activator.CreateInstance<LoginResponse>();
                using (var respStream = httpResponse.GetResponseStream())
                {
                    if (respStream == null) return null;
                    var reader = new StreamReader(respStream);
                    var rep = @reader.ReadToEnd().Trim();
                    try
                    {
                        result = (LoginResponse)JsonHelp.JsonDeserialize<LoginResponse>(rep);//response ok
                    }
                    catch (Exception e)
                    {
                        //----------------------------------
                        // Erreur dans le message de retour
                        //----------------------------------
                        var resp = Activator.CreateInstance<LoginResponse>();
                        resp.Code = WindServMessagesConst.ServerErreurResponseCode;
                        resp.ResponseMsg = e.Message;
                        return resp;
                    }
                }
                return result;
            }
            catch (WebException e)
            {
                var resp = Activator.CreateInstance<LoginResponse>();
                resp.Code = WindServMessagesConst.WebExceptionCode;
                resp.ResponseMsg = e.Message;
                return resp;
            }
            catch (UriFormatException e)
            {
                var resp = Activator.CreateInstance<LoginResponse>();
                resp.Code = WindServMessagesConst.UriFormatExceptionCode;
                resp.ResponseMsg = e.Message;
                return resp;
            }
            catch (Exception e)
            {
                //-------------------------------
                // Erreur générale du Web Service
                //-------------------------------
                var resp = Activator.CreateInstance<LoginResponse>();
                resp.Code = WindServMessagesConst.GlobalExceptionCode;
                resp.ResponseMsg = e.Message;
                return resp;

            }
        }
    }
}
