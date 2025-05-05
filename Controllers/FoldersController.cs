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
    public async Task<IActionResult> Create(FolderCreateRequest folder)
    {
        var response = await _folderService.CreateFolderAsync(folder);

        if (response == null)
        {
            return BadRequest("Failed to create folder.");
        }

        return Ok("Folder successfully created at: " + response);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        var response = await _folderService.DeleteFolderAsync(id);

        if (response == null)
        {
            return BadRequest("Failed to delete folder.");
        }

        return Ok("Folder successfully deleted at: " + response);
    }
}
