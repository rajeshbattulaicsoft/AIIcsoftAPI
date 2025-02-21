using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto.EstimatedJobCardApprovals;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.EstimatedJobCardApprovals
{
    public class EstimatedJobCardApprovalsService : IEstimatedJobCardApprovalsService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _storeIssuesRepository;

        private readonly IMapper _mapper;
        public EstimatedJobCardApprovalsService(IGenericRepository genericRepository, IRepository<AccessLevel> storeIssuesRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _storeIssuesRepository = storeIssuesRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> GetEstimatedJobCardApprovalAsync(int departmentId)
        {
            var serviceResponse = new ServiceResponse<string>();

            string ConStr = string.Empty;
            DBConfig db = new DBConfig();
            ConStr = db.GetMainConnectionString();
            // SqlConnection con;
            //  con = new SqlConnection(ConStr);

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GetJobCard";
                    cmd.Parameters.AddWithValue("deptId", departmentId);
                    cmd.Parameters["deptId"].Direction = ParameterDirection.Input;

                    string DBData = "[]";


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DBData = "[";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DBData = DBData + dt.Rows[i]["DATA"].ToString() + ",";
                        }
                        DBData = DBData.Substring(0, DBData.Length - 1);
                        DBData = DBData + "]";

                        serviceResponse.Data = DBData;
                        serviceResponse.Success = true;
                        serviceResponse.Message = "Data fetched successfully";
                    }
                    else
                    {
                        DBData = "[]";
                        serviceResponse.Data = DBData;
                        serviceResponse.Success = false;
                        serviceResponse.Message = " data not found";
                    }





                }
                catch (Exception ex)
                {

                    serviceResponse.Success = false;
                    serviceResponse.Message = ex.Message;

                }
                finally
                {
                    con.Close();
                }
            }

            return serviceResponse;
        }

        //public async Task<IEnumerable<GetStoreIssueResponseModel>> GetEstimatedJobCardApprovalAsync(int departmentId)
        //{
        //    var entity = await _storeIssuesRepository.GetAllAsync();
        //    IEnumerable<GetStoreIssueResponseModel> response = _mapper.Map<IEnumerable<GetStoreIssueResponseModel>>(entity);
        //    return response;
        //}

        public async Task<ServiceResponse<string>> SaveEstimatedJobCardApprovalAsync(List<EstimatedJobCardApprovalsPostModel> estimatedJobCardApprovalsPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
            {
                                    { "HeaderData", JsonConvert.SerializeObject(estimatedJobCardApprovalsPostModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_JobCard_Approval_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "EstimatedJobCardApproval", JsonConvert.SerializeObject(estimatedJobCardApprovalsPostModel));
        }
    
    }
}
