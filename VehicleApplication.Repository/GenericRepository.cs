using EFCore = Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Common;
using VehicleApplication.DAL;
using Microsoft.EntityFrameworkCore;


namespace VehicleApplication.Repository
{
    public class GenericRepository<T>(IUnitOfWork unitOfWork, EFCore.DbContext context) : IGenericRepository<T> where T : class
    {
        private IUnitOfWork unitOfWork = unitOfWork;
        private EFCore.DbContext context = context;
        private EFCore.DbSet<T> dbSet;

        public async Task<T> AddAsync(T entity)
        {
            await unitOfWork.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await unitOfWork.DeleteAsync<T>(id);
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            await unitOfWork.UpdateAsync(entity);
            return true;
        }
        public async Task<bool> CommitAsync()
        {
            await unitOfWork.CommitAsync();
            return true;
        }
    }
}
