using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Utilities
{
    public class JwtHelper
    {
        #region Private Vars

        private string _systemKey;

        #endregion Private Vars

        #region Ctor

        public JwtHelper(string systemKey)
        { 
            _systemKey = systemKey;
        }

        #endregion Ctor

        /// <summary>
        /// Create a token using the username
        /// </summary>
        /// <param name="key">Identity Key</param>
        /// <param name="expireMinutes">Token Expireation</param>
        /// <returns><![CDATA[ (string Token, DateTime ExpireDate) ]]></returns>
        public (string Token, DateTime ExpireDate) GenerateToken(string key, int expireMinutes = 30)
        {
            return GenerateToken(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, key)
            }), expireMinutes);
        }

        /// <summary>
        /// Generates a valid JWT string.
        /// </summary>
        /// <param name="identity">data that identifies the caller</param>
        /// <param name="expireMinutes">Token Expireation</param>
        /// <returns><![CDATA[ (string Token, DateTime ExpireDate) ]]></returns>
        public (string Token, DateTime ExpireDate) GenerateToken(ClaimsIdentity identity, int expireMinutes = 30)
        {
            var symmetricKey = Convert.FromBase64String(_systemKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var currentDate = DateTime.UtcNow.AddMinutes(expireMinutes);
            var tokenDesc = new SecurityTokenDescriptor {
                Subject = identity,
                Expires = currentDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var jwToken = tokenHandler.CreateToken(tokenDesc);
            var token = tokenHandler.WriteToken(jwToken);

            return (token, currentDate);
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(_systemKey);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
