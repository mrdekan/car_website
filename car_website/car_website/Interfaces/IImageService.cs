namespace car_website.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Uploading photo to the webRootPath/Photos/ folder
        /// </summary>
        /// <returns>Photo URL</returns>
        Task<string> UploadPhotoAsync(IFormFile photo, string carInfo = "");
        Task<string> GetPhotoUrlAsync(string photoName);
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="photoName"></param>
        /// <returns>Aspect ratio of the photo (width/height)</returns>
        float GetPhotoAspectRatio(string photoName);
        /// <summary>
        /// Crop the image to the specified size (for car previews, 300 width and 200 height are used)
        /// </summary>
        void ProcessImage(int width, int height, string filepath);
        /// <summary>
        /// Copy photo with a new name
        /// </summary>
        /// <returns>New photo URL</returns>
        string CopyPhoto(string photoName);
        /// <summary>
        /// Deleting a single file (accept photo URL from web root path)
        /// </summary>
        /// <param name="photoName">photo URL from web root path</param>
        /// <returns>True if opeartian was successful, and False if not</returns>
        bool DeletePhoto(string photoName);
        /// <summary>
        /// Deleting all photos from collection
        /// </summary>
        /// <param name="photoNames">collection of photo URLs from web root path</param>
        void DeletePhotos(IEnumerable<string> photoNames);
        /// <summary>
        /// Downloading an IFormFile by URL
        /// </summary>
        Task<IFormFile> DownloadFileAsync(string url);
    }
}
