using Application.Configurations.Middleware;
using Data.Models.Internal;
using Data.Models.Request.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using Utility.Constants;

namespace Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticatedUser([FromBody][Required] AuthRequest auth)
        {
            var customer = await _authService.AuthenticatedUser(auth);
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("Not Found This User");
            }
        }

        [HttpGet]
        [Authorize(UserRole.Admin, UserRole.Staff, UserRole.Customer)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = (AuthModel)HttpContext.Items["User"]!;
            if (user != null)
            {
                var result = await _userService.GetUser(user.Id);
                if(result is JsonResult jsonResult)
                {
                    return Ok(jsonResult.Value);
                }
            }
            return Unauthorized("Unauthorized");
        }
    }
}
