using System.ComponentModel.DataAnnotations;
using WebApplication1.Domain;

namespace Server.Domain
{
    public class RecipeDomainService
    {
        public ValidationException? ValidateRecipe(Recipe recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.RecipeName))
                return new ValidationException("The recipe name is empty");

            return null;
        }
    }
}
