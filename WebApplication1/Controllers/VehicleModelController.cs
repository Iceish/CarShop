using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly DbContext _dataContext;
    private readonly ILogger<VehicleModelController> _logger;

    private DbSet<VehicleModel> VehicleModelRepository => _dataContext.Set<VehicleModel>();

    public VehicleModelController(DbContext context, ILogger<VehicleModelController> logger)
    {
        _dataContext = context;
        _logger = logger;
    }

    /// <summary>
    /// Retreive all vehicle models.
    /// </summary>
    /// <returns>Vehicle model Array</returns>
    /// <response code="200">Vehicle model found</response>
    /// <response code="204">No vehicle model found</response>
    [HttpGet]
    public IActionResult Get()
    {
        var vehicleModels = VehicleModelRepository
            .OrderBy(x => x.Name)
            .AsEnumerable()
            .Select(x => VehicleModelFactory.ConvertToApiModel(x))
            .ToList();

        if (!vehicleModels.Any())
            return StatusCode(StatusCodes.Status204NoContent);

        return Ok(vehicleModels);
    }

    /// <summary>
    /// Retreive a vehicle model by its id.
    /// </summary>
    /// <param name="vehicleModelId"></param>
    /// <returns>Vehicle model</returns>
    /// <response code="200">Vehicle model found</response>
    /// <response code="404">Vehicle model not found</response>
    [HttpGet("{vehicleModelId}")]
    public IActionResult Get(
        [FromRoute] int vehicleModelId
        )
    {
        var vehicleModel = VehicleModelRepository
            .FirstOrDefault(x => x.Id == vehicleModelId);

        if (vehicleModel == null)
        {
            _logger.LogWarning($"No vehicle model found with id: {vehicleModelId}");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return Ok(VehicleModelFactory.ConvertToApiModel(vehicleModel));
    }

    /// <summary>
    /// Create a new vehicle model.
    /// </summary>
    /// <remarks>
    /// It will return the newly created vehicle model.
    /// 
    /// Available brands: Audi=1,BMW=2,Chevrolet=3,Toyota=4,Volkswagen=5,Ford=6,Honda=7,Hyundai=8,Jeep=9,MercedesBenz=10,Renault=11,Nissan=12,Peugeot=13,Citroen=14,Fiat=15,Other=0
    /// </remarks>
    /// <param name="vehicleModel"></param>
    /// <returns>Vehicle model</returns>
    /// <response code="200">Vehicle model created successfully</response>
    /// <response code="400">Vehicle model creation failed</response>
    [HttpPost]
    [Consumes("application/json")]
    public IActionResult Create(
        [FromBody] VehicleModelCreateApiModel vehicleModel
        )
    {
        var newVehicleModel = new VehicleModel()
        {
            Name = vehicleModel.Name,
            Brand = vehicleModel.Brand,
            MaintenanceFrequency = vehicleModel.MaintenanceFrequency
        };

        var error = new VehicleModelDomainService().Validate(newVehicleModel);
        if (error is not null)
        {
            _logger.LogWarning($"Vehicle Model validation failed: {error}");
            return StatusCode(StatusCodes.Status400BadRequest, error.Message);
        }
        VehicleModelRepository.Add(newVehicleModel);
        _dataContext.SaveChanges();

        return Ok(VehicleModelFactory.ConvertToApiModel(newVehicleModel));
    }

    /// <summary>
    /// Delete a vehicle model.
    /// </summary>
    /// <param name="vehicleModelId"></param>
    /// <returns></returns>
    /// <response code="200">Vehicle model deleted successfully</response>
    /// <response code="404">Vehicle model not found</response>
    [HttpDelete("{vehicleModelId}")]
    public IActionResult Delete(
        [FromRoute] int vehicleModelId
        )
    {
        var vehicleModel = VehicleModelRepository
            .FirstOrDefault(x => x.Id == vehicleModelId);

        if (vehicleModel == null)
        {
            _logger.LogWarning($"No vehicle found with id: {vehicleModelId}");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        VehicleModelRepository.Remove(new VehicleModel { Id = vehicleModelId });
        _dataContext.SaveChanges();

        _logger.LogInformation($"The vehicle model with id {vehicleModelId} has been deleted!");
        return Ok();
    }

}
