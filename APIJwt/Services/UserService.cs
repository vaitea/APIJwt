using APIJwt.Helpers;
using APIJwt.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIJwt.Services
{

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        public IConfiguration _ConnectionString;

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_ConnectionString.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>{
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;
            _ConnectionString = configuration;
        }

        public User Authenticate(string username, string password)
        {
            User user = new User();

            //Get user for login
            try
            {
                MySqlConnection conn = GetConnection();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from users where Username=@usr and Password=@psw", conn);
                cmd.Parameters.AddWithValue("@usr", username);
                cmd.Parameters.AddWithValue("@psw", password);

                using (var reader = cmd.ExecuteReader())
                {
                    int countRows = 0;
                    while (reader.Read())
                    {
                        //Need to find a way to fill
                        //To do

                        user.Id = Convert.ToInt16(reader["Id"]);
                        user.Username = reader["Username"].ToString();
                        user.Password = reader["Password"].ToString();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.Email = reader["Email"].ToString();
                        countRows++;
                    }

                    if (countRows == 0)
                    {
                        user = null;
                    }
                }
                conn.Close();
            }
            catch(Exception ex)
            {
                user = null;
            }

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            //Save token in DB
            //To do

            // remove password before returning
            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _users.Select(x => {
                x.Password = null;
                return x;
            });
        }
    }
}
