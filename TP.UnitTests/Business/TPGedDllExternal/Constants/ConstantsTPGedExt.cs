namespace TP.UnitTests.Utils
{
    /// <summary>
    /// Réunit les constantes
    /// </summary>
    public static class ConstantsTPGedExt
    {
        //Path
        public static readonly string PathVersExe = @"ExternalGed/bin/Debug/ExternalGed.exe";
        //filetype
        public static readonly string WrongFactureFileType = @"xFAC";
        public static readonly string GoodFactureFileType = @"FAC";
        //GoodBareCode
        public static readonly string WrongBareCode = @"______________";
        public static readonly string GoodBareCode = @"0a9d1f62-2807-42fa-834a-e92b011c4012";
        //Expected return code
        public static readonly int ExpectedReturnNoError = 0;
        public static readonly int ExpectedReturnWrongBareCode = 8;
        public static readonly int ExpectedReturnWrongFileType = 10;

    }
}