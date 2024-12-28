using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Repository;
public interface IRepository
{
    Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity;
    Task<T>? GetByIdAsync<T>(string id, CancellationToken cancellationToken) where T : BaseEntity;
    Task<List<T>> ListAsync<T>(CancellationToken cancellationToken) where T : BaseEntity;
    Task UpdateAsync<T>(T modifiedEntity, CancellationToken cancellationToken) where T : BaseEntity;
    void DetachModel<T>(T entity) where T : BaseEntity;
}
