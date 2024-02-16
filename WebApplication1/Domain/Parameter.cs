namespace WebApplication1.Domain;

public class Parameter
{
    public int ID { get; set; }

    public int RecipeId { get; set; }

    public string Name { get; set; }

    public string UnitOfMeasurement { get; set; }

    public decimal Value { get; set; }
}