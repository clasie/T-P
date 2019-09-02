using GED.Access.Enums;
using System;
using System.IO;
using System.Net;

namespace GED.Access.Helpers
{
    public static class DownloadWebHelper
    {
        /// <summary>
        /// Ask GED a token
        /// Create requests sync asynch ...
        /// https://stackoverflow.com/questions/27108264/c-sharp-how-to-properly-make-a-http-web-get-request
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassWord"></param>
        /// <param name="gEDUrl"></param>
        /// <returns>string</returns>
        public static string DownLoadFile(
            string urlDownLoad,
            string token, 
            string barcode, 
            string gEDUrl, 
            GedService.DocTypesEnum enumDocType, 
            string majorVersion="", 
            string minorVersion="")
        {
            using (WebClient client = new WebClient())
            {
                var url = urlDownLoad; // "http://127.0.0.1:8080/GEDServerV2Dvp/WebService/document/download/8000a084f9474ebdb1fb623df68b3ae1?majorVersion=14&minorVersion=0";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                String test = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    test = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
                //DeserializeObject(test...)
                //return string;
                /*
                client.Headers.Add(GedConstants.HeaderCacheParam, GedConstants.HeaderCacheValue);
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add(GedConstants.ParameterProvider, string.Empty);
                reqparm.Add(GedConstants.ParameterToken, token);
                reqparm.Add(GedConstants.ParameterMajorVersion, majorVersion);
                reqparm.Add(GedConstants.ParameterMinorVersion, minorVersion);
                reqparm.Add(GedConstants.ParameterBarCode, barcode);
                reqparm.Add(GedConstants.ParameterDocType, enumDocType.ToString());
                string url = $"{gEDUrl}{GedConstants.WebSeparator}{GedConstants.ServiceDownload}";
                url = "http://127.0.0.1:8080/GEDServerV2Dvp/WebService/document/download/8000a084f9474ebdb1fb623df68b3ae1?majorVersion=14&minorVersion=0";
                byte[] responsebytes = 
                    client.DownloadFile(url);
                string response = Encoding.UTF8.GetString(responsebytes);
                return response;
                */
                return "";
            }
        }
    }
}
