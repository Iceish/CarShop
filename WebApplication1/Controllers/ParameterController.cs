using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.ApiModels;
using WebApplication1.Domain;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController : ControllerBase
    {
        private readonly DbContext _dataContext;
        private readonly ILogger<ParameterController> _logger;

        public ParameterController(DbContext context,
            ILogger<ParameterController> logger)
        {
            _dataContext = context;
            _logger = logger;
        }

        [HttpPut]
        public void EditParameter([FromBody]ParameterModel parameterToEdit)
        {
            var parameterRepository = _dataContext.Set<Parameter>();

            var dbParameter = parameterRepository
                .FirstOrDefault(x => x.ID == parameterToEdit.ID);

            if (dbParameter == null)
            {
                _logger.LogWarning($"No parameter found with Id: {parameterToEdit.ID}");
                return ;
            }

            dbParameter.Name = parameterToEdit.Name;
            dbParameter.Value = parameterToEdit.Value;
            dbParameter.UnitOfMeasurement = parameterToEdit.UnitOfMeasurement;

            parameterRepository.Update(dbParameter);

            _dataContext.SaveChanges();
            _logger.LogInformation($"The parameter with Id: {dbParameter.ID} and name: {dbParameter.Name} has been edited");
        }

        [HttpPost]
        public void CreateParameter(string parameterName,int recipeId,decimal value,string unitOfMeasurement)
        {
            var newParameter = new Parameter()
            {
                Name = parameterName,
                RecipeId = recipeId,
                Value = value,
                UnitOfMeasurement = unitOfMeasurement
            };

            var parameterepository = _dataContext.Set<Parameter>();

            parameterepository.Add(newParameter);

            _dataContext.SaveChanges();
        }


        [HttpGet]
        public IList<ParameterModel> GetAllParameters()
        {
            return _dataContext.Set<Parameter>()
                .AsEnumerable()
                .Select(x=>ParameterFactory.ConvertToApiModel(x))
                .ToList();
        }

        [HttpGet("GetByRecipe")]
        public IList<ParameterModel> GetRecipeParameters(int recipeId)
        {
            return _dataContext.Set<Parameter>()
                .Where(x => x.RecipeId == recipeId)
                .AsEnumerable()
                .Select(x=>ParameterFactory.ConvertToApiModel(x))
                .ToList();
        }
    }
}
