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
                return new JsonResult(new {
                    Status = false,
                    Message = "Auth Failed"
                });
            } else {
                var dict = new List<Claim>() { 
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Name, username)
                };
                var token = WriteToken(_audienceInfoLoader.LoadAudienceInfo(), dict, DateTime.Now.AddSeconds(3));
                return new JsonResult(new {
                    token = token,
                    name = username,
                    role = role,
                    status = true
                });
            }
        }

        public static string WriteToken(AudienceInfo jwtInfo, IEnumerable<Claim> claimDict, DateTime exp)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtInfo.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
