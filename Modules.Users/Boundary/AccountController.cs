using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using device_wall_backend.Models;
using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "login")
        {
            var claims = User.Claims;
            var name = User.FindFirstValue(ClaimTypes.Name);
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(id);
            //If the user doesn't exist in the db yet
            if (user == null)
            {
                var userToCreate = new DeviceWallUser
                {
                    Id = int.Parse(id),
                    UserName = name
                };
            
                var result = await _userManager.CreateAsync(userToCreate);
                if (result.Succeeded)
                {
                    //isPersistent: Flag indicating whether the sign-in cookie should persist after the browser is closed.
                    await _signInManager.SignInAsync(userToCreate, isPersistent: false);
                
                }
            }
            
            //Challenge: Requests authentication by the user (e.g. showing a login page)
            //RedirectURI: where user is redirected after authentication
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl }, "GitLab");
        }
    }
}