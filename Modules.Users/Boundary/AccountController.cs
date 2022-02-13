using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Models;
using device_wall_backend.Modules.Users.Control;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace device_wall_backend.Modules.Users.Boundary
{
    [Route("[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<DeviceWallUser> _userManager;
        private readonly SignInManager<DeviceWallUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(Microsoft.AspNetCore.Identity.UserManager<DeviceWallUser> userManager, SignInManager<DeviceWallUser> signInManager,IHttpContextAccessor httpContextAccessor )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDto)
        {
            if (User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User is authenticated");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userToCreate = new DeviceWallUser
                {
                    Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
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
                        Console.WriteLine("User created");
                    }
                }
                else
                {
                    Console.WriteLine("User exists");
                }
            }
            else
            {
                Console.WriteLine("not authenticated");
            }

            /*var provider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider[0].Name, returnUrl);
            */
            //return Challenge();
            return Ok();
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
            //var signResult = await _signInManager.SignInAsync(user, true);
            await HttpContext.SignInAsync(loginInfo.Principal);
            if (!signInResult.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
            
            return Ok();
        }
        
        [Authorize]
        [HttpGet("current-user")]
        public async Task<DeviceWallUser> GetCurrentUser()
        {
            //return User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //return _httpContextAccessor.HttpContext.User.Identity.GetUserId();

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                //var user = await _userManager.FindByIdAsync(IdentityExtensions.GetUserId(User.Identity));
                /*Console.WriteLine(userid);
                var user = await _userManager.FindByIdAsync(userid);
                Console.WriteLine(user.Name);

                return user;*/
            }
            return null;
        }

        private async Task createTestUser()
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