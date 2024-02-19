using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


builder.Services.AddDbContext<DbContext, ApplicationDbContext>(
    c =>
    {
        c.UseSqlite(
        "Data Source=" +
        Path.Combine(Directory.GetCurrentDirectory(), "Data\\CarShop.db;")
        );
        c.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
    c.IncludeXmlComments(filePath);
    c.SwaggerDoc("v1", new () {
        Title = "CarShop",
        Version = "v1",
        Description = "CarShop API made with Entity Framework to manage Vehicles.",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
