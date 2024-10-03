using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using utgiftsoversikt.Models;
using utgiftsoversikt.Services;
using utgiftsoversikt.Data;
using utgiftsoversikt.Controllers;


namespace Utgiftsoversikt.Controllers
{

    [ApiController]
    [Route("user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMonthService _monthService;
        private readonly IBudgetService _budgetService;
        private readonly IExpenseService _expenseService;

        private readonly CosmosContext _context;

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IMonthService monthService, IBudgetService budgetService, IExpenseService expenseService, ILogger<UsersController> logger, CosmosContext context)
        {
            _userService = userService;
            _monthService = monthService;
            _budgetService = budgetService;
            _expenseService = expenseService;
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<User> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }


            _logger.LogInformation($"User {userId} existing in database and is: {user}");
            return Ok(new { usr = user, id = userId });
        }


        [HttpPost]
        [Route("getall")]
        [Authorize]
        public ActionResult<List<User>> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new List<User>());
            }

            var users = Database.IsLocal ? Database.users : _userService.FindAllUsers();
            return Ok(users);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Post([FromBody] RequestUser reqUser)
        {
            var user =  new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = reqUser.Email,
                First_name = reqUser.First_name,
                Last_name = reqUser.Last_name,
                Is_admin = false,
                BudgetId = reqUser.BudgetId ?? ""
            };
            if(Database.IsLocal)
            {
                Database.users.Add(user);
            }
            else
            {
                _userService.CreateUser(user);
            }
            
            return Ok(user);
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public ActionResult Put([FromBody] RequestUser reqUser)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new List<User>());
            }
            user.Id = userId;
            user.Email = reqUser.Email != "" || reqUser.Email.ToLower() != "string" ? reqUser.Email : user.Email;
            user.First_name = reqUser.First_name != "" && reqUser.First_name.ToLower() != "string" ? reqUser.First_name : user.First_name;
            user.Last_name = reqUser.Last_name != "" && reqUser.Last_name.ToLower() != "string" ? reqUser.Last_name : user.Last_name;

            if(Database.IsLocal)
            {
                Database.users.Find(u => user.Id == u.Id);
                return Ok();
            }


            return _userService.UpdateUser(user) ? Ok() : BadRequest();

        }
        // Delete a user
        [HttpDelete]//("{id}", Name = "DeleteUser")]
        [Route("delete")]
        [Authorize]
        public ActionResult Delete([FromBody] string id)
        {
            
            var user = Database.IsLocal ? Database.users.Find(u => u.Id == id) : _userService.GetUserById(id);

            if(Database.IsLocal)
            {
                Database.users.Remove(user);
            }
            else
            {
                _userService.DeleteUser(user);
            }

            
            return NoContent();
        }

        [HttpPost]//(Name = "GetUsers")]
        [Route("add")]
        public async Task<IActionResult> Add()
        {
            if (Database.IsLocal)
            {
                return BadRequest();
            }
            var users = Database.users;
            var expenses = Database.expenses;
            var budgets = Database.budgets;
            var months = Database.months;

            _context.Users?.AddRange(users);
            _context.Expenses?.AddRange(expenses);
            _context.Month?.AddRange(months);
            _context.Budget?.AddRange(budgets);

            await _context.SaveChangesAsync();

            
            
            return Ok();

        }

    }
}