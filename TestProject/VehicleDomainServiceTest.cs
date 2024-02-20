using FluentAssertions;
using Server.Domain;

namespace TestProject;

[TestClass]
public class VehicleDomainServiceTest
{

    [TestMethod]
    public void ValidateEntity_WhenIsNull_ShouldNotReturnNull()
    {
        // Arrange
        var vehicleDomainService = new VehicleDomainService();

        // Act
        var result = vehicleDomainService.Validate(null);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenEntityIsValid_ShouldReturnNull()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = 1,
            Immatriculation = "1234567",
            Year = 2020,
            Kilometers = 0,
            VehicleModelId = 1
        };

        var vehicleDomainService = new VehicleDomainService();

        // Act
        var result = vehicleDomainService.Validate(vehicle);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenImmatriculationIsTooShort_ShouldNotReturnNull()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = 1,
            Immatriculation = "123456",
            Year = 2020,
            Kilometers = 0,
            VehicleModelId = 1
        };

        var vehicleDomainService = new VehicleDomainService();

        // Act
        var result = vehicleDomainService.Validate(vehicle);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenYearIsNegative_ShouldNotReturnNull()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = 1,
            Immatriculation = "1234567",
            Year = -2020,
            Kilometers = 0,
            VehicleModelId = 1
        };

        var vehicleDomainService = new VehicleDomainService();

        // Act
        var result = vehicleDomainService.Validate(vehicle);

        // Assert
        result.Should().NotBeNull();
    }

    [TestMethod]
    public void ValidateEntity_WhenKilometersIsNegative_ShouldNotReturnNull()
    {
        // Arrange
        var vehicle = new Vehicle
        {
            Id = 1,
            Immatriculation = "1234567",
            Year = 2020,
            Kilometers = -1,
            VehicleModelId = 1
        };

        var vehicleDomainService = new VehicleDomainService();

        // Act
        var result = vehicleDomainService.Validate(vehicle);

        // Assert
        result.Should().NotBeNull();
    }


}
