using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDConnectionUnknownException : Exception
    {
        public GEDConnectionUnknownException()
        {

        }
        public GEDConnectionUnknownException(string name)
            : base($"{GedConstants.ConnectionGEDUnknownExceptionMessage}: {name}")
        {

        }
    }
}