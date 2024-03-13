namespace Pharmacix.Services.Interfaces;

public interface IRepository<T>
{
    public bool Create(T entity);
    
    public bool Update(T medicament);
    
    public bool Delete(T entity);
    
    public bool Delete(int id);
    
    public List<T> GetAll();

    public T? GetById(int id);
}