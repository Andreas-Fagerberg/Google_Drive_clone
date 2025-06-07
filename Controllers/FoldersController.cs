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
            return BadRequest(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred while creating the folder.");
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _folderService.GetFolderAsync(id, userId);

            return Ok(FolderSummary.FromEntity(response));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FolderDataNotFoundException)
        {
            return NotFound();
        }
        catch (FolderOwnershipException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while retrieving the folder.");
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _folderService.GetAllUserFoldersAsync(userId);

            return Ok(FoldersSummary.FromEntities(response));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FoldersDataNotFoundException)
        {
            return NotFound();
        }
        catch (FolderOwnershipException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while retrieving the folders.");
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] FolderUpdateRequest request, int id)
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            var response = await _folderService.UpdateFolderAsync(request, id, userId);

            return Ok(FolderUpdateResponse.FromEntity(response));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (DuplicateItemException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FoldersDataNotFoundException)
        {
            return NotFound();
        }
        catch (FolderOwnershipException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred while updating the folder.");
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            await _folderService.DeleteFolderAsync(id, userId);
            return NoContent();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FoldersDataNotFoundException)
        {
            return NotFound();
        }
        catch (FolderOwnershipException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while deleting the folder.");
        }
    }
}
