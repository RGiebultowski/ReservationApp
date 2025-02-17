using Microsoft.AspNetCore.Mvc;
using ReservationApp.Data;
using ReservationApp.Models;

namespace ReservationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthorizationService authService;

        public AuthController(AuthorizationService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser newUser)
        {
            var user = await authService.CreateNewUser(newUser.Name, newUser.Password, newUser.IsAdmin);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            var token = await authService.Authenticate(user.Name, user.Password);
            if (token == null) return Unauthorized("Nieprawidłowe dane logowania");

            return Ok(new { Token = token });
        }
    }
}
