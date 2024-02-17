namespace Server.Domain;

public class VehicleModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Brand { get; set; }

    public int MaintenanceFrequency { get; set; }

    public IList<Vehicle> Vehicles { get; } = new List<Vehicle>();
}
