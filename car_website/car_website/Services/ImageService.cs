﻿using car_website.Interfaces;
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
        public string CopyPhoto(string photoName)
        {
            var filePath = _webHostEnvironment.WebRootPath + photoName.Replace("/", "\\");
            var photoNameNew = Guid.NewGuid().ToString() + Path.GetExtension(".webp");
            MagickImage img = new(filePath);
            img.Write(Path.Combine(_webHostEnvironment.WebRootPath, "Photos", photoNameNew));
            return photoNameNew;
        }
        public void ProcessImage(int width, int height, string filepath)
        {
            filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Photos", filepath);
            MagickImage img = new(filepath);

            if (img.Height != height || img.Width != width)
            {
                decimal result_ratio = (decimal)height / (decimal)width;
                decimal current_ratio = (decimal)img.Height / (decimal)img.Width;

                bool preserve_width = false;
                if (current_ratio > result_ratio)
                    preserve_width = true;
                int new_width = 0;
                int new_height = 0;
                if (preserve_width)
                {
                    new_width = width;
                    new_height = (int)Math.Round((decimal)(current_ratio * new_width));
                }
                else
                {
                    new_height = height;
                    new_width = (int)Math.Round((decimal)(new_height / current_ratio));
                }

                string geomStr = width.ToString() + "x" + height.ToString();
                string newGeomStr = new_width.ToString() + "x" + new_height.ToString();

                MagickGeometry intermediate_geo = new(newGeomStr);
                MagickGeometry final_geo = new(geomStr);

                img.Resize(intermediate_geo);
                img.Crop(final_geo, Gravity.Center);
            }

            img.Write(filepath);
        }
    }
}