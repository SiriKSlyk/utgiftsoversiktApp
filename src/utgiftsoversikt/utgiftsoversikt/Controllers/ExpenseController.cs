using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Repos;
using utgiftsoversikt.Services;

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

        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<Expense> Get([FromBody] string expId) // change name
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            var expense = _expenseService.GetById(expId);
            if (expense == null)
                return NotFound();
            if (expense.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            return Ok(expense);
        }

        [HttpPost]
        [Route("getall")]
        [Authorize]
        public ActionResult<List<Expense>> GetAll(/*[FromBody] RequestExpense exp*/)
        {
            _logger.LogInformation($"expense endpoint called");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            /*var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }*/
            /*string month = exp.Month;
            var expenses = _expenseService.GetAllByUserIdAndMonth(userId, month);*/
            var expenses = Database.expenses;
            if (expenses == null)
                return NotFound();

            return Ok(expenses);
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] RequestExpense exp)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            var newDate = DateTime.ParseExact(exp.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var expense = new Expense()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Date = newDate,
                Category = exp.Category == "" || exp.Category == "string" ? "etc" : exp.Category,
                Shop = exp.Shop == "string" ? "" : exp.Shop,
                Sum = exp.Sum,
                Month = newDate.ToString("MMyyyy"),
                Description = exp.Description

            };
            
            _expenseService.Create(expense);
            return Ok(expense.Id);
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Put([FromBody] RequestExpense exp)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            var newDate = DateTime.ParseExact(exp.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var expense = new Expense()
            {
                Id = exp.Id,
                UserId = userId,
                Category = exp.Category,
                Shop = exp.Shop,
                Description = exp.Description,
                Sum = exp.Sum,
                Date = newDate,
                Month = newDate.ToString("MMyyyy")

            };
            //Must include modification of sum when changing an expense
            var res = _expenseService.Update(expense);
            return res ? Ok() : BadRequest();
        }


        [HttpDelete]
        [Route("delete")]
        [Authorize]
        public IActionResult Delete([FromBody] string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            var exp = _expenseService.GetById(id);
            if (exp.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            _expenseService.Delete(exp);
            return Ok();
        }


    }
}
