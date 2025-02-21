using AIIcsoftAPI.Dto.EstimatedJobCardScreens;
using AIIcsoftAPI.Dto.MRSApprovals;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.EstimatedJobCardScreens
{
    public interface IEstimatedJobCardScreensService
    {
        Task<ServiceResponse<string>> SaveEstimatedJobCardScreenAsync(List<EstimatedJobCardScreensPostModel> estimatedJobCardScreensPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateEstimatedJobCardScreenAsync(List<EstimatedJobCardScreensPutModel> estimatedJobCardScreensPutModel, string trMode);
        Task<IEnumerable<GetStoreIssueResponseModel>> GetEstimatedJobCardScreenAsync();
    }
}
