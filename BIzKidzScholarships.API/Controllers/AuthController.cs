using BizKidzScholarships.Data.dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BIzKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private UserManager<IdentityUser<Guid>> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private SignInManager<IdentityUser<Guid>> _signInManager;

        public AuthController(UserManager<IdentityUser<Guid>> uM, RoleManager<IdentityRole<Guid>> rM, SignInManager<IdentityUser<Guid>> siM)
        {
            _userManager = uM;
            _roleManager = rM;
            _signInManager = siM;
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

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new { Message = "Login Successful" });
            
        }
    }
}
