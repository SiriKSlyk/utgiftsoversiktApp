using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;
using utgiftsoversikt.Services;


namespace Utgiftsoversikt.Controllers
{
    [ApiController]
    [Route("budget")]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IExpenseService _expenseService;
        private readonly IMonthService _monthService;
        private readonly IUserService _userService;

        private readonly ILogger<BudgetController> _logger;

        public BudgetController(IBudgetService budgetService, ILogger<BudgetController> logger)
        {
            _budgetService = budgetService;

            _logger = logger;
        }

        //[HttpGet("{budgetId}", Name = "GetBudget")]
        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<Budget> Get([FromBody] string budgetId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            var budget = Database.IsLocal ? Database.budgets.Find(b => b.Id == budgetId) : _budgetService.GetById(budgetId);
            if (budget == null)
                return NotFound();
            if (budget.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            return Ok(budget);
        }


        //[HttpGet("{userId}, {test}", Name = "GetBudgets")]
        [HttpPost]
        [Route("getall")]
        [Authorize]
        public ActionResult<List<Budget>> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            var budgets = Database.IsLocal ? Database.budgets.FindAll(b => b.UserId == userId) : _budgetService.GetAll(userId);
            if (budgets == null)
                return NotFound();
            return Ok(budgets);
        }

        //[HttpPost("{userId}", Name = "PostBudget")]
        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] Budget budget)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            budget.UserId = userId;
            if (Database.IsLocal)
            {
                Database.budgets.Add(budget);
            }
            else
            {
                _budgetService.Create(budget);
            }
            
            return Ok(budget.Id);
        }

        //[HttpPut(Name = "PutBudget")]
        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Put([FromBody] Budget budget)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            budget.UserId = userId;

            if(Database.IsLocal)
            {
                var bud = Database.budgets.Find(b => b.Id == budget.Id);
                Database.budgets.Remove(bud);
                Database.budgets.Add(budget);
            }
            else
            {
                _budgetService.Update(budget);
            }

            
            return Ok();
        }

        //[HttpDelete(Name = "DeleteBudget")]
        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public IActionResult Delete([FromBody] string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            if (Database.IsLocal)
            {
                var bud = Database.budgets.Find(b => b.Id == id);
                Database.budgets.Remove(bud);
                
            }
            else
            {
                var budget = _budgetService.GetById(id);
                _budgetService.Delete(budget);
            }

            return Ok();
        }

    }
}