using AIIcsoftAPI.Dto.CustomerMaterialReturns;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.CustomerMaterialReturns
{
    public interface ICustomerMaterialReturnsService
    {
        Task<ServiceResponse<string>> SaveCustomerMaterialReturnsAsync(List<CustomerMaterialReturnsPostModel> customerMaterialReturnsPostModel, string trMode);
        Task<ServiceResponse<string>> UpdateCustomerMaterialReturnsAsync(List<CustomerMaterialReturnsPutModel> customerMaterialReturnsPutModel, string trMode);
        Task<IEnumerable<GetPdoResponseModel>> GetCustomerMaterialReturnsAsync(CustomerMaterialReturnsGetModel request);
        Task<IEnumerable<GetPdoResponseModel>> GetBatterySerialNumberAsync();

        public Task<ServiceResponse<string>> getRefernceNoFromdate(int custid, string fromdate,int locationId);

        Task<ServiceResponse<string>> PopulateDataByRefNumber(string referenceNo);

        public  Task<ServiceResponse<string>> SaveCustomerMaterialReturnsAsyncNew(List<CustomerMaterialReturnsPostModel> HeaderData, string TrMode);

        public  Task<ServiceResponse<string>> UpdateCustomerMaterialReturnsAsyncNew(List<CustomerMaterialReturnsPutModel> HeaderData, string TrMode);
        Task<ServiceResponse<string>> GetCMRRDataByCustomerIdAsync(int custId, int locationId,int WarehouseId);
    }
}
