using System.ComponentModel.DataAnnotations;

namespace Server.Domain;

public class VehicleMaintenance
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "ID must be positive.")]
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    public Vehicle Vehicle { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Kilometers must be positive.")]
    public int Kilometers { get; set; }

    [Required]
    [StringLength(128, MinimumLength = 1)]
    public string Description { get; set; }
}
