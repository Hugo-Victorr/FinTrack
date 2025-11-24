using FinTrack.Analytics.Contracts;
using FinTrack.Analytics.Services;
using FinTrack.Education.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "https://yourdomain.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
    });
});

// Services
builder.Services.AddTransient<IDashboardService, DashboardService>();

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
        options.SwaggerEndpoint(endpoint, "FinTrack Analytics API V1");
        // RoutePrefix must be relative (no leading '/').
        options.RoutePrefix = string.IsNullOrEmpty(basePath) ? "swagger" : $"{basePath}/swagger";
    });
}

// Use CORS - must be before UseAuthorization and MapControllers
app.UseCors();

app.UseAuthorization();

app.UseMiddleware<ApiGatewayUserContextMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors();

app.Run();
