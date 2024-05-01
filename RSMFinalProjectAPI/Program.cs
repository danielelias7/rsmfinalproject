using Microsoft.EntityFrameworkCore;
using RSMFinalProjectAPI.Application.Services;
using RSMFinalProjectAPI.Domain.Interfaces;
using RSMFinalProjectAPI.Infrastructure;
using RSMFinalProjectAPI.Infrastructure.Repositories;
using RSMFinalProjectAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AdvWorksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        opt => opt.MigrationsAssembly(typeof(AdvWorksDbContext).Assembly.FullName));
});

builder.Services.AddTransient<ISalesReportRepository, SalesReportRepository>();
builder.Services.AddTransient<ISalesReportService, SalesReportService>();

builder.Services.AddTransient<ISalesPerformanceRepository, SalesPerformanceRepository>();
builder.Services.AddTransient<ISalesPerformanceService, SalesPerformanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseMiddleware<BasicAuthMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
