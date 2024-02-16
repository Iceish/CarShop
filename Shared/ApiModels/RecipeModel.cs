using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApiModels
{
    public class RecipeModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(64)]
        public string RecipeName { get; set; }

        public RecipeStatus Status { get; set; }

        public IList<ParameterModel> Parameters { get; set; } = new List<ParameterModel>();
    }
}
