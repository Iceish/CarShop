using Shared.ApiModels;

namespace WebApplication1.Domain
{
    public static class RecipeFactory
    {
        public static RecipeModel? ConvertToApiModel(Recipe? dbEntity)
        {
            if (dbEntity == null)
                return null;

            return new RecipeModel
            {
                Id = dbEntity.Id,
                RecipeName = dbEntity.RecipeName,
                Status = dbEntity.Status,
                Parameters = dbEntity.Parameters
                    .Select(x => ParameterFactory.ConvertToApiModel(x))
                    .ToList()

            };
        }

        public static Recipe DuplicateRecipe(Recipe recipeToDuplicate, string newRecipeName)
        {
            return new Recipe
            {
                RecipeName = newRecipeName,
                Status = recipeToDuplicate.Status,
                Parameters = recipeToDuplicate.Parameters
                    .Select(param => new Parameter
                    {
                        Value = param.Value,
                        Name = param.Name,
                        UnitOfMeasurement = param.UnitOfMeasurement
                    })
                    .ToList(),
            };
        }
    }
}
