using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Kasi_Server.Common.Common.Swagger
{
    public static class Extensions
    {
        private static string SwaggerSectionName = "Swagger";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var svcProvider = services.BuildServiceProvider();
            var config = svcProvider.GetRequiredService<IConfiguration>();
            var swaggerOptions = config.GetOptions<SwaggerOptions>(SwaggerSectionName);
            services.AddSwaggerGen(options =>
            {
                // Resolve the temprary IApiVersionDescriptionProvider service
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                // Add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Version = description.ApiVersion.ToString(),
                        Title = swaggerOptions.Title,
                        Description = swaggerOptions.Description,
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "GPS Mobile",
                            Email = swaggerOptions.Contact,
                            Url = new Uri("https://twitter.com/spboyer"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "GPS Mobile LICX",
                            Url = new Uri("https://example.com/license"),
                        }
                    });
                }

                // Bearer accessToken authentication
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization accessToken.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };
                options.AddSecurityDefinition("jwt_auth", securityDefinition);

                // Make sure swagger UI requires a Bearer accessToken specified
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "jwt_auth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                options.AddSecurityRequirement(securityRequirements);

                options.SchemaFilter<SwaggerExcludeFilter>();

                // Add a custom filter for settint the default values
                options.OperationFilter<SwaggerDefaultValues>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var svcProvider = scope.ServiceProvider;
                var config = svcProvider.GetRequiredService<IConfiguration>();
                var swaggerOptions = config.GetOptions<SwaggerOptions>(SwaggerSectionName);
                if (!swaggerOptions.Enabled)
                {
                    return app;
                }

                var routePrefix = string.IsNullOrWhiteSpace(swaggerOptions.RoutePrefix) ? "swagger" : swaggerOptions.RoutePrefix;

                app.UseStaticFiles()
                    .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

                return app.UseSwaggerUI(c =>
                  {
                      var provider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                      // build a swagger endpoint for each discovered API version
                      foreach (var description in provider.ApiVersionDescriptions)
                      {
                          c.SwaggerEndpoint($"/{routePrefix}/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                      }

                      c.RoutePrefix = routePrefix;
                  });
            }
        }
    }
}