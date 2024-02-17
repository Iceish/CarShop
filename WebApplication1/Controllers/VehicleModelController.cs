using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
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

        public VehicleModelController(DbContext context,ILogger<VehicleModelController> logger)
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
        public IActionResult Get(int vehicleModelId)
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
        public IActionResult Create(string name, string brand, int maintenanceFrequency)
        {

            var newVehicleModel = new VehicleModel()
            {
                Name = name,
                Brand = brand,
                MaintenanceFrequency = maintenanceFrequency
            };

            // Backend validation
            var error = new VehicleModelDomainService().Validate(newVehicleModel);
            if (error is not null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, error.Message);
            }

            VehicleModelRepository.Add(newVehicleModel);

            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpPut("Rename/{vehicleModelId}")]
        public IActionResult GetFull(int vehicleModelId, string newName)
        {
            var vehicleModelToRename = VehicleModelRepository.FirstOrDefault(x => x.Id == vehicleModelId);

            if (vehicleModelToRename == null)
            {
                _logger.LogWarning($"No vehicle model found with id: {vehicleModelId}");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            vehicleModelToRename.Name = newName;

            VehicleModelRepository.Update(vehicleModelToRename);

            _dataContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{vehicleModelId}")]
        public void Delete(int vehicleModelId)
        {
            _dataContext.Set<VehicleModel>()
                .Remove(new VehicleModel { Id = vehicleModelId });

            _dataContext.SaveChanges();

            _logger.LogInformation($"The vehicle model with id {vehicleModelId} has been deleted!");
        }

    }
}
