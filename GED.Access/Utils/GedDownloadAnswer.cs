namespace GED.Access.Utils
{
    /// <summary>
    /// Answer from GED after download call  
    /// </summary>
    public class GedDownloadAnswer
    {
        public string RawResponseBody { get; set; }
        public string BarCode { get; set; }
        public string UrlDownload { get; set; }
        public string MajorVersion { get; set; }
        public string MinorVersion { get; set; }
    }
}