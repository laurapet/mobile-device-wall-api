using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Users.Control;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userToCreate = new DeviceWallUser
            {
                Id = Int32.Parse(userId),
                UserName = userDto.userName,
                Name = userDto.name,
                AvatarUrl = userDto.avatarUrl
            };
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(userToCreate);
                if (result.Succeeded)
                {
                   return Created(new PathString("/"+user.Id),user);
                }
            }
            return Ok();
        }

        //methods to use if GitLab authentication and redirection is handeled in the backhand
        /*
        [HttpGet("external-login")]
        public async Task<IActionResult> ExternalLogin(string returnUrl="login-callback")
        {
            var provider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider[0].Name, returnUrl);
            return Challenge();
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

            var signInResult = await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
            await HttpContext.SignInAsync(loginInfo.Principal);
            if (!signInResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            
            return Ok();
        }*/
        
        [Authorize]
        [HttpGet("current-user")]
        public async Task<DeviceWallUser> GetCurrentUser()
        {
            return await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet("logout")]
        public async void LogOut(string returnUrl = "login")
        {
            await _signInManager.SignOutAsync();
        }
    }
}