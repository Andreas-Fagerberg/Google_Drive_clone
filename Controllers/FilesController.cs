using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
    {
        try
        {
            var userId = UserValidation.GetRequiredUserId(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _fileService.UploadFileAsync(request, userId);

            return Ok(FileUploadResponse.FromEntity(response));
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (DuplicateItemException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while uploading the file");
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var userId = UserValidation.GetRequiredUserId(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _fileService.GetFileAsync(id, userId);

            return File(
                fileContents: response.Content,
                contentType: response.ContentType,
                fileDownloadName: response.FileName
            );
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (ItemOwnershipException)
        {
            return Forbid();
        }
        catch (FileDataNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, $"An unexpected error occured while retrieving the file");
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = UserValidation.GetRequiredUserId(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            await _fileService.DeleteFileAsync(id, userId);

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while deleting the file");
        }
    }
}
