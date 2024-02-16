using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using WebApplication1.Domain;

namespace TestProject
{
    [TestClass]
    public class RecipeTest
    {
        [TestMethod]
        public void ValidateRecipe_EmptyName_ReturnError()
        {// Arrange
            var newRecipe = new Recipe { };

            // Act
            var result=ValidateModel(newRecipe);

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
            result[0].ErrorMessage.Should().Contain("RecipeName");
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
