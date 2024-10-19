using car_website.Interfaces;
using car_website.Interfaces.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : IDbStorable
    {
        private readonly IMongoCollection<T> _collection;
        public BaseRepository(IMongoCollection<T> collection) =>
            _collection = collection;

        public virtual async Task Add(T obj) =>
            await _collection.InsertOneAsync(obj);

        public virtual async Task Update(T obj) =>
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq(c => c.Id, obj.Id), obj);

        public virtual async Task Delete(T obj) =>
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq(c => c.Id, obj.Id));

        public virtual async Task<IEnumerable<T>> GetAll() =>
            await _collection.Find(obj => true).ToListAsync();

        public virtual async Task<T> GetByIdAsync(ObjectId id) =>
            await _collection.Find(obj => obj.Id == id).FirstOrDefaultAsync();

        public virtual async Task<IEnumerable<T>> GetByIdListAsync(IEnumerable<ObjectId> ids) =>
            await _collection.Find(obj => ids.Contains(obj.Id)).ToListAsync();

        public virtual long GetCount()
        {
            try
            {
                return _collection.CountDocuments(Builders<T>.Filter.Where(c => c != null));
            }
            catch
            {
                return 0;
            }
        }
    }
}
