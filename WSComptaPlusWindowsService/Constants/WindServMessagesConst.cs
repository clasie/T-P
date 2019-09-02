using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSComptaPlusWindowsService.Constants
{
    public static class WindServMessagesConst
    {
        public const string RapidMailSubject = "RapidMailSubject";
        public const string SubjectError = "Error WSComptaPlusWindowsService";
        public const string Subject = "WSComptaPlusWindowsService";
        public const string OnPingUrl = "On Ping url: ";
        public const string LogIdentif = "loginResponse.IsAuthenticated";
        public const string LogRespCode = "loginResponse.Code:";
        public const string LogRespMessage = "loginResponse.Message: ";
        public const string LogRespMessageMsg = "loginResponse.ResponseMsg: ";
        public const string FalseLoginCode = "9777";
        public const string Err = "error: ";
        public const string ExceptionTrace1 = "Exception: 2.1 ";
        public const string ExceptionTrace2 = "Exception: 2.2 ";
        public const string OnContinue = "In OnContinue.";
        public const string TraceTimer1 = "Inside OnTimer()";
        public const string TraceTimer2 = "OnTimer, urlsToPing.Count: ";
        //Web server do not respond
        public const string ServerDoNotRespondMessage = "Aucune réponse : ";
        public const string ServerDoNotRespondCode = "10002";
        //Attribute missing
        public const string AttributeMissingMessage = "Attribut RequestContractType manquant pour le type ";
        public const string AttributeMissingCode = "10001";
        //Web server do not respond
        public const string ServerErreurResponseCode = "10003";
        //WebException
        public const string WebExceptionCode = "10004";
        //UriFormatException
        public const string UriFormatExceptionCode = "10005";
        //GlobalException
        public const string GlobalExceptionCode = "10006";
        //httpWebRequest
        public const string HttpWebRequestContentType = "application/json; charset=utf-8; ";
        public const string HttpWebRequestMethod = "POST";
        public const string HttpWebRequestSeparator= "\\";

    }
}
