using Microsoft.EntityFrameworkCore;
using StoringData.Ctx;
using StoringData.DevicePerformanceServices;
using StoringData.DevicePerformanceServices.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DevicePerformanceDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod()));


builder.Services.AddScoped<DevicePerformanceDbContext>();
builder.Services.AddScoped<IDevicePerformanceService, DevicePerformanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapPost("/deviceperformance", async (CreateDevicePerformance model) =>
{
    using (var scope = app.Services.CreateScope())
    {
        IDevicePerformanceService performanceService = scope.ServiceProvider.GetRequiredService<IDevicePerformanceService>();
        var record = await performanceService.CreateDevicePerformance(model);
        return record; 
    }
});

app.Run();
