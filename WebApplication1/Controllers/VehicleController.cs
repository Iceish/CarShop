using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using Shared.Enums;
using System.Linq;
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
        public VehicleController(DbContext context, ILogger<VehicleController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        /// <summary>
        /// Retreive all vehicles.
        /// </summary>
        /// <returns>Vehicle Array</returns>
        /// <response code="200">Vehicles found</response>
        /// <response code="204">No vehicles found</response>
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

            if (!vehicles.Any())
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(vehicles);
        }

        /// <summary>
        /// Retreive all vehicles that are late for maintenance.
        /// </summary>
        /// <returns>Vehicle Array</returns>
        /// <response code="200">Vehicles found</response>
        /// <response code="204">No vehicles found</response>
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

            if (!vehicles.Any())
                return StatusCode(StatusCodes.Status204NoContent);

            return Ok(vehicles);
        }

        /// <summary>
        /// Retreive a vehicle by its id.
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns>Vehicle</returns>
        /// <response code="200">Vehicle found</response>
        /// <response code="404">Vehicle not found</response>
        [HttpGet("{vehicleId}")]
        public IActionResult Get(
            [FromRoute] int vehicleId
            )
        {
            var vehicle = VehicleRepository
                .Include(x => x.VehicleModel)
                .Include(x => x.VehicleMaintenances)
                .FirstOrDefault(x => x.Id == vehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning($"No vehicle found with id: {vehicleId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(VehicleFactory.ConvertToApiModel(vehicle));
        }

        /// <summary>
        /// Create a new vehicle.
        /// </summary>
        /// <remarks>
        /// It will return the newly created vehicle with all it's relations.
        /// 
        /// Available FuelType: Gasoline=1,Diesel=2,Ethanol=3,Flex=4,Electric=5,Hybrid=6,Hydrogen=7,NaturalGas=8,Propane=9,Other=0
        /// </remarks>
        /// <param name="vehicle"></param>
        /// <returns>Vehicle</returns>
        /// <response code="200">Vehicle created successfully</response>
        /// <response code="400">Vehicle creation failed</response>
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

        /// <summary>
        /// Update a vehicle.
        /// </summary>
        /// <remarks>
        /// It will return vehicle updated with all it's relations.
        /// 
        /// Available FuelType: Gasoline=1,Diesel=2,Ethanol=3,Flex=4,Electric=5,Hybrid=6,Hydrogen=7,NaturalGas=8,Propane=9,Other=0
        /// </remarks>
        /// <param name="vehicleId"></param>
        /// <param name="vehicleUpdateModel"></param>
        /// <returns>Vehicle</returns>
        /// <response code="200">Vehicle updated successfully</response>
        /// <response code="404">Vehicle not found</response>
        /// <response code="400">Vehicle update failed</response>
        [HttpPut("{vehicleId}")]
        [Consumes("application/json")]
        public IActionResult Update(
            [FromRoute] int vehicleId,
            [FromBody] VehicleCreateApiModel vehicleUpdateModel)
        {
            var existingVehicle = VehicleRepository.FirstOrDefault(x => x.Id == vehicleId);
            if (existingVehicle == null)
            {
                _logger.LogWarning($"No vehicle found with id: {vehicleId}");
                return NotFound("Vehicle not found.");
            }

            existingVehicle.Immatriculation = vehicleUpdateModel.Immatriculation;
            existingVehicle.Year = vehicleUpdateModel.Year;
            existingVehicle.Kilometers = vehicleUpdateModel.Kilometers;
            existingVehicle.FuelType = (VehicleFuelType)vehicleUpdateModel.FuelType;
            existingVehicle.VehicleModelId = vehicleUpdateModel.VehicleModelId;

            var error = new VehicleDomainService().Validate(existingVehicle);
            if (error is not null)
            {
                _logger.LogWarning($"Vehicle validation failed: {error}");
                return StatusCode(StatusCodes.Status400BadRequest, error);
            }

            _dataContext.SaveChanges();

            // Load the related data
            _dataContext.Entry(existingVehicle)
                        .Reference(v => v.VehicleModel)
                        .Load();
            _dataContext.Entry(existingVehicle)
                        .Collection(v => v.VehicleMaintenances)
                        .Load();

            return Ok(VehicleFactory.ConvertToApiModel(existingVehicle));
        }

        /// <summary>
        /// Delete a vehicle.
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        /// <response code="200">Vehicle deleted successfully</response>
        /// <response code="404">Vehicle not found</response>
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
