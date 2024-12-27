using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Text;


namespace MePoC.Utilities.Request;
public static class MultipartRequestHelper
{
    public static MediaTypeHeaderValue? GetMediaTypeHeaderWithBoundary(this HttpRequest request)
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"

        if (request.HasFormContentType && MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) && !String.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
        {
            return mediaTypeHeader;
        }

        return null;
    }

    public static bool ContainsFormDataContent(this MultipartSection section, out string name, out string value)
    {
        // Content-Disposition: form-data; name="key"
        // value

        name = String.Empty;
        value = String.Empty;

        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
        if (hasContentDispositionHeader &&
            contentDisposition!.DispositionType.Equals("form-data") &&
            String.IsNullOrEmpty(contentDisposition.FileName.Value) &&
            String.IsNullOrEmpty(contentDisposition.FileNameStar.Value))
        {
            name = contentDisposition.Name.Value!;
            using (var streamReader = new StreamReader(section.Body, Encoding.UTF8))
            {
                value = streamReader.ReadToEnd();
            }
            return true;

        }

        return false;
    }

    public static bool ContainsFileContent(this MultipartSection section, out string fileName)
    {
        // Content-Disposition: form-data; name="myfile"; filename="Misc 002.jpg"

        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);
        if (hasContentDispositionHeader && contentDisposition!.DispositionType.Equals("form-data") &&
            (!String.IsNullOrEmpty(contentDisposition.FileName.Value) || !String.IsNullOrEmpty(contentDisposition.FileName.Value)))
        {
            fileName = contentDisposition!.FileNameStar.Value ?? contentDisposition.FileName.Value;
            return true;
        }
        else
        {
            fileName = string.Empty;
            return false;
        }
    }

    // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
    // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
    /*public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        if (boundary.Length > lengthLimit)
        {
            throw new InvalidDataException(
                $"Multipart boundary length limit {lengthLimit} exceeded.");
        }

        return boundary;
    }*/

    public static bool IsMultipartContentType(this HttpRequest request)
    {
        var contentType = request.ContentType;
        return !String.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
    {
        // Content-Disposition: form-data; name="key";
        return contentDisposition != null
               && contentDisposition.DispositionType.Equals("form-data")
               && String.IsNullOrEmpty(contentDisposition.FileName.Value)
               && String.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
    }

    public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
    {
        // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
        return contentDisposition != null
               && contentDisposition.DispositionType.Equals("form-data")
               && (!String.IsNullOrEmpty(contentDisposition.FileName.Value)
                   || !String.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
    }
}