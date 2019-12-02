//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;

//namespace RiiZoo.Web
//{
//    public static class JwtHelper
//    {
//        public static string WriteToken(AudienceInfo jwtInfo, IEnumerable<Claim> claimDict, DateTime exp)
//        {
//            var key = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(jwtInfo.Secret));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: jwtInfo.Issuer,
//                audience: jwtInfo.Audience,
//                claims: claimDict,//claimDict.Select(x => new Claim(x.Key, x.Value)),
//                expires: exp,
//                signingCredentials: creds);
//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

//            return jwt;

//            //下面是jwt的算法，其实同微软的是一样的
//            //var payload = claimDict;
//            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
//            //IJsonSerializer serializer = new JsonNetSerializer();
//            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
//            //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
//            //payload.Add("iss", jwtInfo.Issuer);
//            //payload.Add("aud", jwtInfo.Audience);
//            //payload.Add("exp", UnixEpoch.GetSecondsSince(exp).ToString());

//            //var token = encoder.Encode(payload, jwtInfo.Secret);
//            //return token;
//        }

//        public static (IEnumerable<Claim>, DateTime) ReadToken(string jwt)
//        {
//            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
//            //Dictionary<string, string> dict = new Dictionary<string, string>();
//            //if (token.Claims != null) {
//            //    foreach (var claim in token.Claims) {
//            //        dict.Add(claim.Type, claim.Value);
//            //    }
//            //}
//            return (token.Claims, token.ValidTo);
//        }

//        public static System.DateTime ConvertIntDateTime(double d)
//        {
//            System.DateTime time = System.DateTime.MinValue;
//            System.DateTime startTime = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);//TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
//            time = startTime.AddMilliseconds(d);
//            return time;
//        }
//    }
//}
