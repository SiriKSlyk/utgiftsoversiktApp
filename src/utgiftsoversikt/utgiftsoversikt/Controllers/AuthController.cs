using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using utgiftsoversikt.Data;
using utgiftsoversikt.Services;
using Database = utgiftsoversikt.Data.Database;

namespace utgiftsoversikt.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // Return user with id
        /*[HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] string email)
        {
            var user = _userService.GetUserByEmail(email);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid username" });

            }

            HttpContext.Session.SetString("userId", user.Id);

            var claims = new List<Claim>
            {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);


            return Ok(new { success = true, userId = user.Id });
        }*/

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] string email)
        {
            var user = Database.GetByEmail(email); //_userService.GetUserByEmail(email);
            _logger.LogInformation($"User: {user == null || user.Email != email} found");

            if (user == null || user.Email.ToLower() != email.ToLower())
            {
                return Unauthorized(new { success = false, message = $"Invalid email: {email}" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id) 
            };

            // Make JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VeryStrongLoooooongAndSecretValue"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7062/",
                audience: "https://localhost:59294/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
             );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, userId = user.Id }); 
        }

        [HttpPost]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var mon = Database.months.First();
            var bud = Database.budgets.First();
            var exp = Database.expenses;
            var usr = Database.users.First();

            return Ok(new { month = mon, budget = bud, expenses = exp, user=usr});
        }


            // Returns all users
            [HttpPost]//(Name = "GetUsers")]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var uId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = HttpContext.Session.GetString("userId");
            _logger.LogInformation($"User: {userId} trying to log out");
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return Ok(new { message = $"Logout successful with uId: {uId}" });

        }
    }
}
