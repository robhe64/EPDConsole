namespace Chipsoft.Assignments.EPD.DAL.EF;

public abstract class Repository<T>(EpdDbContext context) : IRepository<T> where T : class
{
    public T? Read(Guid id)
    {
        return context.Set<T>().Find(id);
    }
    
    public void Create(T entity)
    {
        context.Set<T>().Add(entity);
        context.SaveChanges();
    }
    
    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
        context.SaveChanges();
    }
    
    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        context.SaveChanges();
    }
}