using Azure.Storage.Blobs;
using MePoC.BlobStorage.Models;

namespace MePoC.BlobStorage;
public class BlobStorageService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;

    public BlobStorageService(IConfiguration config)
    {
        var blobConfig = BlobStorageServiceConfig.New(config);
        // Choose appropriate authn mechanism: https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-dotnet-get-started?tabs=azure-ad

        _blobServiceClient = new BlobServiceClient(blobConfig.ConnectionString);
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(blobConfig.Container);
    }

    public async Task<string> UploadBlobAsync(Stream stream, string name, CancellationToken cancellationToken)
    {
        if (stream.Position != 0)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        var blobUploadOptions = new Azure.Storage.Blobs.Models.BlobUploadOptions
        {
            Metadata = new Dictionary<string, string>
            {
                { "Test Files", name }
                // { "Index",  indexName }
            }
        };

        try
        {
            await _blobContainerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.None, default, default, cancellationToken);
            var blobName = $"/{name}";
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(stream, blobUploadOptions, cancellationToken);

            return blobName;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error uploading blob: {ex.Message}");
        }
    }

    public async Task<Stream> GetBlobStream(string containerName, string blobName, CancellationToken cancellationToken)
    {
        var blob = _blobContainerClient.GetBlobClient(blobName);

        if (!await blob.ExistsAsync(cancellationToken))
        {
            throw new InvalidOperationException($"Blob {blobName} does not exist");
        }

        return await blob.OpenReadAsync(new Azure.Storage.Blobs.Models.BlobOpenReadOptions(false), cancellationToken);
    }
}
