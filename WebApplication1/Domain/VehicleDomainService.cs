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
            return null;
        }
    }
}
