using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using utgiftsoversikt.Models;
using utgiftsoversikt.Services;


namespace Utgiftsoversikt.Controllers
{

    [ApiController]
    [Route("user")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<User> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var user = _userService.GetUserById(userId);

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

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new List<User>());
            }

            var users = _userService.FindAllUsers();
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

            var res = _userService.CreateUser(user);
            return Ok(user);
        }

        [HttpPut]
        [Route("update")]
        [Authorize]
        public ActionResult Put([FromBody] RequestUser reqUser)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new List<User>());
            }
            user.Id = userId;
            user.Email = reqUser.Email != "" || reqUser.Email.ToLower() != "string" ? reqUser.Email : user.Email;
            user.First_name = reqUser.First_name != "" && reqUser.First_name.ToLower() != "string" ? reqUser.First_name : user.First_name;
            user.Last_name = reqUser.Last_name != "" && reqUser.Last_name.ToLower() != "string" ? reqUser.Last_name : user.Last_name;

            return _userService.UpdateUser(user) ? Ok() : BadRequest();

        }
        // Delete a user
        [HttpDelete]//("{id}", Name = "DeleteUser")]
        [Route("delete")]
        [Authorize]
        public ActionResult Delete([FromBody] string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var authUser = _userService.GetUserById(userId);

            if (authUser == null || authUser.Id != userId || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new List<User>());
            }
            _userService.DeleteUser(user);
            return NoContent();
        }

    }
}