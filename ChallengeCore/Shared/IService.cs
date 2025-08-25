using FluentResults;

namespace ChallengeCore.Shareed;

public interface IService<T> : IGetAllService<T>, IGetByIdService<T>, IAddService<T>, IUpdateService<T>, IDeleteService;

public interface IGetAllService<T>
{
    Result<IEnumerable<T>> GetAll();
}

public interface IGetByIdService<T>
{
    Result<T> GetById(int id);
}

public interface IAddService<T>
{
    Result Add(T entity);
}

public interface IUpdateService<T>
{
    Result Update(T entity);
}

public interface IDeleteService
{
    Result Delete(int id);
}
