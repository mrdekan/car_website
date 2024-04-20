using System.Net;

namespace car_website.Services
{
    public static class IFormFileDownloader
    {
        public static async Task<IFormFile> DownloadFileAsync(string url, string fileName)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) ||
                !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                throw new ArgumentException("Invalid URL");
            }

            try
            {
                using (var client = new WebClient())
                {
                    byte[] fileBytes = await client.DownloadDataTaskAsync(uri);

                    using (var stream = new MemoryStream(fileBytes))
                    {
                        return new FormFile(stream, 0, fileBytes.Length, "fileName", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading file: {ex.Message}", ex);
            }
        }
    }
}
