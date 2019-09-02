using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Attributes;

namespace TokenHandler.Models
{
    [ServiceRequest (Url = "ValidateToken/ws")]
    public class TokenValidationRequest
    {
        //All will be into the header
    }
}
