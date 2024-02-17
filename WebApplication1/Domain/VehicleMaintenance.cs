namespace Server.Domain;

public class VehicleMaintenance
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public DateTime Date { get; set; }
    public int Kilometers { get; set; }
    public string Description { get; set; }
}
