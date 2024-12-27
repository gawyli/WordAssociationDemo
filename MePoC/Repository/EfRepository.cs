using MePoC.Common;
using MePoC.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MePoC.Repository;
public class EfRepository : IRepository
{
    private readonly AppDbContext _appDbContext;

    public EfRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public T? GetByIdAsync<T>(string id, CancellationToken cancellationToken) where T : BaseModel
    {
        return _appDbContext.Set<T>().FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<T>> ListAsync<T>(CancellationToken cancellationToken) where T : BaseModel
    {
        return await _appDbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<string> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseModel
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            entity.Id = Guid.NewGuid().ToString();
        }

        try
        {
            await _appDbContext.AddAsync(entity);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding entity", ex);
        }
    }

    public async Task UpdateAsync<T>(T modifiedEntity, CancellationToken cancellationToken) where T : BaseModel
    {
        try
        {
            _appDbContext.Update(modifiedEntity);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating entity", ex);
        }

    }

    public void DetachModel<T>(T entity) where T : BaseModel
    {
        _appDbContext.Entry(entity).State = EntityState.Detached;
    }


    //public Task RemoveByNameAsync<T>(string name, CancellationToken cancellationToken) where T : BaseModelWithName, IAggregateRoot
    //{
    //    throw new NotImplementedException();
    //}

    //public async Task<T?> GetByNameAsync<T>(string name, CancellationToken cancellationToken) where T : BaseModelWithName    //, IAggregateRoot
    //{
    //    return await _appDbContext.Set<T>().FirstOrDefaultAsync(T => T.Name == name, cancellationToken);
    //}

    //public async Task<List<string>> ListNamesAsync<T>(CancellationToken cancellationToken) where T : BaseModelWithName  //, IAggregateRoot
    //{
    //    return await _appDbContext.Set<T>().Select(T => T.Name).ToListAsync(cancellationToken);
    //}
}
