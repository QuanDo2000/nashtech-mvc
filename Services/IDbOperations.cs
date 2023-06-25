namespace MyWebApp.Services;

public interface IDbOperations<T>
{
  bool CheckNull();
  Task Create(T entity);
  Task Update(T entity);
  Task Delete(int id);
  Task<List<T>> List();
  Task<T?> GetById(int id);
  Task<List<T>> GetByFilters(Dictionary<string, string> filters);
}
