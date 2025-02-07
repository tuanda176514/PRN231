using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
    [Route("api/[rates]/[action]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingService service;

        public RatingController(RatingService service)
        {
            this.service = service;
        }
    }
}
