using BizKidzScholarships.API.Services;
using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

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
            {
                var sb = new StringBuilder();
                
                foreach(var entry in ModelState)
                {
                    var errors = entry.Value.Errors.Select(e => e.ErrorMessage).ToList();
                    errors.ForEach(m => { sb.AppendLine(m); sb.AppendLine(); });
                }

                var errorMsg = sb.ToString();

                return BadRequest(errorMsg);
            }

            if (registration.Password != registration.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            if (_udService.GetUserAge(registration.Birthday) < 13)
            {
                return BadRequest("You must be 13 years or older to use this application.");
            }


            var newUser = new IdentityUser<Guid>
            {
                Email = registration.Email,
                UserName = registration.Email,
                PhoneNumber = registration.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, registration.Password);

            if (!result.Succeeded)
            {
                var errorList = result.Errors.Select(e => e.Description).ToList();
                var sb = new StringBuilder();

                errorList.ForEach(e => { sb.Append(e); sb.Append("\n"); });

                return BadRequest(sb.ToString());
            }
                

            var roleResult = await _userManager.AddToRoleAsync(newUser, "Kid");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);

                return BadRequest(roleResult.Errors);
            }

            await _udService.RegisterUserProfile(newUser.Id, registration);

            await _udService.SetGlobalTasksForUser(newUser.Id);

            return Ok();
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
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

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.First());
                }

                await _signInManager.RefreshSignInAsync(user);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User added to role 'Admin' successfully.");
        }
    }
}
