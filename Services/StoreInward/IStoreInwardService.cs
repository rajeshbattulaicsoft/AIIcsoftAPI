using AIIcsoftAPI.Dto.StoreInward;
using AIIcsoftAPI.Dto.StoreIssues;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.StoreInward
{
    public interface IStoreInwardService
    {
        Task<ServiceResponse<string>> SaveStoreInwardAsync(List<StoreInwardPostModel> storeInwardPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateStoreInwardAsync(List<StoreInwardPutModel> storeInwardPutModel, string trMode);
        Task<IEnumerable<GetStoreIssueResponseModel>> GetStoreInwardAsync(StoreInwardGetModel request);
        Task<GetStoreIssueResponseModel> GetByReceiptIdAsync(int receiptId);

        public Task<ServiceResponse<string>> ELRCStoreInwardSerachByDepIdAndDate(int locationid, int deptid, string srdate);

        public Task<ServiceResponse<string>> StoreInwardSearchByRefNo(string refno);
        public Task<ServiceResponse<string>> JobcardNoForElrcStore(int locationid);
        public  Task<ServiceResponse<string>> ElrcDataByJobcard(string refno);

        public Task<ServiceResponse<string>> saveStoreInwardNew(List<StoreInwardPostModel> HeaderData, string TrMode);
    }
}
