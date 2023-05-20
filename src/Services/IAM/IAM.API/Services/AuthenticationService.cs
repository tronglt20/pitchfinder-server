using IAM.API.ViewModels.Authentication.Requests;
using IAM.API.ViewModels.Authentication.Responses;
using IAM.Domain.Entities;
using IAM.Domain.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using PitchFinder.RambitMQ.Events;
using Shared.Infrastructure.Dtos;
using Shared.Infrastructure.DTOs;

namespace IAM.API.Services
{
    public partial class AuthenticationService
    {
        private readonly IdentitySettings IdentitySettings;
        private readonly UserManager<User> _userManager;
        private readonly IUserInfo _userInfo;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRoleRepository _roleRepo;

        public AuthenticationService(IdentitySettings identitySettings
            , UserManager<User> userManager
            , IUserInfo userInfo
            , IPublishEndpoint publishEndpoint
            , IRoleRepository roleRepo)
        {
            IdentitySettings = identitySettings;
            _userManager = userManager;
            _userInfo = userInfo;
            _publishEndpoint = publishEndpoint;
            _roleRepo = roleRepo;
        }

        public async Task<UserInfo?> GetCurrentUserInfoAsync()
        {
            return _userInfo as UserInfo;
        }

        public async Task<SignInResponse> SignInAsync(string userName
            , string password)
        {
            var client = new HttpClient();

            var disco = await GetDiscoveryDocumentAsync(client, IdentitySettings.Authority);

            if (disco.IsError)
                throw new Exception(disco.Exception.Message);

            var tokenResponse = await RequestPasswordTokenAsync(client
                , disco
                , userName
                , password);

            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.ErrorDescription);

            return new SignInResponse(tokenResponse);
        }

        public async Task<User> SignUpAsync(SignUpRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Email);

            if (user == null)
            {
                var role = await _roleRepo.GetByIdAsync(request.RoleId());
                if(role == null)
                    throw new Exception($"Role not found");

                user = new User(request.Email, role);
                var create = await _userManager.CreateAsync(user);
                if (!create.Succeeded)
                    throw new Exception(create.Errors.FirstOrDefault().Description);

                await _userManager.AddPasswordAsync(user, request.Password);
                await _publishEndpoint.Publish(new UserAddedIntergrationEvent(user.Id, user.Email, request.IsCustomer));
            }
            else
                throw new Exception($"Email {user.Email} đã tồn tại.");

            return user;
        }
    }
}
