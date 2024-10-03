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
        private readonly IMonthService _monthService;
        private readonly IBudgetService _budgetService;
        private readonly IExpenseService _expenseService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IMonthService monthService, IBudgetService budgetService, IExpenseService expenseService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _monthService = monthService;
            _budgetService = budgetService;
            _expenseService = expenseService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] string email)
        {
            var user = Database.IsLocal ? Database.users.Find(u => u.Email.ToLower() == email.ToLower()) : _userService.GetUserByEmail(email);
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
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
             );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, userId = user.Id }); 
        }

        [HttpPost]
        [Route("getAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var usr = Database.users.Find(u => u.Id == userId);
            var mon = Database.months.First();
            var bud = Database.budgets.First();
            var exp = Database.expenses.FindAll(e => e.UserId == userId);

            if (!Database.IsLocal)
            {
                usr = _userService.GetUserById(userId);
                mon = _monthService.GetByUserIdAndMonth(userId, "092024");
                bud = _budgetService.GetById(mon.BudgetId);
                exp = _expenseService.GetAllByUserIdAndMonth(userId, mon.MonthYear);


            }
            

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
