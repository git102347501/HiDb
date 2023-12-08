using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("life")]
    public class LifeController : ControllerBase
    {

        [HttpGet]
        public bool Get()
        {
            return true;
        }
    }
}
