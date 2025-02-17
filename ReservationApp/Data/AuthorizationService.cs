using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReservationApp.Models;

namespace ReservationApp.Data
{
    public class AuthorizationService
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public AuthorizationService(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        // user login
        public async Task<string> Authenticate(string username, string password)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Name == username);

            if (user == null || user.PasswordHash != ApplicationDbContext.HashPassword(password))
                return null;

            return GenerateJWTToken(user);
        }

        //create new user
        public async Task<User> CreateNewUser(string name, string password, bool isAdmin)
        {
            var hashedPassword = ApplicationDbContext.HashPassword(password);

            var user = new User
            {
                Name = name,
                PasswordHash = hashedPassword,
                IsAdministrator = isAdmin
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        //token generation
        private string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.IsAdministrator ? "Administrator" : "User")
            }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
