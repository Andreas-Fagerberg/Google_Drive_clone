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
    public async Task<IActionResult> Upload([FromBody] FileUploadRequest request)
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _fileService.UploadFileAsync(request, userId);

            return Ok(FileUploadResponse.FromEntity(response));
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
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );
            var response = await _fileService.GetFileAsync(id);

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
        catch (FileOwnershipException)
        {
            return Forbid();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while retrieving the file");
        }
    }

    // [Authorize]
    // [HttpPut("{id}")]
    // public IActionResult Update([FromBody] FileUpdateRequest request, int id)
    // {
    //     try
    //     {
    //         var userId = UserValidation.ValidateUser(
    //             User.FindFirstValue(ClaimTypes.NameIdentifier)
    //         );
    //         var response = await _fileService.UpdateFileAsync(request);

    //         return Ok(FileUploadResponse.FromEntity(response));
    //     }
    //     catch (ValidationException ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    //     catch (FileOwnershipException)
    //     {
    //         return Forbid();
    //     }
    //     catch (Exception)
    //     {
    //         return StatusCode(500, "An unexpected error occured while updating the file");
    //     }
    // }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var userId = UserValidation.ValidateUser(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

            _fileService.DeleteFileAsync(id);

            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occured while deleting the file");
        }
    }
}
