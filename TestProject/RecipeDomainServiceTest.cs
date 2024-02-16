using FluentAssertions;
using Server.Domain;
using WebApplication1.Domain;

namespace TestProject
{
    [TestClass]
    public class RecipeDomainServiceTest
    {
        [TestMethod]
        public void ValidateRecipe_EmptyName_ReturnError()
        {
            // Arrange
            var newRecipe = new Recipe { };

            // Act
            var result=new RecipeDomainService().ValidateRecipe(newRecipe);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Contain("recipe name");
        }
    }
}