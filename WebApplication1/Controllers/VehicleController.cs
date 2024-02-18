using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using Shared.Enums;
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
        //private IQueryable<Vehicle> PrepareQueryWithOptionalParameters(string? include)
        //{
        //    var query = VehicleRepository.AsQueryable();
        //    if (include is not null)
        //    {
        //        switch (include.ToLower())
        //        {
        //            case "vehiclemodel":
        //                query = query.Include(x => x.VehicleModel);
        //                break;
        //            case "maintenances":
        //                query = query.Include(x => x.VehicleMaintenances);
        //                break;
        //            default:
        //                _logger.LogWarning($"Invalid include query parameter: {include}");
        //                throw new ArgumentException("Invalid include query parameter", nameof(include));
        //        }
        //    }
        //    return query;
        //}

        public VehicleController(DbContext context, ILogger<VehicleController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vehicles = VehicleRepository
                .Include(x => x.VehicleModel)
                .Include(x => x.VehicleMaintenances)
                .OrderBy(x => x.Immatriculation)
                .AsEnumerable()
                .Select(VehicleFactory.ConvertToApiModel)
                .ToList();
            
            return Ok(vehicles);
        }

        [HttpGet("late")]
        public IActionResult GetLateMaintenance()
        {
            var vehicles = VehicleRepository
                .Include(x => x.VehicleModel)
                .Include(x => x.VehicleMaintenances)
                .OrderBy(x => x.Immatriculation)
                .AsEnumerable()
                .Select(VehicleFactory.ConvertToApiModel)
                .Where(x => x.NextMaintenanceDelta <= 0)
                .ToList();

            return Ok(vehicles);
        }

        [HttpGet("{vehicleId}")]
        public IActionResult Get(
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

            return Ok(VehicleFactory.ConvertToApiModel(vehicle));
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Create(
            [FromBody] VehicleCreateApiModel vehicle
            )
        {
            var newVehicle = new Vehicle()
            {
                Immatriculation = vehicle.Immatriculation,
                Year = vehicle.Year,
                Kilometers = vehicle.Kilometers,
                FuelType = (VehicleFuelType)vehicle.FuelType,
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

            // Load the related data
            _dataContext.Entry(newVehicle)
                .Reference(v => v.VehicleModel)
                .Load();
            _dataContext.Entry(newVehicle)
                        .Collection(v => v.VehicleMaintenances)
                        .Load();

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

            _logger.LogInformation($"The vehicle with id {vehicleId} has been deleted!");
            return Ok();
        }

    }
}
