using TiduPizza.Data;
using TiduPizza.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PizzaService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddSqlite<PizzaContext>("Data Source=TiduPizza.db");
builder.Services.AddSqlite<PromotionsContext>("Data Source=Promotions/Promotions.db");
builder.Services.AddScoped<PizzaService>();

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
        //options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        //options.JsonSerializerOptions.WriteIndented = true;

        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("AllowLocalhost");
app.MapControllers();

app.CreateDbIfNotExists();

app.MapGet("/", () => @"Tidu Pizza management API. Navigate to /swagger to open the Swagger test UI.");

app.Run();
