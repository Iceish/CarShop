using Server.Domain;
using Shared.ApiModels;

namespace WebApplication1.Domain
{
    public class VehicleFactory
    {
        public static VehicleApiModel? ConvertToApiModel(Vehicle? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new VehicleApiModel
            {
                Id = dbEntity.Id,
                Immatriculation = dbEntity.Immatriculation,
                Year = dbEntity.Year,
                Kilometers = dbEntity.Kilometers,
                VehicleModelId = dbEntity.VehicleModelId,
                VehicleModel = VehicleModelFactory.ConvertToApiModel(dbEntity.VehicleModel),
                Maintenances = dbEntity.VehicleMaintenances.Select(VehicleMaintenanceFactory.ConvertToApiModel).ToList(),
                NextMaintenanceDelta = dbEntity.VehicleModel.MaintenanceFrequency - (dbEntity.Kilometers - (dbEntity.VehicleMaintenances.Any() ? dbEntity.VehicleMaintenances.Max(x => x.Kilometers):0))
            };
        }
    }
}
