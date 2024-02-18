using Shared.Enums;

namespace Shared.ApiModels;

public class VehicleModelCreateApiModel
{
    public string Name { get; set; }
    public VehicleBrand Brand { get; set; }
    public int MaintenanceFrequency { get; set; }
}
