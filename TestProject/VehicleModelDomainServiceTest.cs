using FluentAssertions;
using Server.Domain;

namespace TestProject;

[TestClass]
public class VehicleModelDomainServiceTest
{
    [TestMethod]
    public void ValidateEntity_WhenEntityIsValid_ShouldReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Name = "Test",
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = 1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenNameIsNull_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = 1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenNameIsTooShort_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Name = "",
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = 1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenNameIsTooLong_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Name = "ThisIsAVeryLongNameThatExceedsTheMaximumLengthOfSixtyFourCharactersAndShouldThrowAnException",
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = 1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenMaintenanceFrequencyIsNegative_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Name = "Test",
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = -1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenEntityIsNull_ShouldNotReturnNull()
    {
        // Arrange
        VehicleModel? vehicleModel = null;

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntiy_WhenMaintenanceFrequencyIsNegative_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleModel = new VehicleModel
        {
            Id = 1,
            Name = "Test",
            Brand = Shared.Enums.VehicleBrand.Audi,
            MaintenanceFrequency = -1
        };

        var vehicleModelDomainService = new VehicleModelDomainService();

        // Act
        var result = vehicleModelDomainService.Validate(vehicleModel);

        // Assert
        result.Should().NotBeNull();
    }

}
