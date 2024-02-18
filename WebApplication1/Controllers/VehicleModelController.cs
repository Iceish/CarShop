using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers
{
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


        [HttpGet]
        public IActionResult Get()
        {
            var vehicleModels = VehicleModelRepository
                .OrderBy(x => x.Name)
                .AsEnumerable()
                .Select(x => VehicleModelFactory.ConvertToApiModel(x))
                .ToList();

            return Ok(vehicleModels);
        }

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
}
