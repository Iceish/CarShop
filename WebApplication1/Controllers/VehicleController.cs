using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain;
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

        public VehicleController(DbContext context,ILogger<VehicleController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }


    }
}
