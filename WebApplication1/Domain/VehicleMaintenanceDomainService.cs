using System.ComponentModel.DataAnnotations;

namespace Server.Domain;

public class VehicleMaintenanceDomainService
{
    public ValidationException? Validate(VehicleMaintenance vehicleMaintenance)
    {
        if (vehicleMaintenance.Kilometers < 0)
            return new ValidationException("Kilometers must be greater than or equal to 0.");

        if (vehicleMaintenance.Description.Length < 0 || vehicleMaintenance.Description.Length > 500)
            return new ValidationException("Description must be between 0 and 500 characters.");

        return null;
    }
}
