namespace ChallengeCore.Shareed;

public interface IRepository<T> : IGetAllRepository<T>, IGetByIdRepository<T>, IAddRepository<T>, IUpdateRepository<T>, IDeleteRepository<T>;

public interface IGetAllRepository<T>
{
    IEnumerable<T> GetAll();
}

public interface IGetByIdRepository<T>
{
    T? GetById(int id);
}

public interface IAddRepository<T>
{
    void Add(T entity);
}

public interface IUpdateRepository<T>
{
    void Update(T entity);
}

public interface IDeleteRepository<T>
{
    void Delete(T entity);
}
