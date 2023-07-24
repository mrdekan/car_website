using car_website.Interfaces;

namespace car_website.Services
{
    public class ImageService : IImageService
    {
        private readonly string _storagePath;

        public ImageService(IConfiguration configuration)
        {
            _storagePath = configuration["PhotoStoragePath"];
        }

        public async Task<string> UploadPhotoAsync(IFormFile photo)
        {
            var photoName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(_storagePath, photoName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return photoName;
        }

        public Task<string> GetPhotoUrlAsync(string photoName)
        {
            var filePath = Path.Combine(_storagePath, photoName);
            if (!File.Exists(filePath))
                return Task.FromResult<string>(null);

            return Task.FromResult(filePath);
        }
    }
}