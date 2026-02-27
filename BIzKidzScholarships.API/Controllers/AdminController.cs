using BizKidzScholarships.API.Controllers.Base;
using BizKidzScholarships.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseBKController
    {
        private AdminService adminService;
        private IUserDataService userDataService;
        public AdminController(AdminService svc, IUserDataService ud)
        {
            adminService = svc;
            userDataService = ud;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsers()
        {
            var results = await adminService.AdminUserList();

            return Ok(results);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Activities()
        {
            var results = await adminService.GetActivities();

            return Ok(results);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Submissions()
        {
            var results = await adminService.GetAllSubmissions();

            return RouteResponse(results);
        }

        [HttpGet("[action]/{taskId}")]
        public async Task<IActionResult> Submissions(int taskId)
        {
            var results = await adminService.GetSubmissions(taskId);

            return RouteResponse(results);
        }

        [HttpGet("[action]/{submissionId}")]
        public async Task<IActionResult> GetSubmission(Guid submissionId) 
        { 
            var res = await adminService.GetSubmissionLink(submissionId);

            return RouteResponse(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> User(Guid userId)
        {
            var res = await userDataService.GetUserProfile(userId);

            return Ok(res);
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> Tasks(Guid userId)
        //{
        //    var res = await userDataService.GetTasks(userId);

        //    return Ok(res);
        //}

        [HttpGet("[action]")]
        public async Task<IActionResult> Tasks()
        {
            var res = await adminService.GetTasksSearch();

            return Ok(res);
        }
    }
}
