using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace InternalGRPCService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        //[Authorize]//不能用这个东西
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            //获取用户的方式
            var user = context.GetHttpContext()?.User?.Identity?.Name;

            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name + ", User is :" + (string.IsNullOrEmpty(user) ? "nobody" : user)
            });
        }
    }
}
