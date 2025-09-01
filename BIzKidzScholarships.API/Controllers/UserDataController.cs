using BizKidzScholarships.Data.NetworkedModels;
using BIzKidzScholarships.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BIzKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/user")]
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
    }
}
