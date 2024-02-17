namespace Server.Domain;

public class Vehicle
{
    public int Id { get; set; }
    public string Immatriculation { get; set; }
    public int Year { get; set; }
    public int Kilometers { get; set; }
    public int VehicleModelId { get; set; }
    public VehicleModel VehicleModel { get; set; } = null!;
    public IList<VehicleMaintenance> VehicleMaintenances { get; set; } = new List<VehicleMaintenance>();
}
