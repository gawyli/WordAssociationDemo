using ICSharpCode.SharpZipLib.Zip;
using MePoC.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace MePoC.Utilities;
public static class FileUtils
{
    public static string GetMimeTypeForFileExtension(string filePath)
    {
        const string defaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        string? contentType;
        if (!provider.TryGetContentType(filePath, out contentType))
        {
            contentType = defaultContentType;
        }

        return contentType ?? defaultContentType;
    }

    public static ZipDetails GenerateZipFileDetails()
    {
        string today = DateTime.UtcNow.ToString("yyyyMMdd");
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string subfolderName = "Records";
        string sessionFolderName = $"session{today}-audio-report";

        // Build the full path to the subfolder
        string fullPath = Path.Combine(baseDirectory, subfolderName, sessionFolderName);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        string fileName = $"audio-response{Guid.NewGuid()}.zip";
        string outputPath = Path.Combine(fullPath, fileName);

        var zipDetails = new ZipDetails(fileName, outputPath);

        return zipDetails;
    }

    public static ZipDetails ToZipFile(Record record)
    {
        var zipDetails = GenerateZipFileDetails();
        using (var zipStream = new ZipOutputStream(System.IO.File.Create(zipDetails.Path)))
        {
            zipStream.SetLevel(0);  // store only

            byte[] buffer = new byte[4096];
            var entry = new ZipEntry(record.Path);

            entry.DateTime = DateTime.UtcNow;
            zipStream.PutNextEntry(entry);

            using (var audioStream = System.IO.File.OpenRead(record!.Path))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = audioStream.Read(buffer, 0, buffer.Length);
                    zipStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }

            zipStream.Finish();

            zipStream.Close();
        }

        return zipDetails;
    }
}
