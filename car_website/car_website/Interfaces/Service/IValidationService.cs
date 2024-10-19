namespace car_website.Interfaces.Service
{
    public interface IValidationService
    {
        /// <summary>
        /// Accepts a phone number which will later try to convert to a unified saving format
        /// </summary>
        /// <returns>Correctness of the returned phone number</returns>
        public bool FixPhoneNumber(ref string phone);
        /// <summary>
        /// Obsolete. Use FixPhoneNumber instead.
        /// </summary>
        /// <returns>Result of validation</returns>
        public bool IsValidPhoneNumber(string phone);
        public bool IsValidName(string name);
        /// <summary>
        /// Checks the existence of the video on YouTube
        /// </summary>
        /// <returns>Result of validation</returns>
        public Task<bool> DoesThisVideoExist(string videoId);
        /// <summary>
        /// Checks the correctness of the link and takes the video ID from there
        /// </summary>
        /// <returns>Result of validation</returns>
        public bool GetVideoIdFromUrl(string url, out string videoId);
        /// <summary>
        /// By default compares with 20Mb
        /// </summary>
        /// <returns>True if less or False if not</returns>
        public bool IsLessThenNMb(IFormFile file, int maxSizeMb = 20);
        /// <summary>
        /// Checks password length
        /// </summary>
        /// <returns>Result of validation</returns>
        bool IsValidPassword(string password);
    }
}
