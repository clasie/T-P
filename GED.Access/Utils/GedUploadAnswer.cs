namespace GED.Access.Utils
{
    /// <summary>
    /// Answer from GED after a call  
    /// </summary>
    public class GedUploadAnswer 
    {
        public string RawResponseBody { get; set; }
        public string BarCode { get; set; }
        public string UrlDownload { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
    }
}