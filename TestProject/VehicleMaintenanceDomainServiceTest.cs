using FluentAssertions;
using Server.Domain;

namespace TestProject;

[TestClass]
public class VehicleMaintenanceDomainServiceTest
{

    [TestMethod]
    public void ValidateEntity_WhenIsNull_ShouldNotReturnNull()
    {
        // Arrange
        VehicleMaintenance? vehicleMaintenance = null;

        var vehicleMaintenanceDomainService = new VehicleMaintenanceDomainService();

        // Act
        var result = vehicleMaintenanceDomainService.Validate(vehicleMaintenance);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenKilometersIsNegative_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleMaintenance = new VehicleMaintenance
        {
            Kilometers = -1,
            Description = "Test"
        };

        var vehicleMaintenanceDomainService = new VehicleMaintenanceDomainService();

        // Act
        var result = vehicleMaintenanceDomainService.Validate(vehicleMaintenance);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenDescriptionIsTooShort_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleMaintenance = new VehicleMaintenance
        {
            Kilometers = 1,
            Description = ""
        };

        var vehicleMaintenanceDomainService = new VehicleMaintenanceDomainService();

        // Act
        var result = vehicleMaintenanceDomainService.Validate(vehicleMaintenance);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenDescriptionIsTooLong_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleMaintenance = new VehicleMaintenance
        {
            Kilometers = 1,
            Description = new string('a', 501)
        };

        var vehicleMaintenanceDomainService = new VehicleMaintenanceDomainService();

        // Act
        var result = vehicleMaintenanceDomainService.Validate(vehicleMaintenance);

        // Assert
        result.Should().NotBeNull();
    }


}
