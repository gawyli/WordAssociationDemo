using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Repository;
public class EfRepository : IRepository
{
    private readonly AppDbContext _dbContext;

    public EfRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public T? GetByIdAsync<T>(string id, CancellationToken cancellationToken) where T : BaseEntity
    {
        return _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<T>> ListAsync<T>(CancellationToken cancellationToken) where T : BaseEntity
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity
    {
        if (!string.IsNullOrEmpty(entity.Id))
        {
            throw new Exception($"Entity with id: {entity.Id}");
        }

        entity.Id = Guid.NewGuid().ToString();

        try
        {
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Error adding entity", ex);
        }
    }

    public async Task UpdateAsync<T>(T modifiedEntity, CancellationToken cancellationToken) where T : BaseEntity
    {
        try
        {
            _dbContext.Update(modifiedEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating entity", ex);
        }

    }

    public void DetachModel<T>(T entity) where T : BaseEntity
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
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
