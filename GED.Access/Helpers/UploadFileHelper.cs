using GED.Access.Const;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GED.Access.Helpers
{
    public class UploadFileHelper
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, string headerkey, string headervalue)
        {
            string formDataBoundary = String.Format(GedConstants.StringFormatBoundary, Guid.NewGuid());
            string contentType = $"{GedConstants.StringMultiPart}{formDataBoundary}";
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return PostForm(postUrl, userAgent, contentType, formData, headerkey, headervalue);
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            if (request == null)
            {
                throw new NullReferenceException(GedConstants.NotHttpRequest);
            }
            // Set up the request properties.  
            request.Method = GedConstants.HeaderMethodPost;
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            #region add auth if needed code in comment
            // You could add authentication here as well if needed:  
            // request.PreAuthenticate = true;  
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;  
            #endregion
            //Add header if needed  
            request.Headers.Add(headerkey, headervalue);
            // Send the form data to the request.  
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes(GedConstants.SeparatorClrf), 0, encoding.GetByteCount(GedConstants.SeparatorClrf));

                needsCLRF = true;

                if (param.Value is FileParameter) // to check if parameter is of file type   
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;
                    // Add just the first part of this param, since we will write the file data directly to the Stream  
                    string header = string.Format(GedConstants.StringFormatHeader,
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? GedConstants.ApplicationOctetHeader);
                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    // Write the file data directly to the Stream, rather than serializing it to a string.  
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format(GedConstants.StringFormatPostData,
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }
            // Add the end of the request.  Start with a newline  
            string footer = GedConstants.SeparatoFooterStart + boundary + GedConstants.SeparatoFooterEnd;
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
            // Dump the Stream into a byte[]  
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
}