using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FoldersController : ControllerBase
{
    private readonly IFolderService _folderService;

    public FoldersController(IFolderService folderService)
    {
        _folderService = folderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateFolder(FolderCreateRequest folder)
    {
        var response = await _folderService.CreateFolderAsync(folder);

        if (response == null)
        {
            return BadRequest("Failed to create folder.");
        }

        return Ok(response);
    }
}
