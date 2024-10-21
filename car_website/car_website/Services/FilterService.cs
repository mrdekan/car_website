namespace car_website.Services
{
    public static class FilterService<T>
    {
        public static IEnumerable<T> FilterPages(IEnumerable<T> all, int page, int perPage, out int totalPages)
        {
            int totalItems = all.Count();
            totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
            int skip = (page - 1) * perPage;
            all = all.Skip(skip).Take(perPage);
            return all;
        }
    }
}
