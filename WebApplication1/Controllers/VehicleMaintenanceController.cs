using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleMaintenanceController : ControllerBase
    {
        private readonly DbContext _dataContext;
        private readonly ILogger<VehicleMaintenanceController> _logger;

        private DbSet<VehicleMaintenance> VehicleMaintenanceRepository => _dataContext.Set<VehicleMaintenance>();
        private IQueryable<VehicleMaintenance> PrepareQueryWithOptionalParameters(string? include)
        {
            var query = VehicleMaintenanceRepository.AsQueryable();
            if (include is not null)
            {
                switch (include.ToLower())
                {
                    case "vehicle":
                        query = query.Include(x => x.Vehicle);
                        break;
                    default:
                        _logger.LogWarning($"Invalid include query parameter: {include}");
                        throw new ArgumentException("Invalid include query parameter", nameof(include));
                }
            }
            return query;
        }

        public VehicleMaintenanceController(DbContext context, ILogger<VehicleMaintenanceController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(
            [FromQuery] string? include
            )
        {
            var vehicleMaintenance = PrepareQueryWithOptionalParameters(include)
                .OrderBy(x => x.Date)
                .AsEnumerable()
                .Select(VehicleMaintenanceFactory.ConvertToApiModel)
                .ToList();
            
            return Ok(vehicleMaintenance);
        }

        [HttpGet("{vehicleMaintenanceId}")]
        public IActionResult Get(
            [FromRoute] int vehicleMaintenanceId,
            [FromQuery] string? include
            )
        {
            var vehicleMaintenance = PrepareQueryWithOptionalParameters(include)
                .FirstOrDefault(x => x.Id == vehicleMaintenanceId);

            if (vehicleMaintenance == null)
            {
                _logger.LogWarning($"No vehicle maintenance found with id: {vehicleMaintenanceId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(VehicleMaintenanceFactory.ConvertToApiModel(vehicleMaintenance));
        }

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
}
