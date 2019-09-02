using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Attributes;

namespace TokenHandler.Models
{
    public class TokenValidationResponse
    {
        public string Token { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string ResponseMsg { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsValid { get; set; }

    }
}
