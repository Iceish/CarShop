﻿using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain;
public class VehicleModelDomainService
{
    public ValidationException? Validate(VehicleModel vehicleModel)
    {

        if (string.IsNullOrWhiteSpace(vehicleModel.Name))
            return new ValidationException("The Vehicle model name must not be null.");

        if (vehicleModel.Name.Length < 1 || vehicleModel.Name.Length > 64)
            return new ValidationException("The Vehicle model name must be between 1 and 64 characters.");

        if (vehicleModel.Brand == null)
            return new ValidationException("The Vehicle model brand must not be null.");

        if (vehicleModel.MaintenanceFrequency < 0)
            return new ValidationException("The Vehicle model maintenance frequency must be positive.");

        return null;
    }
}
