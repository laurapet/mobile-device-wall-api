using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("login")]
        public async Task<IActionResult> ChallengeLogin(string returnUrl = "Account/login-callback")
        {
            var provider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider[0].Name, returnUrl);
            return Challenge(properties);
        }
        
        [HttpGet("login-callback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            await createTestUser();
            
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
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
                await _signInManager.RefreshSignInAsync(user);
            }
            
            return Ok();
        }

        public async Task createTestUser()
        {
            var testUser = new DeviceWallUser()
            {
                Id = 1,
                UserName = "testusername",
                Name = "Testname",
                AvatarUrl = "https://via.placeholder.com/50"
            };
            Console.WriteLine("testuser 1");

            var user = await _userManager.FindByIdAsync("1");
            if (user == null)
            {
                var result = await _userManager.CreateAsync(testUser);
                if (result.Succeeded)
                {
                    Console.WriteLine("testuser created");
                }
            }
        }
        
        [HttpGet("logout")]
        public async void LogOut(string returnUrl = "login")
        {
            await _signInManager.SignOutAsync();
        }
    }
}