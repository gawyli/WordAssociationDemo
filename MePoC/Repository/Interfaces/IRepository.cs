using MePoC.Common;

namespace MePoC.Repository.Interfaces;
public interface IRepository
{
    Task<string> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseModel;
    T? GetByIdAsync<T>(string id, CancellationToken cancellationToken) where T : BaseModel;
    Task<List<T>> ListAsync<T>(CancellationToken cancellationToken) where T : BaseModel;
    Task UpdateAsync<T>(T modifiedEntity, CancellationToken cancellationToken) where T : BaseModel;
    void DetachModel<T>(T entity) where T : BaseModel;

    //Task<T?> GetByNameAsync<T>(string name, CancellationToken cancellationToken) where T : BaseModelWithName;    //, IAggregateRoot;
    //Task<List<string>> ListNamesAsync<T>(CancellationToken cancellationToken) where T : BaseModelWithName;  //, IAggregateRoot;
    //Task RemoveByNameAsync<T>(string name, CancellationToken cancellationToken) where T : BaseModelWithName, IAggregateRoot;
}

