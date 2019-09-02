using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TokenHandler.Models
{
    public class LoginResponse
    {
        /// <summary>
        /// Login response class.
        /// </summary>
        public LoginResponse()
        {
            this.Token = ""; 
            IsAuthenticated = false;
        }

        public string Token { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string ResponseMsg { get; set; }
        public bool IsAuthenticated { get; set; }
        public override string ToString()
        {
            return $" Token: {Token}, Code: {Code}, Message: {Message}, ResonseMsg: {ResponseMsg}, IsAuthenticated: {((IsAuthenticated)?"True":"False")}";
        }
    }
}
