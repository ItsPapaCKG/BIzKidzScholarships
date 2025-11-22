using BizKidzScholarships.Data.dto;
using BizKidzScholarships.Data.NetworkedModels;
using BizKidzScholarships.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BizKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserDataController : ControllerBase
    {

        private ICurrentUser _user;
        private IUserDataService _udService;
        private UserManager<IdentityUser<Guid>> _userManager;
        private TaskFileUploadService _fileService;
        public UserDataController(ICurrentUser currentUser, IUserDataService svc, UserManager<IdentityUser<Guid>> UserManager, TaskFileUploadService fus)
        {
            _user = currentUser;
            _udService = svc;
            _userManager = UserManager;
            _fileService = fus;
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
        // TODO: Include step to upload byte[] image to S3, retrieve the link, and set to profile column LogoKey
        public async Task<IActionResult> RegisterProfile([FromBody] RegisterUserProfileDTO profile)
        {
            var result = await _udService.SetUserProfile(_user.Id, profile);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Profile successfull registered." });
        }

        [HttpPut("[action]")]
        // TODO: Include step to upload byte[] image to S3, retrieve the link, and set to profile column LogoKey
        public async Task<IActionResult> UpdateProfile([FromBody] RegisterUserProfileDTO profile)
        {
            var result = await _udService.SetUserProfile(_user.Id, profile);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Profile successfull updated." });
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = _user.Id;
            var tasks = await _udService.GetUserTasks(userId);

            return Ok(tasks);
        }

        [HttpGet("[action]")]
        public IActionResult UserPoints()
        {
            var userId = _user.Id;
            var pointsView = _udService.GetUserPoints(userId);

            if (pointsView is null)
                return BadRequest("No points information found");

            return Ok(pointsView);
        }

        [HttpGet("[action]")]
        public IActionResult GetPresignedURL()
        {
            var url = _fileService.Generate_Presigned_URL();

            return Ok(url);
        }

        [HttpPost]
        public IActionResult SetUserProfileS3Key(string key)
        {

        }

    }
}
