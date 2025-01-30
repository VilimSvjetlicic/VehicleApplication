using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VehicleApplication.DAL;

namespace VehicleApplication.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext dbContext { get; private set; }

        public UnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("DbContext");
            }
            this.dbContext = dbContext;
        }

        public async Task<int> CommitAsync()
        {
            int result = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await dbContext.SaveChangesAsync();
                scope.Complete();
            }
            return result;
        }

        public Task<int> AddAsync<T>(T entity) where T : class
        {
            try
            {
                EntityEntry entityEntry = dbContext.Entry(entity);
                if (entityEntry.State != EntityState.Detached)
                {
                    entityEntry.State = EntityState.Added;
                }
                else
                {
                    dbContext.Set<T>().Add(entity);
                }
                return Task.FromResult(1);
            }
                catch (Exception e)
            {
                throw;
            }
        }

        public virtual Task<int> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                EntityEntry entityEntry = dbContext.Entry(entity);
                if (entityEntry.State == EntityState.Detached)
                {
                    dbContext.Set<T>().Attach(entity);
                }
                entityEntry.State = EntityState.Modified;
                return Task.FromResult(1);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual Task<int> DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                EntityEntry entityEntry = dbContext.Entry(entity);
                if (entityEntry.State != EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Deleted;
                }
                else
                {
                    dbContext.Set<T>().Attach(entity);
                    dbContext.Set<T>().Remove(entity);
                }
                return Task.FromResult(1);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual Task<int> DeleteAsync<T>(int id) where T : class
        {
            var entity = dbContext.Set<T>().Find(id);
            if (entity == null)
            {
                return Task.FromResult(0);
            }
            return DeleteAsync<T>(entity);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }


    }
}
