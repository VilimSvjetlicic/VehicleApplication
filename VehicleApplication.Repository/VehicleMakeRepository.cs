using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Common;
using VehicleApplication.Model;

namespace VehicleApplication.Repository
{
    public class VehicleMakeRepository : IGenericRepository<VehicleMake>
    {
        private readonly IGenericRepository<VehicleMake> genericRepository;

        public VehicleMakeRepository(IGenericRepository<VehicleMake> genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public Task<IEnumerable<VehicleMake>> GetAllAsync()
        {
            return genericRepository.GetAllAsync();
        }

        public Task<VehicleMake> GetByIdAsync(int id)
        {
            return genericRepository.GetByIdAsync(id);
        }

        public Task<VehicleMake> AddAsync(VehicleMake entity)
        {
            return genericRepository.AddAsync(entity);
        }

        public Task<bool> UpdateAsync(VehicleMake entity)
        {
            return genericRepository.UpdateAsync(entity);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return genericRepository.DeleteAsync(id);
        }
    }
}
