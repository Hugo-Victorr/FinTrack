using Amazon;
using Amazon.S3;
using FinTrack.Database;
using FinTrack.Database.Interfaces;
using FinTrack.Database.Migrations;
using FinTrack.Education.Middlewares;
using FinTrack.Education.Services;
using FinTrack.Infrastructure.DTO;
using FinTrack.Infrastructure.Interfaces;
using FinTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()   // TODO: disable "allow any" in production
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register health check
builder.Services.AddHealthChecks();

// Register database context
builder.Services.AddDbContext<FintrackDbContext>(options =>
{
    _ = options.UseNpgsql(builder.Configuration.GetConnectionString("FintrackDb"));
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddTransient<IStartupFilter, MigrationStartupFilter<FintrackDbContext>>();

// Register strongly typed settings
builder.Services.Configure<AwsStorageSettings>(builder.Configuration.GetSection("AWS:Storage"));

// Register clients
builder.Services.AddSingleton<IAmazonS3>(_ =>
    new AmazonS3Client(RegionEndpoint.GetBySystemName(config["AWS:Region"])));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Repositories
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseModuleRepository, CourseModuleRepository>();
builder.Services.AddScoped<ICourseCategoryRepository, CourseCategoryRepository>();
builder.Services.AddScoped<ICourseLearningPlanRepository, CourseLearningPlanRepository>();
builder.Services.AddScoped<ICourseLessonProgressRepository, CourseLessonProgressRepository>();
builder.Services.AddScoped<ICourseLessonRepository, CourseLessonRepository>();
builder.Services.AddScoped<ICourseTagAssigmentRepository, CourseTagAssigmentRepository>();
builder.Services.AddScoped<ICourseProgressRepository, CourseProgressRepository>();
builder.Services.AddScoped<ICourseTagRepository, CourseTagRepository>();

// Register Services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseCategoryService>();
builder.Services.AddScoped<ProgressService>();
builder.Services.AddScoped<IStorageService, S3StorageService>();

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


app.UseMiddleware<ApiGatewayUserContextMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
