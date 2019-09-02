using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDUploadFileUnknownException : Exception
    {
        public GEDUploadFileUnknownException()
        {

        }
        public GEDUploadFileUnknownException(string name)
            : base($"{GedConstants.GEDUploadFileUnknownExceptionMessage}: {name}")
        {

        }
    }
}