using ClinicFlow.Dto;
using ClinicFlow.Exceptions;
using ClinicFlow.Models;
using ClinicFlow.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            var user = userService.GetUserByEmail(loginRequest.Email);
            if (user == null || user?.Password != loginRequest.Password)
            {
                throw new InvalidCredentialsException("Invalid email or password.");
            }

            var loginResponse = new LoginResponse
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email
            };

            return Ok(loginResponse);
        }
    }
}
