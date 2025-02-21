using AIIcsoftAPI.Dto.EstimatedJobCardApprovals;
using AIIcsoftAPI.Dto.StoreIssues;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.EstimatedJobCardApprovals
{
    public interface IEstimatedJobCardApprovalsService
    {
        Task<ServiceResponse<string>> SaveEstimatedJobCardApprovalAsync(List<EstimatedJobCardApprovalsPostModel> estimatedJobCardApprovalsPostModel, string trMode);
        //Task<ServiceResponse<string>> UpdateEstimatedJobCardApprovalAsync(List<EstimatedJobCardApprovalsPutModel> estimatedJobCardApprovalsPutModel, string trMode);
        //Task<IEnumerable<GetStoreIssueResponseModel>> GetEstimatedJobCardApprovalAsync(int departmentId);
        Task<ServiceResponse<string>> GetEstimatedJobCardApprovalAsync(int departmentId);
    }
}
