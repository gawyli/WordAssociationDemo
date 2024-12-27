namespace MePoC.BlobStorage;
public interface IBlobService
{
    Task<string> UploadBlobAsync(Stream stream, string name, CancellationToken cancellationToken);
    Task<Stream> GetBlobStream(string containerName, string blobName, CancellationToken cancellationToken);
}
