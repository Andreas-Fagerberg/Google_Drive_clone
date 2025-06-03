using System.Security.Claims;
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
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            throw new UnauthorizedAccessException();
        }
        try
        {
            var response = await _folderService.CreateFolderAsync(folder, userId);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }
        catch (NameAlreadyExistsException ex)
        {
            return BadRequest();
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Get(int id)
    {
        throw new NotImplementedException();
        // var response = await _folderService.GetFolderAsync(id);

        // return Ok(response);
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
