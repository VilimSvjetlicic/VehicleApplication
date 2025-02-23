using System.ComponentModel.DataAnnotations;

namespace VehicleApplication.WebAPI.Models
{
    public class VehicleModelDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Abrv { get; set; }
        [Required]
        public int MakeId { get; set; }
    }
}
