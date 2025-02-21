using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AIIcsoftAPI.MiddleWare
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            //_apiKey = "jk4l0B3ceRk7QuFqa0UlxtI7o6WsU02ZH3LoAQK3n89j"; //configuration["ApiSettings:ApiKey"]; // IFSE
            _apiKey = "aKAetRirTY9@yIhniDd#ev%oEbhV16a7va"; //configuration["ApiSettings:ApiKey"]; //SMPL
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("API-KEY", out var extractedApiKey) || extractedApiKey != _apiKey)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid API Key");
                return;
            }

            await _next(context);
        }
    }


}
