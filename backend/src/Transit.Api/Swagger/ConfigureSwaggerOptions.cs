using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Transit.Api.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions opts)
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            opts.SwaggerDoc(desc.GroupName, new OpenApiInfo
            {
                Title = "Transit API",
                Version = desc.ApiVersion.ToString(),
                Description = "Backend systemu komunikacji miejskiej — rozkłady, wyszukiwanie połączeń, panel zarządzania."
            });
        }

        opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Wpisz token JWT (bez prefiksu 'Bearer ')."
        });

        opts.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                []
            }
        });

        var xmlFile = $"{typeof(ConfigureSwaggerOptions).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            opts.IncludeXmlComments(xmlPath);
    }
}
