using Shared.Enums;

namespace Shared.ApiModels;

public class VehicleCreateApiModel
{
    public string Immatriculation { get; set; }
    public int Year { get; set; }
    public int Kilometers { get; set; } = 0;
    public VehicleFuelType FuelType { get; set; } = VehicleFuelType.Other;
    public int VehicleModelId { get; set; }
}
