using FinTrack.Database;
using FinTrack.Database.Interfaces;
using FinTrack.Database.Migrations;
using FinTrack.Education.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FintrackDbContext>(options =>
{
    _ = options.UseNpgsql(builder.Configuration.GetConnectionString("FintrackDb"));
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddTransient<IStartupFilter, MigrationStartupFilter<FintrackDbContext>>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();

// Register Services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseCategoryService>();

var app = builder.Build();

string basePath = ""; // set to non-empty if your app is hosted under a path base

// Configure the HTTP request pipeline. Enable Swagger only in Development.
if (app.Environment.IsDevelopment())
{
    var prefix = string.IsNullOrEmpty(basePath) ? string.Empty : $"/{basePath}";

    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            var scheme = app.Environment.IsProduction() ? "https" : "http";
            swaggerDoc.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}{prefix}" }
            };
        });

        // RouteTemplate should not start with a leading slash
        c.RouteTemplate = (string.IsNullOrEmpty(basePath) ? string.Empty : $"{basePath}/") + "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(options =>
    {
        var endpoint = (string.IsNullOrEmpty(basePath) ? string.Empty : $"/{basePath}") + "/swagger/v1/swagger.json";
        options.SwaggerEndpoint(endpoint, "FinTrack API V1");
        // RoutePrefix must be relative (no leading '/').
        options.RoutePrefix = string.IsNullOrEmpty(basePath) ? "swagger" : $"{basePath}/swagger";
    });
}
app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
