using Server.Domain;
using Shared.ApiModels;

namespace WebApplication1.Domain;

public class VehicleMaintenanceFactory
{
    public static VehicleMaintenanceApiModel? ConvertToApiModel(VehicleMaintenance? dbEntity)
    {
        if (dbEntity == null)
            return null;

        return new VehicleMaintenanceApiModel
        {
            Id = dbEntity.Id,
            VehicleId = dbEntity.VehicleId,
            Date = dbEntity.Date,
            Kilometers = dbEntity.Kilometers,
            Description = dbEntity.Description
        };
    }
}
