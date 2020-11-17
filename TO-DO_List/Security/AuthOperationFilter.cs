using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using TO_DO_List.Data;

namespace TO_DO_List.Security
{
    public class AuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var isAuthorized = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                              context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!isAuthorized) return;

            operation.Responses.TryAdd(Constants.Int401, new OpenApiResponse { Description = Constants.Unauthorized });
            operation.Responses.TryAdd(Constants.Int403, new OpenApiResponse { Description = Constants.Forbidden });

            var jwtbearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = Constants.Bearer }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
               {
                   new OpenApiSecurityRequirement
                   {
                       [ jwtbearerScheme ] = new string [] { }
                   }
               };
        }
    }
}
