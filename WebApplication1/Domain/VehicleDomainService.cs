using HotChocolate.Types.Relay;
using Microsoft.Extensions.Options;
using Server.Utils;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain;

namespace Server.Domain
{
    public class VehicleDomainService
    {
        public ValidationException? Validate(Vehicle vehicle)
        {
            if (vehicle.Immatriculation.Length < 7 || vehicle.Immatriculation.Length > 9)
            {
                return new ValidationException("Immatriculation must be between 7 and 9 characters long.");
            }

            if (vehicle.Kilometers < 0)
            {
                return new ValidationException("Kilometers must be a positive number.");
            }
 
            return null;
        }
    }
}
