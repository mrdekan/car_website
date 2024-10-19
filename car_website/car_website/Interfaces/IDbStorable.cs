using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IDbStorable
    {
        ObjectId Id { get; }
    }
}
