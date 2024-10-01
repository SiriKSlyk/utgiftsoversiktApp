﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using utgiftsoversikt.Data;
using utgiftsoversikt.Models;
using utgiftsoversikt.Services;
using Database = utgiftsoversikt.Data.Database;

namespace utgiftsoversikt.Controllers
{
    [ApiController]
    [Route("month")]
    public class MonthController : ControllerBase
    {
        private readonly IMonthService _monthService;
        private readonly ILogger<MonthController> _logger;
        private readonly IUserService _userService;


        public MonthController(IMonthService monthService, IUserService userService, ILogger<MonthController> logger)
        {
            _monthService = monthService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("get")]
        [Authorize]
        public ActionResult<Month> Get(/*[FromBody] Month month*/)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            /*var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            string mon = month.MonthYear;
            var result = _monthService.GetByUserIdAndMonth(userId, mon);
            if (result == null)
                return NotFound();
            if (month.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }*/
            var result = Database.months.First();
            return Ok(result);
        }

        [HttpPost]
        [Route("getall")]
        [Authorize]
        public ActionResult<List<Month>> GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }

            if (User.Identity.IsAuthenticated)
            {
                var result = _monthService.GetAll(userId);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            return Unauthorized( new { message = "User not authorized" });
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] Month month)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            month.UserId = userId;
            month.Id = Guid.NewGuid().ToString();
            _monthService.Create(month);
            return Ok(month.Id);
        }

        //[HttpPut(Name = "PutMonth")]
        [HttpPut]
        [Route("update")]
        [Authorize]
        public IActionResult Put([FromBody] Month month)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = _userService.GetUserById(userId);

            if (user == null || user.Id != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            month.UserId = userId;
            _monthService.Update(month);
            return Ok(month.Id);
        }

        //[HttpDelete(Name = "DeleteMonth")]
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

            var month = _monthService.GetById(id);
            if (month.UserId != userId)
            {
                _logger.LogInformation($"Could not find user with id {userId}");
                return Unauthorized(new { id = userId, message = $"Could not find user with id {userId}" });
            }
            _monthService.Delete(month);
            return Ok(month.Id);
        }
    }
}
