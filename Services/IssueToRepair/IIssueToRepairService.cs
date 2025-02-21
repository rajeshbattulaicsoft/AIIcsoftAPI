using AIIcsoftAPI.Dto.IssueToRepair;
using AIIcsoftAPI.Dto.StoreIssues;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.IssueToRepair
{
    public interface IIssueToRepairService
    {
        Task<ServiceResponse<string>> SaveIssueToRepairAsync(List<IssueToRepairPostModel> issueToRepairPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateIssueToRepairAsync(List<IssueToRepairPutModel> issueToRepairPutModel, string trMode);
        Task<IEnumerable<GetStoreIssueResponseModel>> GetIssueToRepairAsync(IssueToRepairGetModel request);

        public Task<ServiceResponse<string>> IssueToRepairGridFill(int locationid, int srno, string srdate);

        public Task<ServiceResponse<string>> StoreIsueSerachByDepIdAndDate(int locationid, int deptid, string srdate);
        public  Task<ServiceResponse<string>> GetJobCardDropdwonIssueToRepair(int locationid);
         public  Task<ServiceResponse<string>> IssueToRepairByJobcardNo(string RefNo);
         public  Task<ServiceResponse<string>> SearchByIssueRefNo(string RefNo);
          public  Task<ServiceResponse<string>> SerialNoListForIssueToRepair(int rawmatid, int warehouseid);
    }
}
