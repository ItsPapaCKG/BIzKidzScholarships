using BizKidzScholarships.API.Services;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BizKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser<Guid>> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private SignInManager<IdentityUser<Guid>> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;
        private IUserDataService _udService;
        private ICurrentUser _user;

        public AuthController(UserManager<IdentityUser<Guid>> uM, RoleManager<IdentityRole<Guid>> rM, SignInManager<IdentityUser<Guid>> siM, IHttpContextAccessor acc, IUserDataService svc, ICurrentUser usr)
        {
            _userManager = uM;
            _roleManager = rM;
            _signInManager = siM;
            _httpContextAccessor = acc;
            _udService = svc;
            _user = usr;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new IdentityUser<Guid>
            {
                Email = registration.Email,
                UserName = registration.Email,
                PhoneNumber = registration.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, registration.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _udService.SetGlobalTasksForUser(newUser.Id);

            return Ok(new { Message = "User registered successfully", Redirect = "http://example.com/" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
                return Unauthorized();

            var check = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!check.Succeeded) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            await _signInManager.SignInWithClaimsAsync(
                user,
                isPersistent: true,
                roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList()
            );

            return Ok(new { Message = "Login Successful" });

        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            //var user = await _userManager.FindByIdAsync(_user.Id.ToString());
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).Distinct().ToList();

            return Ok(new
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                email = User.FindFirstValue(ClaimTypes.Email),
                roles
            });
        }

        [Authorize]
        [HttpGet("GetAdminRole")]
        public async Task<IActionResult> AdminRole()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(_user.Id.ToString());

                if (user is null)
                {
                    return BadRequest("User not found");
                }

                var result = await _userManager.AddToRoleAsync(user, "Admin");
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User added to role 'Admin' successfully.");
        }
    }
}
