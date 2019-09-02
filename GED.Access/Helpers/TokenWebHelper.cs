using GED.Access.Const;
using System.Net;
using System.Text;

namespace GED.Access.Helpers
{
    public static class TokenWebHelper
    {
        /// <summary>
        /// Ask GED a token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassWord"></param>
        /// <param name="gEDUrl"></param>
        /// <returns>string</returns>
        public static string GetNewToken(string userName, string userPassWord, string gEDUrl) {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(GedConstants.HeaderCacheParam, GedConstants.HeaderCacheValue);
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add(GedConstants.ParamUserLogin, userName);
                reqparm.Add(GedConstants.ParamUserPassword, HashHelper.ComputeSha256Hash(userPassWord));
                byte[] responsebytes = client.UploadValues($"{gEDUrl}{GedConstants.WebSeparator}{GedConstants.ServiceToken}", GedConstants.HeaderMethodPost, reqparm);
                return Encoding.UTF8.GetString(responsebytes);
            }
        }
    }
}