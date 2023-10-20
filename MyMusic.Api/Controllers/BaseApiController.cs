using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyMusic.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {

    }
}
