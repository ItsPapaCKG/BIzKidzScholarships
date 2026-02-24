using BizKidzScholarships.Data.dto;
using Microsoft.AspNetCore.Mvc;

namespace BizKidzScholarships.API.Controllers.Base
{
    public abstract class BaseBKController : Controller
    {
        protected IActionResult RouteResponse(ResponseModel response)
        {
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
