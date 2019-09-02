using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDGlobalUnknownException : Exception
    {
        public GEDGlobalUnknownException()
        {

        }
        public GEDGlobalUnknownException(string name)
            : base($"{GedConstants.GlobalGEDUnknownExceptionMessage}: {name}")
        {

        }
    }
}