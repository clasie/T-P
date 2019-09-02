using GED.Access.Const;
using System;

namespace GED.Access.CustomExceptions
{
    [Serializable]
    public class GEDUserLockedTooMuchFailingAttempsToConnectException : Exception
    {
        public GEDUserLockedTooMuchFailingAttempsToConnectException()
        {

        }
        public GEDUserLockedTooMuchFailingAttempsToConnectException(string name)
            : base($"{GedConstants.UserLockedTooMuchAttemptsExceptionMessage}: {name}")
        {

        }
    }
}