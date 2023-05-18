using IAM.API.Services;
using IAM.API.ViewModels.Authentication.Requests;
using IAM.API.ViewModels.Authentication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.DTOs;

namespace IAM.API.Controllers
{
    [Route("api/iam/authentication")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<SignInResponse>> Authenticate([FromBody] SignInRequest request)
        {
            return await _authenticationService.SignInAsync(request.Username
                    , request.Password);
        }

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup([FromForm] SignUpRequest request)
        {
            var result = await _authenticationService.SignUpAsync(request);
            return Ok();
        }

        [HttpGet("current-user")]
        public async Task<UserInfo?> GetCurrentUser()
        {
            return await _authenticationService.GetCurrentUserInfoAsync();
        }
    }
}
