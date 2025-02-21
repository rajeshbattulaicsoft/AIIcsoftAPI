using AIIcsoftAPI.Dto.PDO;
using AIIcsoftAPI.Dto.StoreIssues;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.PDO
{
    public interface IPdosService
    {

        // Task<ServiceResponse<string>> UpdatePdoAsync(List<PdosPutModel> pdosPutModel, string v);
        //Task<ServiceResponse<string>> SavePdoAsync(List<PdosPostModel> pdosPostModel, string v);

        Task<ServiceResponse<string>> UpdatePdoAsync(List<PdosPutModel> HeaderData, string TrMode);

        Task<ServiceResponse<string>> SavePdoAsync(List<PdosPostModel> HeaderData, string TrMode);
        Task<IEnumerable<GetPdoResponseModel>> GetJobCardNumberAsync();
        //Task<IEnumerable<GetPdoResponseModel>> GetPdoAsync(DateTime fromDate);

        Task<ServiceResponse<string>> SearchDataByPdoGrnDate(string GrnDate,int locationid);

        Task<ServiceResponse<string>> GetPdoRefNoByGrnDate(string RefNo);

        public Task<ServiceResponse<string>> WareHouseDropdownForPDO(int plocationid);
        public Task<ServiceResponse<string>> GetRejectiontypeDropdwon();
        public Task<ServiceResponse<string>> QaDropdwonGet();
        public Task<ServiceResponse<string>> GetJobCardDropdwonforPdo(int locationid);
        public  Task<ServiceResponse<string>> PdodataByJobcardNumber(string refno);
    }
}
