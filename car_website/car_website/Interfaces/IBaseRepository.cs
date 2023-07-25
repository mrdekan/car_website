namespace car_website.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task Add(T brand);
        Task Update(T brand);
        Task Delete(T brand);
    }
}
