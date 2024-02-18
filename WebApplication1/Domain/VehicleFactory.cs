using Server.Domain;
using Shared.ApiModels;
using Shared.Enums;

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
                FuelType = (VehicleFuelType)dbEntity.FuelType,
                VehicleModelId = dbEntity.VehicleModelId,
                VehicleModel = VehicleModelFactory.ConvertToApiModel(dbEntity.VehicleModel),
                Maintenances = dbEntity.VehicleMaintenances.Select(VehicleMaintenanceFactory.ConvertToApiModel).ToList(),
                NextMaintenanceDelta = dbEntity.VehicleModel.MaintenanceFrequency - (dbEntity.Kilometers - (dbEntity.VehicleMaintenances.Any() ? dbEntity.VehicleMaintenances.Max(x => x.Kilometers):0))
            };
        }
    }
}
