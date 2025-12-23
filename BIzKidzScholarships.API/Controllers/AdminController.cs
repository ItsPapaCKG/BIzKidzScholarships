using BizKidzScholarships.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AdminService adminService;
        public AdminController(AdminService svc)
        {
            adminService = svc;
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
    }
}
