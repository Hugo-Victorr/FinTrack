using Microsoft.OpenApi.Models;
using OpenFinance.Model.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
