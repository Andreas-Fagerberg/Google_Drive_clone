using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public IActionResult UploadFile()
    {
        return Ok();
    }
}
