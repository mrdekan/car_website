using car_website.Interfaces;
using MongoDB.Bson;

namespace car_website.Services
{
    public class CarDeleteService
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IImageService _imageService;
        public CarDeleteService(ICarRepository carRepository, IUserRepository userRepository, IImageService imageService)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _imageService = imageService;
        }
        public async Task<bool> Delete(string id)
        {
            if (ObjectId.TryParse(id, out var carId))
                return await Delete(carId);
            return false;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null) return false;
            try
            {
                List<string> photosToDeletion = car.PhotosURL.ToList();
                photosToDeletion.Add(car.PreviewURL);
                _imageService.DeletePhotos(photosToDeletion);
                var users = await _userRepository.GetAll();
                foreach (var user in users)
                {
                    if (user.Favorites != null && user.Favorites.Contains(id))
                    {
                        user.Favorites.Remove(id);
                        await _userRepository.Update(user);
                    }
                    if (user.Id.ToString() == car.SellerId)
                    {
                        user.CarsForSell.Remove(car.Id);
                        await _userRepository.Update(user);
                    }
                }
                await _carRepository.Delete(car);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
