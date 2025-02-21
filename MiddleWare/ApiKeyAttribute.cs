using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
namespace AIIcsoftAPI.MiddleWare
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        //private readonly IConfiguration _configuration;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            //_configuration = configuration;
            //var AppSettings = _configuration.GetSection("ProductionApi")
            //var apiKey = "jk4l0B3ceRk7QuFqa0UlxtI7o6WsU02ZH3LoAQK3n89j"; // configuration["AppSettings:ApiKey"]; // IFSE
            var apiKey = "aKAetRirTY9@yIhniDd#ev%oEbhV16a7va"; // configuration["AppSettings:ApiKey"]; // SMPL

            if (!context.HttpContext.Request.Headers.TryGetValue("API-KEY", out var extractedApiKey) || extractedApiKey != apiKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }

}
