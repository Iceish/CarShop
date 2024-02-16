using System.ComponentModel.DataAnnotations;
using Shared;

namespace WebApplication1.Domain
{
    public class Recipe
    {
        [Range(0,int.MaxValue,
            ErrorMessage = "Attention l'ID doit être positif")]
        public int Id { get; set; }

        [Required]
        [StringLength(64,MinimumLength = 1)]
        [RegularExpression("\\w*",ErrorMessage = "Doit contenir que des lettres")]
        public string RecipeName { get; set; }

        public RecipeStatus Status { get; set; }

        public IList<Parameter> Parameters { get; set; } = new List<Parameter>();


    }
}
