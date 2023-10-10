namespace car_website.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Add(T obj);
        Task Update(T pbj);
        Task Delete(T obj);
    }
}
