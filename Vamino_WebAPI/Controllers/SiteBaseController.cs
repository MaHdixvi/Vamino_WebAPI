using Microsoft.AspNetCore.Mvc;

namespace Vamino_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteBaseController : ControllerBase
    {
        // کنترلر پایه برای ارث‌بری توسط سایر کنترلرها
    }
}