using Server.Domain;
using Shared.ApiModels;

namespace WebApplication1.Domain
{
    public static class VehicleModelFactory
    {
        public static VehicleModelApiModel? ConvertToApiModel(VehicleModel? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new VehicleModelApiModel
            {
                Id = dbEntity.Id,
                Name = dbEntity.Name,
                Brand = dbEntity.Brand,
                MaintenanceFrequency = dbEntity.MaintenanceFrequency,
            };
        }

    }
}
