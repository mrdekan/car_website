using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Add(T obj);
        Task Update(T pbj);
        Task Delete(T obj);
        Task<IEnumerable<T>> GetAll();
        long GetCount();
        Task<T> GetByIdAsync(ObjectId id);
        Task<IEnumerable<T>> GetByIdListAsync(IEnumerable<ObjectId> ids);
    }
}
