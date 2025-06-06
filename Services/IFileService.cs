
public interface IFileService
{
    public Task<FileEntity> UploadFileAsync(FileUploadRequest request);
}
