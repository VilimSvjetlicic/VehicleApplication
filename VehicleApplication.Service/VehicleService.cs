using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Common;
using VehicleApplication.Model;
using VehicleApplication.Model.Common;
using VehicleApplication.Service.Common;

namespace VehicleApplication.Service
{
    public class VehicleService<T>: IVehicleService<T> where T : IDataModel
    {
        private readonly IGenericRepository<T> vehicleRepository;

        public VehicleService(IGenericRepository<T> vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await vehicleRepository.GetAllAsync();
        }
        public async Task<IDataModel> GetByIdAsync(int id)
        {
            return await vehicleRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(T dataModel)
        {
            await vehicleRepository.AddAsync(dataModel);
        }

        public async Task UpdateAsync(T dataModel)
        {
            await vehicleRepository.UpdateAsync(dataModel);
        }

        public async Task DeleteAsync(int id)
        {
            await vehicleRepository.DeleteAsync(id);
        }

        public async Task CommitAsync()
        {
            await vehicleRepository.CommitAsync();
        }
    }
}
