using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleApplication.Model.Common;

namespace VehicleApplication.Model
{
    public class VehicleModel : IDataModel
    {
        public int Id { get; set; }
        public int MakeId { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public VehicleMake Make { get; set; }
    }
}
