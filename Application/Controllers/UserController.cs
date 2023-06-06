using Data.Models.Filters;
using Data.Models.Internal;
using Data.Models.Request.Post;
using Data.Models.Request.Put;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilterModel filter)
        {
            var result =  await _userService.GetUsers(filter);
            if(result is JsonResult jsonResult)
            {
                return Ok(jsonResult.Value);
            }
            return BadRequest("Something wrong!!!");
        }


        [HttpPost]
        [Route("register-customers")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerRequest request)
        {
            var result = await _userService.RegisterCustomer(request);
            if(result is JsonResult jsonResult)
            {
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }
            if(result is StatusCodeResult statusCodeResult)
            {
                if(statusCodeResult.StatusCode == StatusCodes.Status409Conflict)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Email already in use.");
                }
                if(statusCodeResult.StatusCode == StatusCodes.Status400BadRequest)
                {
                    return BadRequest("Something wrong when save to database.");
                }
            }
            return BadRequest("Something wrong!!!");
        }

        [HttpPut]
        [Route("change-avatar")]
        public async Task<IActionResult> ChangeAvatar(IFormFile? avatar)
        {
            if (avatar == null || avatar.Length <= 0)
            {
                return BadRequest("Please choose image");
            }

            var user = (AuthModel)HttpContext.Items["User"]!;
            if (user != null)
            {
                try
                {
                    var result = await _userService.AddAvatar(user.Id, avatar);
                    if(result is JsonResult jsonResult)
                    {
                        return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
                    }
                    if (result is StatusCodeResult statusCodeResult)
                    {
                        var statusCode = statusCodeResult.StatusCode;
                        if (statusCode == StatusCodes.Status400BadRequest)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "Something wrong when save to database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
                }
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id,
                                                        [FromBody] UpdateCustomerRequest request)
        {
            var result = await _userService.UpdateCustomer(id, request);
            if(result is JsonResult jsonResult)
            {
                return StatusCode(StatusCodes.Status201Created, jsonResult.Value);
            }

            if (result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;
                if (statusCode == StatusCodes.Status404NotFound)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Customer not found.");
                }

            }
            return BadRequest("Something wrong!!!");

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            var result = await _userService.DeleteCustomer(id);
            if (result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;
                if (statusCode == StatusCodes.Status404NotFound)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Customer not found.");
                }
                if (statusCode == StatusCodes.Status204NoContent)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                if (statusCode == StatusCodes.Status400BadRequest)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Something wrong when save to database.");
                }
            }
            return BadRequest("Something wrong!!!");
        }

    }
}
