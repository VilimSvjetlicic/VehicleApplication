using System.ComponentModel.DataAnnotations;

namespace VehicleApplication.WebAPI.Models
{
    public class VehicleMakeDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Abrv { get; set; }
    }
}
