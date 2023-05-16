﻿using IAM.API.Services;
using IAM.API.ViewModels.Authentication.Requests;
using IAM.API.ViewModels.Authentication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IAM.API.Controllers
{
    [Route("api/authentication")]
    public class AuthenticationController : Controller
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
    }
}
