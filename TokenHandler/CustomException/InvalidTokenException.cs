using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Constants;

namespace TokenHandler.CustomException
{
    /// <summary>
    /// Token custom exception.
    /// </summary>
    [Serializable]
    public class InvalidTokenException : Exception 
    {
        /// <summary>
        /// Launched when the Token inspector detects an invalid Token.
        /// </summary>
        /// <param name="name"></param>
        public InvalidTokenException(string name)
            : base(String.Format("{0} {1}", TokenKey.CustomExceptionLabel, name))
        {

        }

    }
}
