using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsumeService.Controllers
{
    
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [Authorize]
        [AuthFilter("Permission")]
        [HttpGet]
        public string Get()
        {
            var userName = HttpContext.User?.Identity?.Name;
            //return new string[] { "value1", "value2" };
            return string.IsNullOrEmpty(userName) ? "nobody" : userName;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
