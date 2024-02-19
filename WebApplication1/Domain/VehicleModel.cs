using Shared.Enums;

namespace Server.Domain;
public class VehicleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public VehicleBrand Brand { get; set; }
    public int MaintenanceFrequency { get; set; }
}
