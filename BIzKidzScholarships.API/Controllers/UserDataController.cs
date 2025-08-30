using Microsoft.AspNetCore.Mvc;

namespace BIzKidzScholarships.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserDataController
    {
        [HttpGet]
        public IActionResult Get(int userId)
        {

        }
    }
}
