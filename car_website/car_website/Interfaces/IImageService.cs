namespace car_website.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadPhotoAsync(IFormFile photo);
        Task<string> GetPhotoUrlAsync(string photoName);
    }
}
