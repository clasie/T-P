namespace TP.UnitTests.Utils
{
    /// <summary>
    /// Réunit les constantes
    /// </summary>
    public static class ConstantsTPGed
    {
        //GED bon credentials
        public static readonly string GedUserLogin = "r.diana@side.eu";
        public static readonly string GedUserPassword = "side";
        public static readonly string GedUserPasswordWEB = "side";
        public static readonly string GedUserPasswordDVP = "@side1";
        //GED mauvais!!! credentials
        public static readonly string GedWrongUserLogin = "wrong.loginMail@side.eu";
        public static readonly string GedWrongUserPassword = "wrongPassword";
        //GED Url
        public static readonly string GedUrlToTestWEB = "http://35.241.194.111/GEDServerV2WS/WebService/document";
        public static readonly string GedUrlToTestDVP = "http://35.241.194.111/GEDServerV2Dvp/WebService/document";
        public static readonly string ServiceDonloadUrlReceivedForAnnexMustContain = "annexe=true";
        //Versions
        public static readonly string GedMinorVersionMinimum = "0";
        public static readonly string GedMajorVersionMinimum = "0";
        //Passed
        public static readonly string ConsolePassed = "Ok";
        public static readonly string ConsoleNotPassed = "KO";
        //Sizes
        public static readonly int MinSize = 0;
        //wrong url to test
        public static readonly string GEDWronWebServiceUrl = @"http://1.1.1.1/GEDServerV2WS/WebService/document";        
        //Resource names
        public static readonly string GEDFacturePdf_ResourceFileName = @"E1903693365_541449020000700550_190605.PDF"; //559 Ko
        public static readonly string GEDFacturePdf2_ResourceFileName = @"N1900420442_541449060017014325_190605.PDF"; //548 Ko
        public static readonly string GEDFacturePdfAnnexe1_ResourceFileName = @"SFM402173INV359097Annexe1.PDF";//57 ko
        public static readonly string GEDFacturePdfAnnexe2_ResourceFileName = @"SFM402173INV359097Annexe2.PDF";// 66 ko
        //Resource names Wrong files to test
        public static readonly string GEDFacturePdfFileDoNotExists_ResourceFileName = "WrongPath.PDF";
        public static readonly string GEDFacturePdfFileExistsWithWrongExtensionTxt_ResourceFileName = @"WrongExtensionTxt.txt";
    }
}