using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Minimarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public DownloadController(IWebHostEnvironment env) => _env = env;

        [HttpGet("postman-json")]
        public IActionResult DownloadPrivateJson()
        {
            // Buscar en wwwroot (WebRootPath)
            var fullPath = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "Minimarket.postman_collection.json");
            if (!System.IO.File.Exists(fullPath)) return NotFound();
            return PhysicalFile(fullPath, "application/json", "Minimarket.postman_collection.json");
        }
    }
}
