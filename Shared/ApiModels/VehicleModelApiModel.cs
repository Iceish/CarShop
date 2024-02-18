using Shared.Enums;

namespace Shared.ApiModels;

public class VehicleModelApiModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public VehicleBrand Brand { get; set; }
    public int MaintenanceFrequency { get; set; }
}
