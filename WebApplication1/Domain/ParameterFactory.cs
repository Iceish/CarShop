using Shared.ApiModels;

namespace WebApplication1.Domain
{
    public class ParameterFactory
    {
        public static ParameterModel? ConvertToApiModel(Parameter? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new ParameterModel
            {
                ID = dbEntity.ID,
                Name = dbEntity.Name,
                RecipeId = dbEntity.RecipeId,
                Value = dbEntity.Value,
                UnitOfMeasurement = dbEntity.UnitOfMeasurement,
            };
        }
    }
}
