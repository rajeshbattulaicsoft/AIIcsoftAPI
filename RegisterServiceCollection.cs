using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Models.RequestModels;
using AIIcsoftAPI.Services;
using AIIcsoftAPI.Services.CustomerMaterialReturns;
using AIIcsoftAPI.Services.EstimatedJobCardApprovals;
using AIIcsoftAPI.Services.EstimatedJobCardScreens;
using AIIcsoftAPI.Services.IssueToRepair;
using AIIcsoftAPI.Services.JobCardClosures;
using AIIcsoftAPI.Services.JobCardCreations;
using AIIcsoftAPI.Services.JsonData;
using AIIcsoftAPI.Services.MrsApprovals;
using AIIcsoftAPI.Services.PDO;
using AIIcsoftAPI.Services.StoreInward;
using AIIcsoftAPI.Services.StoreIssue;
using AIIcsoftAPI.Validations;
using FluentValidation;

namespace AIIcsoftAPI
{
    public static class RegisterServiceCollection
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IStoreIssuesService, StoreIssuesService>();
            services.AddScoped<IStoreIssuesService, StoreIssuesService>();
            services.AddScoped<IPdosService, PdosService>();
            services.AddScoped<ICustomerMaterialReturnsService, CustomerMaterialReturnsService>();
            services.AddScoped<IJobCardCreationsService, JobCardCreationsService>();
            services.AddScoped<IJobCardClosuresService, JobCardClosuresService>();
            services.AddScoped<IEstimatedJobCardApprovalsService, EstimatedJobCardApprovalsService>();
            services.AddScoped<IMrsApprovalsService, MrsApprovalsService>();
            services.AddScoped<IEstimatedJobCardScreensService, EstimatedJobCardScreensService>();
            services.AddScoped<IStoreInwardService, StoreInwardService>();
            services.AddScoped<IIssueToRepairService, IssueToRepairService>();
            services.AddScoped<IJsonDataService, JsonDataService>();
            services.AddScoped<IValidator<JsonDataGetModel>, JsonDataGetModelValidator>();
            return services;
        }
    }
}
