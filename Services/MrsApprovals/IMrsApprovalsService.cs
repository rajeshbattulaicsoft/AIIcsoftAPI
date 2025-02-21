using AIIcsoftAPI.Dto.MRSApprovals;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.MrsApprovals
{
    public interface IMrsApprovalsService
    {
        Task<ServiceResponse<string>> SaveMrsApprovalAsync(List<MrsApprovalsPostModel> mrsApprovalsPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateMrsApprovalAsync(List<MrsApprovalsPutModel> mrsApprovalsPutModel, string trMode);
        public Task<IEnumerable<GetMrsApprovalResponseModel>> GetMrsApprovalAsync(MrsApprovalsGetModel request);

        public Task<ServiceResponse<string>> GetMRSApprovalGridFill(int deptid, string approvaldate,int locationid);
    }
}
