using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAuth.Authorization;
using TestAuth.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TestAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [CustomAuthorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtUtils _jwtUtils;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtUtils jwtUtils)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.roleManager = roleManager;
            _jwtUtils = jwtUtils;
        }

        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<List<IdentityUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Name
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]
        [CustomAllowAnonymous]
        public async Task<IActionResult> Login(AuthenticationRequest details)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await _userManager.FindByNameAsync(details.UserName);

                if (user != null)
                {
                    var signinResult = await _signInManager.CheckPasswordSignInAsync(user, details.Password, false);

                    if (signinResult.Succeeded)
                    {
                        var token = _jwtUtils.GenerateJwtToken(user);
                        
                        return Ok(new AuthenticationResponse(user, token));
                    }

                }
                else
                    return BadRequest(new { message = "Username or password is incorrect" });
            }

            return BadRequest(new { message = "model is invalid" });
        }

        [HttpPost]
        [Route("EditRole")]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd)
                {
                    IdentityUser? user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            return BadRequest();
                        }
                    }
                }

                foreach (string userId in model.IdsToDelete)
                {
                    IdentityUser? user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            return BadRequest();
                        }
                    }
                }

                return Ok();
            }

            return BadRequest("model is not valid");
        }
    }
}
