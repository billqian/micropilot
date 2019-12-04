using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;

namespace MVCClient
{
    public class TokenResult
    {
        public bool Status { get; set; } = false;
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
        public string Token { get; set; } = "";
    }

    public interface IRefitTest
    {
        [Post("/auth/token")]
        Task<TokenResult> Login(string username, string password);

        [Get("/test/values")]
        Task<string> GetValue([Header("Authorization")] string authorization);
    }
}
