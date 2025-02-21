using AIIcsoftAPI.Dto.StoreIssues;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.StoreIssue
{
    public interface IStoreIssuesService
    {
        Task<ServiceResponse<string>> SaveStoreIssueAsync(List<StoreIssuesPostModel> storeIssuesPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateStoreIssueAsync(List<StoreIssuesPutModel> storeIssuesPutModel, string trMode);
        Task<IEnumerable<GetStoreIssueResponseModel>> GetStoreIssueAsync(StoreIssuesGetModel request);

        public Task<ServiceResponse<string>> getStoreReqNo( int locationid);
        public  Task<ServiceResponse<string>> StoreIssueGridDataFilling(int locationid,  int srno, string srdate);

        public  Task<ServiceResponse<string>> StoreIssueSerachByRefNo(string refno);
        public  Task<ServiceResponse<string>> InsertStoreissuenew(List<StoreIssuesPostModel> HeaderData, string TrMode);

        public Task<ServiceResponse<string>> getAvlStockByWarehouse(int warehouseid,int materialid,int locationid,int SerialnoId);
    }
}
