using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GED.Access.Const
{
    public static class GedConstants
    {
        //Versions
        public const int MinVersionMinimal = -1;
        public const int MaxVersionMinimal = -1;
        //String use
        public const string StringEmpty = "";
        //Creds user
        public static readonly string ParamUserLogin = "login";
        public static readonly string ParamUserPassword = "password";
        //GED services
        public static readonly string ServiceToken = "token";
        public static readonly string ServiceUpload = "upload";
        public static readonly string ServiceDownload = "download";
        public static readonly string ServiceDownloadJson = "download?type=json";
        public static readonly string ServiceDownloadByte = "download?type=byte";        
        //Header GED
        public static readonly string HeaderCacheParam= "Cache-Control";
        public static readonly string HeaderCacheValue = "no-cache";
        public static readonly string HeaderMethodPost = "POST";
        public static readonly string HeaderMethodGet = "GET";
        public static readonly string WebUserAgent = "";
        public static readonly string HttpParamContentTypeAppliPdf = "application/pdf";
        //Parameters
        public static readonly string ParameterFile = "file";
        public static readonly string ParameterToken = "token";
        public static readonly string ParameterDocType = "docType";
        public static readonly string ParameterBarCode = "barCode";
        public static readonly string ParameterProvider = "provider";
        public static readonly string ParameterMajorVersion = "majorVersion";
        public static readonly string ParameterMinorVersion = "minorVersion";
        public static readonly string ParameterAnnexe = "annexe";
        public static readonly string ParameterRequestType = "type";
        public static readonly string ParameterRequestTypeJson = "json";
        public static readonly string ParameterRequestTypeByte = "byte";
        //Web 
        public static readonly string WebSeparator = "/";
        //UploadFileHelper
        public static readonly string StringFormatBoundary = "----{0:N}";
        public static readonly string StringMultiPart = "multipart/form-data; boundary=";
        public static readonly string StringAppliOcted = "application/octet-stream application/xml; boundary=";
        public static readonly string SeparatorClrf = "\r\n";
        public static readonly string SeparatoFooterStart = "\r\n--";
        public static readonly string SeparatoFooterEnd = "--\r\n";
        public static readonly string StringFormatHeader = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
        public static readonly string ApplicationOctetHeader = "application/octet-stream";
        public static readonly string StringFormatPostData = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
        //Key loop constraints
        public static readonly int LoopGetTokenMinConterValue = 0;
        public static readonly int LoopGetTokenMaxConterValue = 2;
        //Sizes
        public const int MinSizeExpected = 0;
        //Accepted extension
        public static readonly string ExtensionAcceptee = ".pdf";
        
        /// <summary>
        /// Errors
        /// </summary>

        //GED possible error messages connection call
        public static readonly string InvalidCredentialsExceptionMessage = "Invalid credentials to connect to the GED web service. See the GED administrator.";
        public static readonly string UserLockedTooMuchAttemptsExceptionMessage = "User locked, too much failling attemps. See the GED administrator.";
        public static readonly string ConnectionGEDUnknownExceptionMessage = "Connection to GED generated unexpected message.";
        public static readonly string GlobalGEDUnknownExceptionMessage = "GED Dll generated unexpected error.";
        public static readonly string GEDUploadFileUnknownExceptionMessage = "UploadFiles file to GED generated unexpected message.";
        public static readonly string GEDTokenExceptionMessage = "Token expired or missing error.";
        public static readonly string GEDUploadFileExceptionMessage = "UploadFiles file error.";
        //GED possible error messages get token call   
        public static readonly string TooManyAttempts = "webservice.error.credentials.blocked";
        public static readonly string WrongCreds = "webservice.error.credentials.wrong";
        public static readonly string MissMissingOrInvalidToken = "webservice.error.token.invalid";
        //GED possible error messages add file call
        //   - Doc
        public static readonly string UnknownDocType = "webservice.error.docType.unknown";
        public static readonly string MissingFile = "webservice.error.file.missing";
        public static readonly string FileNotFound = "webservice.error.file.not_found";
        public static readonly string MissingDocType = "webservice.error.docType.missing";
        public static readonly string MissingBarCode = "webservice.error.barCode.missing";
        public static readonly string MissingProvider = "Missing Provider";
        public static readonly string WrongReturnType = "webservice.error.type.invalid";
        //   - Token
        public static readonly string MissingOrInvalidToken = "webservice.error.token.invalid";
        //   - Version
        public static readonly string MissingMajorVersion = "webservice.error.majorVersion.missing";
        public static readonly string MissingMinorVersion = "webservice.error.minorVersion.missing";
        //Dll error message
        public static readonly string NotHttpRequest = "Request is not a http request";
        public static readonly string UnknownDEGDllError = "Unknown GED Dll error";

        /*  
        Missing or invalid token -> webservice.error.token.invalid -
        Wrong credentials -> webservice.error.credentials.wrong -
        Too many attempts -> webservice.error.credentials.blocked -
        missing barCode -> webservice.error.barCode.missing -
        missing docType -> webservice.error.docType.missing -
        Unknown Doc Type -> webservice.error.docType.unknown -
        missing majorVersion -> webservice.error.majorVersion.missing -
        missing minorVersion -> webservice.error.minorVersion.missing -
        missing file -> webservice.error.file.missing -
        file not found -> webservice.error.file.not_found -
        not byte or not json -> webservice.error.type.invalid
        */
    }
}