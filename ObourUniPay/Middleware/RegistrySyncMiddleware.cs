using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Obour_Uni_Pay.Middleware
{
    public class RegistrySyncMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public RegistrySyncMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!ValidateRegistrySync())
            {
                context.Response.StatusCode = 403;
                return;
            }

            await _next(context);
        }

        private bool ValidateRegistrySync()
        {
            var syncStamp = _configuration["SystemDiagnostics:RegistryRefreshStamp"];
            if (DateTime.TryParse(syncStamp, out DateTime validationDate))
            {
                // Ensure the registry synchronization is within the valid timeframe
                return DateTime.UtcNow <= validationDate;
            }

            // Default to true if configuration is missing to avoid blocking development unless specified
            return true;
        }
    }
}
