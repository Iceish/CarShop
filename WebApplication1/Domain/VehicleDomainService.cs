using System.ComponentModel.DataAnnotations;

namespace Server.Domain;

public class VehicleDomainService
{
    public ValidationException? Validate(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            return new ValidationException("Vehicle cannot be null.");
        }

        if (vehicle.Immatriculation.Length < 7 || vehicle.Immatriculation.Length > 9)
        {
            return new ValidationException("Immatriculation must be between 7 and 9 characters long.");
        }

        if (vehicle.Year < 0)
        {
            return new ValidationException("Year must be positive.");
        }

        if (vehicle.Kilometers < 0)
        {
            return new ValidationException("Kilometers must be a positive number.");
        }

        return null;
    }
}
