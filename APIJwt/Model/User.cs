using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIJwt.Model
{
    public class User
    {
        private UserContext context;

        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Error { get; set; }
    }
}
