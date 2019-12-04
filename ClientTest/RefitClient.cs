using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace ClientTest
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


    class RefitClient
    {
        static string _baseUrl = "http://localhost:5000";

        /// <summary>
        /// 使用refit，测试代码漂亮得像一首情诗
        /// </summary>
        public static async void Test()
        {
            var api = RestService.For<IRefitTest>(_baseUrl);
            var loginResult = await api.Login("bill", "bill");
            if (loginResult.Status) {
                for (var i = 0;i < 3; i++) {
                    string tk = "Bearer " + loginResult.Token;
                    var ret = api.GetValue(tk).Result;
                    Console.WriteLine("Consumer Service return value: " + ret);
                    Thread.SpinWait(10);
                }
            }
        }
    }
}
