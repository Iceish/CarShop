using Shared.Enums;

namespace Shared.ApiModels;

public class VehicleApiModel
{
    public int Id { get; set; }
    public string Immatriculation { get; set; }
    public int Year { get; set; }
    public int Kilometers { get; set; } = 0;
    public VehicleFuelType FuelType { get; set; } = VehicleFuelType.Other;
    public int VehicleModelId { get; set; }
    public VehicleModelApiModel? VehicleModel { get; set; } = null!;
    public IList<VehicleMaintenanceApiModel> Maintenances { get; set; } = new List<VehicleMaintenanceApiModel>();
    public int NextMaintenanceDelta { get; set; }
}
