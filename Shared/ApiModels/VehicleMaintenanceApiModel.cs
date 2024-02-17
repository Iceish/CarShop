namespace Shared.ApiModels;

public class VehicleMaintenanceApiModel
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime Date { get; set; }
    public int Kilometers { get; set; }
    public string Description { get; set; }
}
