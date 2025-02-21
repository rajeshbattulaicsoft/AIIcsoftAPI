using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto.IssueToRepair;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.IssueToRepair
{
    public class IssueToRepairService : IIssueToRepairService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _issueToRepairRepository;

        private readonly IMapper _mapper;
        public IssueToRepairService(IGenericRepository genericRepository, IRepository<AccessLevel> issueToRepairRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _issueToRepairRepository = issueToRepairRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetStoreIssueResponseModel>> GetIssueToRepairAsync(IssueToRepairGetModel request)
        {
            var entity = await _issueToRepairRepository.GetAllAsync();
            IEnumerable<GetStoreIssueResponseModel> response = _mapper.Map<IEnumerable<GetStoreIssueResponseModel>>(entity);
            return response;
        }

        public async Task<ServiceResponse<string>> SaveIssueToRepairAsync(List<IssueToRepairPostModel> issueToRepairPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
            {
                                    { "HeaderData", JsonConvert.SerializeObject(issueToRepairPostModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_StoreIssue_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Issue to Repair", JsonConvert.SerializeObject(issueToRepairPostModel));
        }

        public async Task<ServiceResponse<string>> UpdateIssueToRepairAsync(List<IssueToRepairPutModel> issueToRepairPutModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(issueToRepairPutModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_StoreIssue_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Issue to Repair", JsonConvert.SerializeObject(issueToRepairPutModel));
        }

        public async Task<ServiceResponse<string>> IssueToRepairGridFill(int locationid, int srno, string srdate)
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
                    cmd.CommandText = "USP_GET_IssueToStoreRepairData";
                    cmd.Parameters.AddWithValue("LocationId", locationid);
                    cmd.Parameters["LocationId"].Direction = ParameterDirection.Input;
                    // cmd.Parameters.AddWithValue("DepartmentId", deptid);
                    // cmd.Parameters["DepartmentId"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("SrNo", srno);
                    cmd.Parameters["SrNo"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(new SqlParameter("SRDate", SqlDbType.Date) { Value = HelperFunctions.GetDateOnlyFromYYYYMMDD(srdate) });
                    // cmd.Parameters.AddWithValue("Warehouse", warehouseid);
                    // cmd.Parameters["Warehouse"].Direction = ParameterDirection.Input;
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

//Issue To Repair job card dropdwon
public async Task<ServiceResponse<string>> GetJobCardDropdwonIssueToRepair(int locationid)
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
                    cmd.CommandText = "USP_GetJobCardForIssueToRepairStore";
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
//Issue To Repair Store By Job Card No
        public async Task<ServiceResponse<string>> IssueToRepairByJobcardNo(string RefNo)
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
                    cmd.CommandText = "IssueToRepairStoreByJobcardNo";
                    cmd.Parameters.AddWithValue("RefNo", RefNo);
                    cmd.Parameters["RefNo"].Direction = ParameterDirection.Input;
               

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

//Issue To Repair Store By IssueRefNo
         public async Task<ServiceResponse<string>> SearchByIssueRefNo(string RefNo)
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
                    cmd.CommandText = "Sp_IssueToRepairStoreSearchByRefNo";
                    cmd.Parameters.AddWithValue("RefNo", RefNo);
                    cmd.Parameters["RefNo"].Direction = ParameterDirection.Input;
               

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
        public async Task<ServiceResponse<string>> StoreIsueSerachByDepIdAndDate(int locationid, int deptid, string srdate)
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
                    cmd.CommandText = "USP_Search_StoreIssueAndRepairToStoreData";
                    cmd.Parameters.AddWithValue("LocationId", locationid);
                    cmd.Parameters["LocationId"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("DepartmentId", deptid);
                    cmd.Parameters["DepartmentId"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(new SqlParameter("SRDate", SqlDbType.Date) { Value = HelperFunctions.GetDateOnlyFromYYYYMMDD(srdate) });
                   
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

        public async Task<ServiceResponse<string>> SerialNoListForIssueToRepair(int rawmatid, int warehouseid)
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
                    cmd.CommandText = "getSerialnoListForIssueToRepairStore";
                    cmd.Parameters.AddWithValue("rawmatid", rawmatid);
                    cmd.Parameters["rawmatid"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("warehouseid", warehouseid);
                    cmd.Parameters["warehouseid"].Direction = ParameterDirection.Input;
                  
                   
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
