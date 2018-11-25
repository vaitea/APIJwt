using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIJwt.Model;
using APIJwt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIJwt.Controllers
{
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("users/authenticate")]
        [HttpPost]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        [Route("users/testAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [AllowAnonymous]
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