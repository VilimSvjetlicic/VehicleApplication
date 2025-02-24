using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Model;
using VehicleApplication.Model.Common;

namespace VehicleApplication.Service.Common
{
    public interface IVehicleService<T> where T : IDataModel
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IDataModel> GetByIdAsync(int id);
        Task<IDataModel> AddAsync(T vehicleMake);
        Task<bool> UpdateAsync(T vehicleMake);
        Task<bool> DeleteAsync(int id);
        Task<bool> CommitAsync();
    }
}
