using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace car_website.Models
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole<ObjectId>
    {
    }
}
