using car_website.Interfaces;
using ImageMagick;

namespace car_website.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadPhotoAsync(IFormFile photo)
        {
            var photoName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Photos", photoName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            string res = await GetPhotoUrlAsync(photoName);
            return res;
        }

        public Task<string> GetPhotoUrlAsync(string photoName)
        {
            var photoUrl = Path.Combine("/Photos", photoName);
            return Task.FromResult(photoUrl);
        }

        public float GetPhotoAspectRatio(string photoName)
        {
            var filePath = _webHostEnvironment.WebRootPath + photoName.Replace("/", "\\");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Фотография не найдена.", filePath);
            }

            using (var image = new MagickImage(filePath))
            {
                return (float)image.Width / image.Height;
            }
        }
    }
}