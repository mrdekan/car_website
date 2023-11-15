using car_website.Interfaces;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace car_website.Services
{
    public class ValidationService : IValidationService
    {
        private const int MAX_PHOTO_SIZE = 20; //Mb
        private const int NAME_MAX_LENGTH = 30;
        private const string NAME_PATTERN = @"^[а-яА-ЯёЁіІїЇєЄ'\s]+$";
        private const string PHONE_PATTERN = @"^38\d{10}$";
        private readonly IConfiguration _configuration;

        public ValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> DoesThisVideoExist(string videoId)
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetAsync($"https://www.googleapis.com/youtube/v3/videos?id={videoId}&key={_configuration.GetSection("GoogleApiSettings")["ApiKey"]}&part=snippet");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var videoDetails = JsonConvert.DeserializeObject<VideoDetailsResponse>(responseBody);
                    if (videoDetails == null || videoDetails.Items.Length <= 0)
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool GetVideoIdFromUrl(string url, out string videoId)
        {
            videoId = "";
            if (url == null) return false;

            try
            {
                Uri uri = new Uri(url);
                string host = uri.Host.ToLower();

                if (host.Contains("youtube.com"))
                {
                    string queryString = uri.Query;
                    var queryParameters = HttpUtility.ParseQueryString(queryString);
                    videoId = queryParameters["v"];
                    if (string.IsNullOrEmpty(videoId))
                        return false;
                    else return true;
                }
                else if (host.Contains("youtu.be"))
                {
                    string[] segments = uri.Segments;
                    if (segments.Length > 1)
                        videoId = segments[1];
                    else return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        // 1048576 = 1024 * 1024 (b => Mb)
        public bool IsLessThenNMb(IFormFile file, int maxSizeMb = MAX_PHOTO_SIZE) =>
            file != null && ((double)file.Length / (1048576)) <= maxSizeMb;

        public bool FixPhoneNumber(ref string phone)
        {
            if (phone == null) return false;
            phone = phone.Replace("+", "").Replace(" ", "");
            if (IsValidPhoneNumber(phone)) return true;
            if (IsOnlyLetters(phone))
            {
                if (phone.Length == 10)
                {
                    phone = $"38{phone}";
                    return true;
                }
                else if (phone.Length == 9)
                {
                    phone = $"380{phone}";
                    return true;
                }
            }
            return false;
        }
        public bool IsValidName(string name) =>
             name != null && name.Length < NAME_MAX_LENGTH && Regex.IsMatch(name, NAME_PATTERN);
        public bool IsValidPhoneNumber(string phone) =>
            Regex.IsMatch(phone, PHONE_PATTERN);
        private static bool IsOnlyLetters(string s) =>
            Regex.IsMatch(s, @"^\d+$");
    }
    internal class VideoDetailsResponse
    {
        public VideoSnippet[] Items { get; set; }
    }

    internal class VideoSnippet
    {
        public SnippetDetails Snippet { get; set; }
    }

    internal class SnippetDetails
    {
        public string Title { get; set; }
        public string ChannelTitle { get; set; }
    }
}
