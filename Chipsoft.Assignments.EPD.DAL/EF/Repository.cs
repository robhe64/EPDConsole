namespace Chipsoft.Assignments.EPD.DAL.EF;

public abstract class Repository<T>(EpdDbContext context) : IRepository<T> where T : class
{
    protected readonly EpdDbContext Context = context;
    
    public T? Read(Guid id)
    {
        return Context.Set<T>().Find(id);
    }

    public IEnumerable<T> ReadAll()
    {
        return Context.Set<T>().ToList();
    }
    
    public void Create(T entity)
    {
        Context.Set<T>().Add(entity);
        Context.SaveChanges();
    }
    
    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
        Context.SaveChanges();
    }
    
    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
        Context.SaveChanges();
    }
}