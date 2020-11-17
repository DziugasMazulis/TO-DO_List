using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TO_DO_List.Security;

namespace TO_DO_List.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwag(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo-List", Version = "v1" });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                c.OperationFilter<AuthOperationFilter>();
            });

            return services;
        }

        public static IApplicationBuilder UseSwag(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
                c.OAuthClientId("cleint-id");
                c.OAuthClientSecret("client-secret");
                c.OAuthRealm("client-realm");
                c.OAuthAppName("OAuth-app");
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            return app;
        }
    }
}
