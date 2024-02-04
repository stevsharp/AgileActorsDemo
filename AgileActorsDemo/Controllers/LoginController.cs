using AgileActorsDemo.Models;
using AgileActorsDemo.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Net;

namespace AgileActorsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserInput credentials, CancellationToken cancellationToken)
        {
            var isUserValid = await _userService.ValidateUser(credentials.Username, credentials.Password, cancellationToken);

            if (!isUserValid)
                return StatusCode((int)HttpStatusCode.Unauthorized);

            var accessToken = _userService.GenerateToken(credentials.Username, cancellationToken);

            return Ok(accessToken);
        }

    }
}
