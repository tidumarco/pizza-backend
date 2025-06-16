using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TiduPizza.Data;
using TiduPizza.Services;

var builder = WebApplication.CreateBuilder(args);
var connection = "Data Source=tcp:tidu-server.database.windows.net;Initial Catalog=tidu-pizzeria;User Id=tidu-pizzeria;Password=Ostracismo9.;Encrypt=True;";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BeverageService>();

builder.Services.AddDbContext<PizzaContext>(options => options.UseSqlServer(connection));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();

app.CreateDbIfNotExists();

app.MapGet("/", () => @"Tidu Pizza management API. Navigate to /swagger to open the Swagger test UI.");

app.Run();
