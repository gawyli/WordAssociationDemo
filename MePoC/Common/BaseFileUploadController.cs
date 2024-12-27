using Microsoft.AspNetCore.Mvc;

namespace MePoC.Common;
public class FileModel
{
    public string ClientFileName { get; private set; }
    public string FileId { get; private set; }

    public FileModel(string clientFileName, string fileId)
    {
        this.ClientFileName = clientFileName;
        this.FileId = fileId;
    }
}

public class FileStream : IDisposable
{
    public string FileId { get; private set; }
    public Stream Stream { get; private set; }

    public FileStream(string fileId, Stream stream)
    {
        this.FileId = fileId;
        this.Stream = stream;
    }

    public void Dispose()
    {
        this.Stream.Dispose();
    }
}

public class BadMediaTypeException : Exception
{
    public BadMediaTypeException() : base("Attempt made to upload file with content-type that is not multi-part") { }
}

public abstract class BaseFileUploadController : Controller
{
    private readonly ILogger<BaseFileUploadController> _logger;

    protected BaseFileUploadController(ILogger<BaseFileUploadController> logger)
    {
        _logger = logger;
    }


    //protected abstract Task<string> ProcessStream(IDictionary<string, string> formData, string clientFileName, Stream stream, CancellationToken cancellationToken);

    //protected async Task<IList<FileModel>> ParseRequest(IDictionary<string, string> formData, CancellationToken cancellationToken)
    //{
    //    /*        
    //     * Typical multi-file message format is:
    //     *        
    //    POST /devwfapi/api/form/uploadattachmentsonfly HTTP/1.1
    //    System: xxxxxxxx
    //    AccessToken: ALwzpK59onuqtvZVTOEsKsyEIFNa-xVGc22RqCh8z0Y8HHkKV_w3MLmKnJE_ywj_Aw==
    //    User-Agent: PostmanRuntime/7.29.2
    //    Cache-Control: no-cache
    //    Postman-Token: 9edc5eb6-0397-4520-a03a-13212d1b6996
    //    Host: localhost:5001
    //    Accept-Encoding: gzip, deflate, br
    //    Connection: keep-alive
    //    Content-Type: multipart/form-data; boundary=--------------------------156313382635509050530525
    //    Content-Length: 444312365

    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="name1"
    //    value1
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="name2"
    //    value2
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="files"; filename="Compressed_1.zip"
    //    <Compressed_1.zip>
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="files"; filename="InstallProg.exe"
    //    <InstallProg.exe>
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="files"; filename="PowerSetup-0.64.0-x64.exe"
    //    <PowerSetup-0.64.0-x64.exe>
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="files"; filename="Example_PPT.pdf"
    //    <Example_PPT.pdf>
    //    ----------------------------156313382635509050530525
    //    Content-Disposition: form-data; name="files"; filename="ProgramSetup-x64-1.2.3.exe"
    //    <ProgramSetup-x64-1.2.3.exe>
    //    ----------------------------156313382635509050530525--
    //     * 
    //     *
    //     */

    //    var fileModels = new List<FileModel>();

    //    var mediaTypeHeaderBoundary = VerifyMediaType();
    //    var reader = new MultipartReader(mediaTypeHeaderBoundary, this.Request.Body);
    //    var section = await reader.ReadNextSectionAsync(cancellationToken);
    //    while (section != null)
    //    {
    //        string name, value, clientFileName;
    //        if (section.ContainsFormDataContent(out name, out value))
    //        {
    //            formData.Add(name, value);
    //        }
    //        else if (section.ContainsFileContent(out clientFileName))
    //        {
    //            var fileId = await ProcessStream(formData, clientFileName, section.Body, cancellationToken);
    //            fileModels.Add(new FileModel(clientFileName, fileId));
    //        }

    //        // Drain any remaining section body that hasn't been consumed and read the headers for the next section.
    //        section = await reader.ReadNextSectionAsync(cancellationToken);
    //    }

    //    return fileModels;
    //}

    //private string VerifyMediaType()
    //{
    //    var mediaTypeHeader = this.Request.GetMediaTypeHeaderWithBoundary();
    //    if (mediaTypeHeader == null)
    //    {
    //        _logger.LogWarning("Attempt made to upload file with content-type that is not multi-part");
    //        throw new BadMediaTypeException();
    //    }

    //    return mediaTypeHeader.Boundary.Value!; // mediaTypeHeader is null if boundary is null or empty
    //}
}
