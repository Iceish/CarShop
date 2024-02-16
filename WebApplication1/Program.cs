using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1;
using WebApplication1.GraphQL;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


builder.Services.AddDbContext<DbContext, ApplicationDbContext>(
    c=> c.UseSqlite("Data Source=C:\\Application.db;"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddGraphQLServer()
    .AddType<ParameterType>()
    .AddType<RecipeType>()
    .AddQueryType<GraphQLQuery>();

builder.Services.AddScoped<GraphQLQuery>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapBananaCakePop("/graphql/ui");
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapGraphQL();

app.Run();
