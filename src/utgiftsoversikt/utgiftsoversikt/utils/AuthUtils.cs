using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using utgiftsoversikt.Models;
using utgiftsoversikt.Services;


namespace utgiftsoversikt.utils
{
    public class AuthUtils
    {
        private readonly IUserService _userService;

        public AuthUtils(IUserService userService) {
            _userService = userService;
        }

        public static bool isAuth() {
            return false;
        }
    }
}
