using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleMaintenanceController : ControllerBase
{
    private readonly DbContext _dataContext;
    private readonly ILogger<VehicleMaintenanceController> _logger;

    private DbSet<VehicleMaintenance> VehicleMaintenanceRepository => _dataContext.Set<VehicleMaintenance>();

    public VehicleMaintenanceController(DbContext context, ILogger<VehicleMaintenanceController> logger)
    {
        _dataContext = context;
        _logger = logger;
    }

    /// <summary>
    /// Retreive all vehicle maintenances.
    /// </summary>
    /// <returns>Vehicle maintenances Array</returns>
    /// <response code="200">Vehicle maintenances found</response>
    /// <response code="204">No vehicle maintenances found</response>
    [HttpGet]
    public IActionResult Get()
    {
        var vehicleMaintenance = VehicleMaintenanceRepository
            .OrderBy(x => x.Date)
            .AsEnumerable()
            .Select(VehicleMaintenanceFactory.ConvertToApiModel)
            .ToList();
        
        return Ok(vehicleMaintenance);
    }

    /// <summary>
    /// Retreive a vehicle maintenance by its id.
    /// </summary>
    /// <param name="vehicleMaintenanceId"></param>
    /// <returns>Vehicle maintenance</returns>
    /// <response code="200">Vehicle maintenance found</response>
    /// <response code="404">Vehicle maintenance not found</response>
    [HttpGet("{vehicleMaintenanceId}")]
    public IActionResult Get(
        [FromRoute] int vehicleMaintenanceId
        )
    {
        var vehicleMaintenance = VehicleMaintenanceRepository
            .FirstOrDefault(x => x.Id == vehicleMaintenanceId);

        if (vehicleMaintenance == null)
        {
            _logger.LogWarning($"No vehicle maintenance found with id: {vehicleMaintenanceId}");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return Ok(VehicleMaintenanceFactory.ConvertToApiModel(vehicleMaintenance));
    }

    /// <summary>
    /// Create a new vehicle maintenance.
    /// </summary>
    /// <remarks>
    /// It will return the newly created vehicle maintenance.
    /// </remarks>
    /// <param name="vehicleMaintenance"></param>
    /// <returns>Vehicle maintenance</returns>
    /// <response code="200">Vehicle maintenance created successfully</response>
    /// <response code="400">Vehicle maintenance creation failed</response>
    [HttpPost]
    [Consumes("application/json")]
    public IActionResult Create(
        [FromBody] VehicleMaintenanceCreateApiModel vehicleMaintenance
        )
    {
        var vehicle = _dataContext.Set<Vehicle>().FirstOrDefault(x => x.Id == vehicleMaintenance.VehicleId);
        int currentKilometers = (vehicle is not null) ? vehicle.Kilometers : 0;


        var newVehicleMaintenance = new VehicleMaintenance()
        {
            Date = DateTime.Now,
            Kilometers = currentKilometers,
            Description = vehicleMaintenance.Description,
            VehicleId = vehicleMaintenance.VehicleId
        };

        
        
        var error = new VehicleMaintenanceDomainService().Validate(newVehicleMaintenance);
        if (error is not null)
        {
            _logger.LogWarning($"Vehicle maintenance validation failed: {error}");
            return StatusCode(StatusCodes.Status400BadRequest, error);
        }
        VehicleMaintenanceRepository.Add(newVehicleMaintenance);
        _dataContext.SaveChanges();

        return Ok(VehicleMaintenanceFactory.ConvertToApiModel(newVehicleMaintenance));
    }

    /// <summary>
    /// Delete a vehicle maintenance.
    /// </summary>
    /// <param name="vehicleMaintenanceId"></param>
    /// <returns></returns>
    /// <response code="200">Vehicle maintenance deleted successfully</response>
    /// <response code="404">Vehicle maintenance not found</response>
    [HttpDelete("{vehicleMaintenanceId}")]
    public IActionResult Delete(
        [FromRoute] int vehicleMaintenanceId
        )
    {
        var vehicleMaintenance = VehicleMaintenanceRepository
            .FirstOrDefault(x => x.Id == vehicleMaintenanceId);

        if (vehicleMaintenance == null)
        {
            _logger.LogWarning($"No vehicle maintenance found with id: {vehicleMaintenanceId}");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        VehicleMaintenanceRepository.Remove(vehicleMaintenance);
        _dataContext.SaveChanges();

        _logger.LogInformation($"The vehicle maintenance with id {vehicleMaintenanceId} has been deleted!");
        return Ok();
    }

}
