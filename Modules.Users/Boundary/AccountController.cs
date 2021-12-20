using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Boundary
{
    [Route("[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<DeviceWallUser> _userManager;
        private readonly SignInManager<DeviceWallUser> _signInManager;

        public AccountController(UserManager<DeviceWallUser> userManager, SignInManager<DeviceWallUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("challenge")]
        public async Task<IActionResult> ChallengeLogin(string returnUrl = "Account/login")
        {
            //Challenge: Requests authentication by the user (e.g. showing a login page)
            //RedirectURI: where user is redirected after authentication
            var provider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider[0].Name, returnUrl);
            return Challenge(properties);
        }

        //TODO: wenn nicht eingeloggt ist id = null
        [HttpGet("login")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "login")
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return await challengeLogin();
            }
            
            var username = loginInfo.Principal.FindFirstValue(ClaimTypes.Name);
            var id = loginInfo.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var avatarURL = loginInfo.Principal.FindFirstValue("urn:gitlab:avatar");
            var name = loginInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
          
            var userToCreate = new DeviceWallUser
            {
                Id = int.Parse(id),
                UserName = username,
                Name = name,
                AvatarUrl = avatarURL
            };
            
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(userToCreate);
                if (result.Succeeded)
                {
                    await _userManager.AddLoginAsync(userToCreate, loginInfo);
                }
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
            if (!signInResult.Succeeded)
            {
                Console.WriteLine("lets refresh ma dudes");
                await _signInManager.RefreshSignInAsync(user);
            }
            
            return Ok();
        }
        
        [HttpGet("logout")]
        public async void LogOut(string returnUrl = "login")
        {
            await _signInManager.SignOutAsync();
        }

        private async Task<IActionResult> challengeLogin()
        {
            var provider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider[0].Name, "Account/login");
            return Challenge(properties);
        }
    }
}