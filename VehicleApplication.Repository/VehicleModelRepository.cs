using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Common;
using VehicleApplication.Model;

namespace VehicleApplication.Repository
{
    internal class VehicleModelRepository : IVehicleModelRepository
    {
        private readonly IGenericRepository<VehicleModel> genericRepository;

        public VehicleModelRepository(IGenericRepository<VehicleModel> genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            return genericRepository.GetAllAsync();
        }

        public Task<VehicleModel> GetByIdAsync(int id)
        {
            return genericRepository.GetByIdAsync(id);
        }

        public Task<VehicleModel> AddAsync(VehicleModel entity)
        {
            return genericRepository.AddAsync(entity);
        }

        public Task<bool> UpdateAsync(VehicleModel entity)
        {
            return genericRepository.UpdateAsync(entity);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return genericRepository.DeleteAsync(id);
        }

    }
}
