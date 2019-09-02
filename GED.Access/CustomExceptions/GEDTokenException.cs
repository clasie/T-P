using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDTokenException : Exception
    {
        public GEDTokenException()
        {

        }
        public GEDTokenException(string name)
            : base($"{GedConstants.GEDTokenExceptionMessage}: {name}")
        {

        }
    }
}