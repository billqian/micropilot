using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RiiZoo.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    public class TokenResult
    {
        public bool Status { get; set; } = false;
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
        public string Token { get; set; } = "";
    }

    [Route("[controller]")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private IAudienceInfoLoader _audienceInfoLoader;

        public TokenController(IAudienceInfoLoader _audienceLoader)
        {
            _audienceInfoLoader = _audienceLoader;
        }

        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            var isValidated = (username == "bill" || username == "qian");
            var role = username == "bill" ? "admin" : "system";
            if (!isValidated) {
                return new JsonResult(new TokenResult() {
                    Status = false
                });
            } else {
                var dict = new List<Claim>() { 
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, username)
                };
                dict.Add(new Claim(ClaimTypes.Role, "User"));
                dict.Add(new Claim("wx_openid", "12345"));
                dict.Add(new Claim("session_id", Guid.NewGuid().ToString()));
                var token = WriteToken(_audienceInfoLoader.LoadAudienceInfo(), dict, DateTime.Now.AddMinutes(30));
                return new JsonResult(new TokenResult() {
                    Token = token,
                    Name = username,
                    Role = role,
                    Status = true
                });
            }
        }

        public static string WriteToken(AudienceInfo jwtInfo, IEnumerable<Claim> claimDict, DateTime exp)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtInfo.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: jwtInfo.Issuer,
                audience: jwtInfo.Audience,
                claims: claimDict,//claimDict.Select(x => new Claim(x.Key, x.Value)),
                expires: exp,
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
