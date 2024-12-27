//using Microsoft.KernelMemory;

namespace MePoC.BlobStorage.Models;
public class BlobStorageServiceConfig
{
    public string ConnectionString { get; set; } = null!;
    public string EndpointSuffix { get; set; } = null!;
    public string Account { get; set; } = null!;
    public string AccountKey { get; set; } = null!;
    public string Container { get; set; } = null!;
    //public AzureBlobsConfig.AuthTypes Auth { get; set; }


    public BlobStorageServiceConfig()
    {
    }

    public static BlobStorageServiceConfig New(IConfiguration config)
    {
        BlobStorageServiceConfig blobStorageConfig = new();
        config.GetSection("BlobStorageService").Bind(blobStorageConfig);

        return blobStorageConfig;
    }

    //public static AzureBlobsConfig NewAzure(IConfiguration config)
    //{
    //    var azureBlobsCfg = new AzureBlobsConfig();
    //    config.GetSection("BlobStorageService").Bind(azureBlobsCfg);

    //    azureBlobsCfg.Auth = AzureBlobsConfig.AuthTypes.ConnectionString;

    //    return azureBlobsCfg;
    //}
}
