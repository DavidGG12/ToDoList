using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Models.Request;

namespace ToDoList.Helpers
{
    public class JWT
    {
        private IConfiguration _config;
        private string keyToken;

        public JWT(string key)
        {
            this.keyToken = key;
        }

        public string generateToken(rq_PostLogin_Model infoLogin)
        {
            var mngToken = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(this.keyToken);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", infoLogin.email),
                    new Claim("Password", infoLogin.password),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = mngToken.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
