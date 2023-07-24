using car_website.Interfaces;

namespace car_website.Services
{
    public class CloudimageService : ICloudimageService
    {
        private const string CloudimageBaseUrl = "https://your-cloudimage-base-url.cloudimg.io";

        private readonly HttpClient _httpClient;

        public CloudimageService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> UploadAndProcessImageAsync(byte[] imageBytes)
        {
            try
            {
                // Загрузка изображения на Cloudimage
                string imageUrl = await UploadImageToCloudimageAsync(imageBytes);

                // Добавьте здесь логику для обработки изображения на Cloudimage, если необходимо.
                // Например, изменение размеров, обрезка, конвертация формата и т. д.

                // Возвращаем ссылку на обработанное изображение
                return imageUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при обработке изображения: {ex.Message}");
            }
        }

        private async Task<string> UploadImageToCloudimageAsync(byte[] imageBytes)
        {
            try
            {
                using (var content = new ByteArrayContent(imageBytes))
                {
                    // Опционально, можно задать Content-Type для файла.
                    // Например, для JPEG изображений: "image/jpeg"
                    // Для PNG: "image/png", и т. д.
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    // Отправляем POST-запрос на Cloudimage для загрузки изображения
                    HttpResponseMessage response = await _httpClient.PostAsync($"{CloudimageBaseUrl}/your-processing-options", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string imageUrl = await response.Content.ReadAsStringAsync();
                        return imageUrl;
                    }
                    else
                    {
                        throw new Exception($"Ошибка при загрузке изображения на Cloudimage: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Произошла ошибка при загрузке изображения на Cloudimage: {ex.Message}");
            }
        }
    }
}
