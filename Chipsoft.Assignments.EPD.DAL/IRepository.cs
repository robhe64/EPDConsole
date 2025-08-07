namespace Chipsoft.Assignments.EPD.DAL;

public interface IRepository<T>
{
    T? Read(Guid id);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}