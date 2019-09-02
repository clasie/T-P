using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TokenHandler.Constants;
using TokenHandler.Utils;

namespace TokenHandler
{
    /// <summary>
    /// 
    /// ----------------------------------------------
    ///             JWT Token protection
    /// ----------------------------------------------
    /// 
    /// 
    /// 1- How is it implemented in this WCF Web service?
    /// 
    /// We have an 'Inspector' class method (WSComptaPlus.CustomBehaivious.TokenInspector.BeforeCall())
    /// targeted to take the hand on each try to consume any WCF interface before entering it if the
    /// interface method called is decorated wwhith [TokenInspector].
    /// 
    /// While hanving the hand the inspector method 'BeforeCall()' call 'ManageAuthAndToken.Instance.ValidateToken();'.
    /// 
    /// This 'ValidateToken()' will throw 'TokenHandler.CustomException.InvalidTokenException' if
    /// the Token passed in Header is not recognized as signed by this web service.
    /// 
    /// 
    /// 2- What is JWT ? https://jwt.io/introduction/
    /// 
    /// 
    /// 3- How to install/uninstall jwt ?
    /// 
    /// uninstall-package newtonsoft.json -force
    /// install-package newtonsoft.json
    /// 
    /// </summary>
    public class Token
    {        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        private static readonly log4net.ILog logTokenAccess = log4net.LogManager.GetLogger(TokenKey.TokenAccessNameSpace);
        /// <summary>
        /// Get the complete Authorization string from the header if any.
        /// </summary>
        /// <returns></returns>
        public string GetTokenInHeader()
        {
            string header = string.Empty;
            try
            {
                header = WebOperationContext.Current.IncomingRequest.Headers.GetValues(TokenKey.HeaderTokenToUse)[TokenKey.ExpectedPlaceToFindHeader].ToString();

            }
            catch (Exception ex) {
                header = TokenKey.TokenNotFound;
                log.Error(FormatMessages.getLogMessage(
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    string.Empty,
                    ex.ToString()));
            }
            return header;
        }
        /// <summary>
        /// Extract the Token without the secret header == TokenKey.TokenPrefixRaw
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns></returns>
        public string GetTokenFromAuthHeaderString(string authHeader)
        {
            try { 
                return authHeader.StartsWith(TokenHandler.Constants.TokenKey.PrefixSetByTheCallerBeforeTheKey) ?
                    authHeader.Substring(TokenKey.TokenPrefixRaw.Length) : authHeader;
            }
            catch (Exception ex) {
                log.Error(FormatMessages.getLogMessage(
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    string.Concat("authHeader:", authHeader),
                    ex.ToString()));
            }
            return string.Empty;

}
        /// <summary>
        /// Operates typical JWT checks on valid dates and other values.
        /// The only way to know if the token is valid is the NONE throwing 
        /// exception possibly launched by the JWT library.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public TokenHandler.Models.TokenCheckResult CheckTokenValidity(string tokenToChallange, string privateKey)
        {
            TokenHandler.Models.TokenCheckResult tokenCheckResult = new TokenHandler.Models.TokenCheckResult();
            try
            {
                string sec = privateKey;
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
                Microsoft.IdentityModel.Tokens.SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = TokenKey.Audience,
                    ValidIssuer = TokenKey.Issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };
                handler.ValidateToken(
                    tokenToChallange, 
                    validationParameters, 
                    out securityToken);
                tokenCheckResult.IsValidKey = true;
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException e)
            {
                tokenCheckResult.IsValidKey = false;
                tokenCheckResult.HttpStatusCode = HttpStatusCode.Unauthorized;
                tokenCheckResult.ErrorMessage = string.Format("{0}", e.ToString());

                log.Error(FormatMessages.getLogMessage(
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    string.Concat("tokenToChallange:", tokenToChallange, " privateKey: ", " private"),
                    e.ToString()));

                throw new TokenHandler.CustomException.InvalidTokenException(TokenKey.TokenNotFound);
            }
            catch (Exception ex)
            {
                tokenCheckResult.IsValidKey = false;
                tokenCheckResult.HttpStatusCode = HttpStatusCode.Unauthorized;
                tokenCheckResult.ErrorMessage = string.Format("{0}", ex.ToString());

                log.Error(FormatMessages.getLogMessage(
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                string.Concat("tokenToChallange:", tokenToChallange, " privateKey: ", " private"),
                ex.ToString()));

                throw new TokenHandler.CustomException.InvalidTokenException(TokenKey.TokenNotFound);

            }
            //If we reach this place means the token has been validated by JWT.
            return tokenCheckResult;
        }
        /// <summary>
        /// Verify the dates constrains made when the token was created.
        /// </summary>
        /// <param name="notBefore"></param>
        /// <param name="expires"></param>
        /// <param name="securityToken"></param>
        /// <param name="validationParameters"></param>
        /// <returns></returns>
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
        /// <summary>
        /// The very place where the token is created.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string createToken(string username, int tokenLifeDuration, string privateKey)
        {
            try
            {
                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                //set the time when it expires
                DateTime expires = DateTime.UtcNow.AddMinutes(tokenLifeDuration);

                //To encrypt the private token -> http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();

                //create an identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                });
                string sec = privateKey;
                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
                var signingCredentials
                    = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                        securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);
                //create the jwt
                var token =
                    (JwtSecurityToken)
                        tokenHandler.CreateJwtSecurityToken(
                            issuer: TokenKey.Issuer,
                            audience: TokenKey.Audience,
                            subject: claimsIdentity,
                            notBefore: issuedAt,
                            expires: expires,
                            signingCredentials: signingCredentials);
                var tokenString = tokenHandler.WriteToken(token);
                return tokenString;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                this.GetType().Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                string.Concat("username:", username, " tokenLifeDuration: ", tokenLifeDuration, " privateKey: ", " privateKey "),
                ex.ToString()));
            }
            return string.Empty;
        }
    }
}
