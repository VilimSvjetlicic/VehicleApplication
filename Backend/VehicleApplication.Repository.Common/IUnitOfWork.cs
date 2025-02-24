using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    DbContext dbContext { get; }

    Task<int> CommitAsync();
    Task<int> AddAsync<T>(T entity) where T : class;
    Task<int> UpdateAsync<T>(T entity) where T : class;
    Task<int> DeleteAsync<T>(T entity) where T : class;
    Task<int> DeleteAsync<T>(int id) where T : class;
}
