using System;

namespace GED.Access.GEDJsonClasses
{
    public class GEDJsonClassRequestResponse
    {
        public string url { get; set; }
        public string barCode { get; set; }
        public string docType { get; set; }
        public string provider { get; set; }
        public string token { get; set; }
        public string expiration { get; set; }
        public string error { get; set; }
        public string name { get; set; }
        public SByte[] file { get; set; }
        public bool StatusFileCopiedLocally { get; set; }
        public string PathFileCopiedLocally { get; set; }
        public string majorVersion { get; set; }
        public string minorVersion { get; set; }
        public int sizeExpected { get; set; }
        public bool doubleCheckLocalSizeStatus { get; set; }
        public string doubleCheckLocalSizeStatusError { get; set; }
    }
}