using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIJwt.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIJwt.Controllers
{
    public class UsersControllerController : Controller
    {

        // GET api/values
        [Route("users/test")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value3" };
        }

        //Get all users
        [Route("users/all")]
        [HttpGet]
        public List<User> test()
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(APIJwt.Model.UserContext)) as UserContext;

            return context.GetAll();
        }
    }
}