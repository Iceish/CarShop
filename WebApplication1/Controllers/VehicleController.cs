using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly DbContext _dataContext;
        private readonly ILogger<VehicleController> _logger;

        private DbSet<Vehicle> VehicleRepository => _dataContext.Set<Vehicle>();
        private IQueryable<Vehicle> PrepareQueryWithOptionalParameters(string? include)
        {
            var query = VehicleRepository.AsQueryable();
            if (include is not null)
            {
                switch (include.ToLower())
                {
                    case "vehiclemodel":
                        query = query.Include(x => x.VehicleModel);
                        break;
                    default:
                        _logger.LogWarning($"Invalid include query parameter: {include}");
                        throw new ArgumentException("Invalid include query parameter", nameof(include));
                }
            }
            return query;
        }

        public VehicleController(DbContext context, ILogger<VehicleController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(
            [FromQuery] string? include
            )
        {
            var vehicles = PrepareQueryWithOptionalParameters(include)
                .OrderBy(x => x.Immatriculation)
                .AsEnumerable()
                .Select(VehicleFactory.ConvertToApiModel)
                .ToList();
            
            return Ok(vehicles);
        }

        [HttpGet("{vehicleId}")]
        public IActionResult Get(
            [FromRoute] int vehicleId,
            [FromQuery] string? include
            )
        {
            var vehicle = PrepareQueryWithOptionalParameters(include)
                .FirstOrDefault(x => x.Id == vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning($"No vehicle found with id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(VehicleFactory.ConvertToApiModel(vehicle));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Create(
            [FromBody] VehicleApiModel vehicle
            )
        {
            var newVehicle = new Vehicle()
            {
                Immatriculation = vehicle.Immatriculation,
                Year = vehicle.Year,
                Kilometers = vehicle.Kilometers,
                VehicleModelId = vehicle.VehicleModelId
            };
            
            var error = new VehicleDomainService().Validate(newVehicle);
            if (error is not null)
            {
                _logger.LogWarning($"Vehicle validation failed: {error}");
                return StatusCode(StatusCodes.Status400BadRequest, error);
            }
            VehicleRepository.Add(newVehicle);
            _dataContext.SaveChanges();

            return Ok(VehicleFactory.ConvertToApiModel(newVehicle));
        }

        [HttpDelete("{vehicleId}")]
        public IActionResult Delete(
            [FromRoute] int vehicleId
            )
        {
            var vehicle = VehicleRepository
                .FirstOrDefault(x => x.Id == vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning($"No vehicle found with id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            VehicleRepository.Remove(vehicle);
            _dataContext.SaveChanges();

            _logger.LogInformation($"The vehicle model with id {vehicleId} has been deleted!");
            return Ok();
        }

    }
}
