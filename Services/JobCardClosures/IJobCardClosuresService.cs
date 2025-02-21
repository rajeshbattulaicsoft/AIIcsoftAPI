using AIIcsoftAPI.Dto.JobCardClosures;
using AIIcsoftAPI.Models.ResponseModels;

namespace AIIcsoftAPI.Services.JobCardClosures
{
    public interface IJobCardClosuresService
    {
        //Task<ServiceResponse<string>> SaveJobCardClosuresAsync(List<JobCardClosuresPostModel> jobCardClosuresPostModel, string trMode);

        Task<ServiceResponse<string>> SaveJobCardClosuresAsyncnew(List<JobCardClosuresPostModel> HeaderData, string trMode);


        //Task<ServiceResponse<string>> UpdateJobCardClosuresAsync(List<JobCardClosuresPutModel> jobCardClosuresPutModel, string trMode);
        public Task<ServiceResponse<string>> UpdateJobCardClosuresAsync(List<JobCardClosuresPutModel> HeaderData, string TrMode);
        Task<IEnumerable<GetPdoResponseModel>> GetJobCardClosuresAsync(JobCardClosuresGetModel request);

        public Task<ServiceResponse<string>> GetJobCardRefNoByDeptID(int deptid);
        public Task<ServiceResponse<string>> GetJobcardClosureDataByRefNo(string refno);

        public Task<ServiceResponse<string>> GetJobCardRefNoDropDown();
        public  Task<ServiceResponse<string>> JobcardClosureRefNoByDeptId(int deptId,int locationid);
        public  Task<ServiceResponse<string>> GetJobCardDropdwonInJobCardClosure(int locationid);
        public Task<ServiceResponse<string>> ClosuredataByJobcardNo(string refno);


    }
}
