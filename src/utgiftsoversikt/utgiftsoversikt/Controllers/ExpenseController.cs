using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Services;
using utgiftsoversikt.utils;

namespace utgiftsoversikt.Controllers
{
    [ApiController]
    [Route("expense")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IUserService _userService;
        private readonly ILogger<ExpensesController> _logger;

        public ExpensesController(IExpenseService expenseService, IUserService userService, ILogger<ExpensesController> logger)
        {
            _expenseService = expenseService;
            _userService = userService;
            _logger = logger;
        }
        /*
         * Checks if the user is auth, and returnes expense with correct id
         */
        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<Expense> Get([FromBody] string expId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);
            var expense = _expenseService.GetById(expId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            
            if (expense == null)
                return NotFound();
            if (expense.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            return Ok(expense);
        }
        /*
         * Checks if user is auth and retunes all expenses this user ownes
         */
        [HttpPost]
        [Route("getall")]
        [Authorize]
        public ActionResult<List<Expense>> GetAll()
        {
            _logger.LogInformation($"expense endpoint called");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            string month = "092024";
            var expenses = Database.IsLocal ? Database.expenses.FindAll(e => e.UserId == userId) : _expenseService.GetAllByUserIdAndMonth(userId, month);
            if (expenses == null)
                return NotFound();

            return Ok(expenses);
        }
        /*
         * Checks if the user is auth, and creates a new expense owned by this user
         */
        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] RequestExpense exp)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            
            var expense = new Expense()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Date = exp.Date,
                Category = exp.Category == "" || exp.Category == "string" ? "etc" : exp.Category,
                Shop = exp.Shop == "string" ? "" : exp.Shop,
                Sum = exp.Sum,
                Month = "092024",
                Description = exp.Description

            };
            if(Database.IsLocal)
            {
                var month = Database.months.First();
                var newMonth = MonthUtils.AddToMonth(expense, month);

                Database.months.Remove(month);
                Database.months.Add(newMonth);
                Database.expenses.Add(expense);
            }
            else
            {
                _expenseService.Create(expense);
            }
            
            return Ok(expense.Id);
        }
        /*
         * Checks if user is auth, and updates an expense
         */
        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Put([FromBody] RequestExpense exp)
        {
            _logger.LogInformation($"{exp.Sum} {exp.Month} {exp.Category} {exp.Description} {exp.Date}");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = Database.IsLocal ? Database.users.Find(u => u.Id == userId) : _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            var expense = new Expense()
            {
                Id = exp.Id,
                UserId = userId,
                Category = exp.Category,
                Shop = exp.Shop,
                Description = exp.Description,
                Sum = exp.Sum,
                Date = exp.Date,
                Month = "092024"
            };

            if (Database.IsLocal)
            {
                var remExp = Database.expenses.Find(e => e.Id == exp.Id);

                var month = Database.months.First();
                var newMonth = MonthUtils.SubFromMonth(remExp, month);
                var addMonth = MonthUtils.AddToMonth(expense, newMonth);

                Database.months.Remove(month);
                Database.months.Add(addMonth);
                Database.expenses.Remove(remExp);
                Database.expenses.Add(expense);
            }
            else
            {
                _expenseService.Create(expense);
            }
            return Ok();
        }
        /*
         * Checks if user is auth, and deletes the expense with correct id
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
                var remExp = Database.expenses.Find(e => e.Id == id);
                var month = Database.months.First();
                var newMonth = MonthUtils.SubFromMonth(remExp, month);

                Database.months.Remove(month);
                Database.months.Add(newMonth);
                Database.expenses.Remove(remExp);
            }
            else
            {
                var exp = _expenseService.GetById(id);
                _expenseService.Delete(exp);
            }
            return Ok();
        }


    }
}
