using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConsumeService
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public AuthFilter(string name = "") { PermissionName = name; }
        public string PermissionName { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var filter = context.Filters.LastOrDefault(o=> o is AuthFilter) as AuthFilter;
            //var p = filter == null ? "___" : filter.PermissionName; //需要的权限
            //Console.WriteLine(p);
            //if (context.HttpContext.User.Identity.Name != "qian") { //一个demo而已，权限可以放在这里做
            //    Console.WriteLine("Only Bill can use this function");
                
            //    context.Result = new UnauthorizedResult();
            //}
        }
    }
}
