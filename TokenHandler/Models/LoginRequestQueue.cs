using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Attributes;

namespace TokenHandler.Models
{
    /// <summary>
    /// Used by the Windows service to launche the Queue.
    /// </summary>
    [ServiceRequest (Url = "wakeUpQueue/ws")] 
    public class LoginRequestQueue
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
