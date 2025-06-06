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
    public async Task<IActionResult> Create(FolderCreateRequest request)
    {
        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException();

        try
        {
            var response = await _folderService.CreateFolderAsync(request, userId);

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }
        catch (DuplicateItemException ex)
        {
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occured while creating the folder.");
        }
    }

    [Authorize]
    [HttpDelete("id")]
    public async Task<IActionResult> Get(int id)
    {
        var userId =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException();

        try
        {
            var response = await _folderService.GetFolderAsync(id, userId);

            return Ok(FolderCreateResponse.FromEntity(response));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An unexpected error occured while retrieving the folder.");
        }

        
    }

    [Authorize]
    [HttpPut("id")]
    public async Task<IActionResult> Update([FromBody] string folderName, int id)
    {
        throw new NotImplementedException();
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
