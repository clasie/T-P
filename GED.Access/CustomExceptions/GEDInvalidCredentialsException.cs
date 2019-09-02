using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDInvalidCredentialsException : Exception
    {
        public GEDInvalidCredentialsException()
        {

        }
        public GEDInvalidCredentialsException(string name)
            : base($"{GedConstants.InvalidCredentialsExceptionMessage}: {name}")
        {

        }
    }
}