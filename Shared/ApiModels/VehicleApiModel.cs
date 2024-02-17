namespace Shared.ApiModels;

public class VehicleApiModel
{
    public int Id { get; set; }

    public string Immatriculation { get; set; }

    public int Year { get; set; }

    public int Kilometers { get; set; }

    public int VehicleModelId { get; set; }

    public VehicleModelApiModel? VehicleModel { get; set; } = null!;
}
