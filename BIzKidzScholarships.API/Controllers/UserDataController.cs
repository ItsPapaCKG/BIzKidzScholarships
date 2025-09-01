using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;
using BIzKidzScholarships.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIzKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserDataController : ControllerBase
    {

        private ICurrentUser _user;
        private IUserDataService _udService;
        public UserDataController(ICurrentUser currentUser, IUserDataService svc)
        {
            _user = currentUser;
            _udService = svc;
        }

        [HttpGet]
        public IActionResult UserProfile()
        {
            var response = _udService.GetUserProfile(_user.Id);

            if (response is null)
                return BadRequest("No profile found.");

            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterProfile([FromBody] RegisterUserProfileDTO profile)
        {
            var result = await _udService.RegisterUserProfile(_user.Id, profile);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Profile successfull registered." });
        }
    }
}
