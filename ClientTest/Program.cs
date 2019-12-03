using Grpc.Core;
using Grpc.Net.Client;
using InternalGRPCService;
using RestSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static string _token = "";

        //static string _authUrl = "http://localhost:50000";
        //static string _consUrl = "http://localhost:50001";

        static string _baseUrl = "http://localhost:5000";

        static string _grpcUrl = "https://localhost:5050";


        static void Main(string[] args)
        {
            if (!_grpcUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                    true);
            }

            //1. login as bill
            var ret = Login("bill", "bill");
            
            if (ret) {
                //var client = new RestClient(_consUrl);
                var client = new RestClient(_baseUrl);
                //这里要在获取的令牌字符串前加Bearer
                string tk = "Bearer " + _token;
                client.AddDefaultHeader("Authorization", tk);
                var request = new RestRequest("/test/values", Method.GET);
                
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                Console.WriteLine($"状态码：{(int)response.StatusCode} 状态信息：{response.StatusCode}  返回结果：{content}");


                //-----------------下面是GRPC调用--------------------------------
                var channel = GrpcChannel.ForAddress(_grpcUrl);

                var gclient =  new Greeter.GreeterClient(channel);
                var hiRequest = new HelloRequest { Name = "GreeterClient" };

                var headers = new Metadata();
                headers.Add("Authorization", $"Bearer {_token}");//token加在header里面
                var reply = gclient.SayHelloAsync(hiRequest, headers).GetAwaiter().GetResult();
                Console.WriteLine("调用Greeter服务 : " + reply.Message);
            }

            Console.WriteLine(Environment.NewLine + "Press enter to exit.");
            Console.ReadLine();
        }
        

        static bool Login(string userName, string password)
        {
            //var loginClient = new RestClient(_authUrl);
            //var loginRequest = new RestRequest("/token", Method.POST);
            var loginClient = new RestClient(_baseUrl);
            var loginRequest = new RestRequest("/auth/token", Method.POST);

            loginRequest.AddParameter("username", userName);
            loginRequest.AddParameter("password", password);
            IRestResponse loginResponse = loginClient.Execute(loginRequest);
            var loginContent = loginResponse.Content;
            //Console.WriteLine(loginContent);
            dynamic token = Newtonsoft.Json.JsonConvert.DeserializeObject(loginContent);
            if (token!= null && (bool)token.status) {
                _token = token.token;
                Console.WriteLine($"{userName}登录成功,token: {_token}");
                return true;
            }  else {
                _token = "";
                Console.WriteLine($"{userName}登录失败");
                return false;
            }
        }
    }
}
