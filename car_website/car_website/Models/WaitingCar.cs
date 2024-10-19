using car_website.Interfaces;
using car_website.Interfaces.Repository;
using car_website.Interfaces.Service;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class WaitingCar : IDbStorable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public WaitingCar(Car car, bool edited)
        {
            Car = car;
            Rejected = false;
            Edited = edited;
            OldPhotosToDeletion = new();
            NewPhotosToDeletion = new();
        }
        public bool Rejected { get; private set; }
        public string Description { get; private set; }
        public Car Car { get; set; }
        //Obsolete
        public bool OtherModel { get; set; }
        //Obsolete
        public bool OtherBrand { get; set; }
        public bool? Edited { get; private set; }
        //Delete if edited model approved
        public List<string> OldPhotosToDeletion { get; set; }
        //Delete if edited model rejected
        public List<string> NewPhotosToDeletion { get; set; }
        /// <summary>
        /// Approving the car and deleting old photos if they were changed
        /// </summary>
        public async Task Approve(ICarRepository carRepository, IWaitingCarsRepository waitingCarsRepository, IUserRepository userRepository, IImageService imageService)
        {
            User seller = await userRepository.GetByIdAsync(ObjectId.Parse(Car.SellerId));
            if (Edited != null)
            {
                if (Edited ?? true)
                {
                    await carRepository.Update(Car);
                    imageService.DeletePhotos(OldPhotosToDeletion);
                }
                else
                {
                    seller.CarsForSell.Add(Car.Id);
                    await carRepository.Add(Car);
                }
            }
            else
            {
                Car findCar = await carRepository.GetByIdAsync(Car.Id);
                if (findCar != null)
                {
                    await carRepository.Update(Car);
                    imageService.DeletePhotos(OldPhotosToDeletion);
                }
                else
                {
                    seller.CarsForSell.Add(Car.Id);
                    await carRepository.Add(Car);
                }
            }
            seller.CarWithoutConfirmation.Remove(Id);
            await userRepository.Update(seller);
            await waitingCarsRepository.Delete(this);
        }
        public void Reject(string reason, IImageService imageService)
        {
            Rejected = true;
            Description = reason;
            if (Edited ?? true && NewPhotosToDeletion != null)
                imageService.DeletePhotos(NewPhotosToDeletion);
        }
    }
}
