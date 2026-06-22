using ClinicFlow.Dto;
using ClinicFlow.Exceptions;
using ClinicFlow.Services.Interfaces;
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

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = userService.Login(loginRequest);
            return Ok(loginResponse);
        }
    }
}
