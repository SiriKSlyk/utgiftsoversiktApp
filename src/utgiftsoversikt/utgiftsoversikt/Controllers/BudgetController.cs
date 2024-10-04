using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;
using utgiftsoversikt.Services;
using utgiftsoversikt.utils;


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

        /*
         * Autorizes the user and uses the budgetId given in payload to return the correct budget if user is auth
         * 
         */
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
        /*
         * Checks if the user is authorized
         * Returnes a list of all the budgets the user ownes
         */
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

        /*
         * Checks if the user is authorized
         * Creates a new budget that this user ownes
         */
        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] RequestBudget budget)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }


            var newBudget = Database.budgets.First();

            newBudget.UserId = userId;
            newBudget.House = decimal.Parse(budget.House);
            newBudget.Food = decimal.Parse(budget.Food);
            newBudget.Transport = decimal.Parse(budget.Transport);
            newBudget.Debt = decimal.Parse(budget.Debt);
            newBudget.Saving = decimal.Parse(budget.Saving);
            newBudget.Etc = decimal.Parse(budget.Etc);
            newBudget.Sum = 0;
            newBudget.Sum = BudgetUtils.CalculateSum(newBudget);

            if (Database.IsLocal)
            {
                Database.budgets.Add(newBudget);
            }
            else
            {
                _budgetService.Create(newBudget);
            }
            
            return Ok(newBudget.Id);
        }

        /*
         * Checks if the user is authorized
         * Updates the  budget if the user is the owner
         */
        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Put([FromBody] RequestBudget budget)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var newBudget = Database.budgets.First();

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogError($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            newBudget.UserId = userId;
            newBudget.House = decimal.Parse(""+budget.House) ;
            newBudget.Food = decimal.Parse(""+budget.Food);
            newBudget.Transport = decimal.Parse(""+budget.Transport);
            newBudget.Debt = decimal.Parse(""+budget.Debt);
            newBudget.Saving = decimal.Parse(""+budget.Saving);
            newBudget.Etc = decimal.Parse(""+budget.Etc);
            newBudget.Sum = 0;
            newBudget.Sum = BudgetUtils.CalculateSum(newBudget);

            _logger.LogInformation("Budget is edited successfully!");

            if (Database.IsLocal)
            {
                var bud = Database.budgets.First();
                Database.budgets.Remove(bud);
                Database.budgets.Add(newBudget);
            }
            else
            {
                _budgetService.Update(newBudget);
            }

            
            return Ok();
        }
        /*
         * Checks if the user is authorized
         * Deletes the budget with this id if the user is the owner
         */
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
                var bud = Database.budgets.First();
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