using Microsoft.AspNetCore.Mvc;

public interface IFileService
{
    public Task<FileUploadResponse> UploadFileAsync();
}
