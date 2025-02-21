using AIIcsoftAPI.Dto.JobCardClosures;
using AIIcsoftAPI.Dto.JobCardCreations;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.JobCardCreations
{
    public interface IJobCardCreationsService
    {
        //Task<ServiceResponse<string>> SaveJobCardCreationsAsync(List<JobCardCreationsPostModel> jobCardCreationsPostModel, string trMode);

        Task<ServiceResponse<string>> SaveJobCardCreationsAsyncNew(List<JobCardCreationsPostModel> HeaderData, string trMode);

        //Task<ServiceResponse<string>> UpdateJobCardCreationsAsync(List<JobCardCreationsPutModel> jobCardCreationsPutModel, string trMode);

        public Task<ServiceResponse<string>> UpdateJobCardCreationsAsync(List<JobCardCreationsPutModel> HeaderData, string TrMode);

        Task<IEnumerable<GetPdoResponseModel>> GetJobCardCreationsAsync();

        public Task<ServiceResponse<string>> GetRepairProcessDropdown();

        public Task<ServiceResponse<string>> FillDataByRefNumber(string refno);

        public Task<ServiceResponse<string>> BpPartCodeDropdown(string keystroke);
        public Task<ServiceResponse<string>> ReasonFillingDropdown();
        public Task<ServiceResponse<string>> AutoPopulateDatabySerialNo(int rawmatId,string serialNo);
        public Task<ServiceResponse<string>> serialNoFillDropdown();
        public Task<ServiceResponse<string>> JobcardRefNoByDeptId(int deptId,int locationid);

        public  Task<ServiceResponse<string>> getSerialNumberbyBpPartCode(int rawmatid,int locationid,int deptid);
        
        public Task<ServiceResponse<string>> DepartementDropdownJobcard(int plocationid);
         public  Task<ServiceResponse<string>> JobcardAddGridBymaterial(int materialid) ;

       


    }
}
