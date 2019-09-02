using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDUploadFileException : Exception
    {
        public GEDUploadFileException()
        {

        }
        public GEDUploadFileException(string name)
            : base($"{GedConstants.ConnectionGEDUnknownExceptionMessage}: {name}")
        {

        }
    }
}