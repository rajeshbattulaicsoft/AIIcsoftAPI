using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto.MRSApprovals;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AIIcsoftAPI.Services.MrsApprovals
{
    public class MrsApprovalsService : IMrsApprovalsService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _mrsApprovalRepository;

        private readonly IMapper _mapper;
        public MrsApprovalsService(IGenericRepository genericRepository, IRepository<AccessLevel> mrsApprovalRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mrsApprovalRepository = mrsApprovalRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetMrsApprovalResponseModel>> GetMrsApprovalAsync(MrsApprovalsGetModel request)
        {
            IEnumerable<GetMrsApprovalResponseModel> response = new List<GetMrsApprovalResponseModel>() { 
                new GetMrsApprovalResponseModel 
            { JobCardDate = DateTime.Now, JobCardNumber = "123",SrNo = 1, Material = "Test", MaterialSno = "Test123", ProfitCenter = "test",
                    ReferenceDate = DateTime.Now, ReferenceNo = "123" } ,
                new GetMrsApprovalResponseModel{JobCardDate = DateTime.Now,JobCardNumber = "123",SrNo = 2,Material = "Test",MaterialSno = "Test123",ProfitCenter = "test",ReferenceDate = DateTime.Now,ReferenceNo = "123" },
                new GetMrsApprovalResponseModel{JobCardDate = DateTime.Now,JobCardNumber = "123",SrNo = 3,Material = "Test",MaterialSno = "Test123",ProfitCenter = "test",ReferenceDate = DateTime.Now,ReferenceNo = "123" },
                new GetMrsApprovalResponseModel{JobCardDate = DateTime.Now,JobCardNumber = "123",SrNo = 4,Material = "Test",MaterialSno = "Test123",ProfitCenter = "test",ReferenceDate = DateTime.Now,ReferenceNo = "123" }
            };
            return response;
        }

       

        public async Task<ServiceResponse<string>> SaveMrsApprovalAsync(List<MrsApprovalsPostModel> mrsApprovalsPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
            {
                                    { "HeaderData", JsonConvert.SerializeObject(mrsApprovalsPostModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_MRSApproval_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "MRS Approval", JsonConvert.SerializeObject(mrsApprovalsPostModel));
        }

        public async Task<ServiceResponse<string>> UpdateMrsApprovalAsync(List<MrsApprovalsPutModel> mrsApprovalsPutModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(mrsApprovalsPutModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_MRSApproval_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Updated", "MRS Approval", JsonConvert.SerializeObject(mrsApprovalsPutModel));
        }

        public async Task<ServiceResponse<string>> GetMRSApprovalGridFill(int deptid,string approvaldate,int locationid)
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
                    cmd.CommandText = "USP_GET_MRSApproval";
                    cmd.Parameters.AddWithValue("DepartmentId", deptid);
                    cmd.Parameters["DepartmentId"].Direction = ParameterDirection.Input;
                     cmd.Parameters.AddWithValue("approvedDate", approvaldate);
                    cmd.Parameters["approvedDate"].Direction = ParameterDirection.Input;
                    //cmd.Parameters.Add(new SqlParameter("approvedDate", SqlDbType.Date) { Value = HelperFunctions.GetDateOnlyFromYYYYMMDD(approvaldate) });
                    cmd.Parameters.AddWithValue("locationid", locationid);
                    cmd.Parameters["locationid"].Direction = ParameterDirection.Input;


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
    }
}
