using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto;
using AIIcsoftAPI.Dto.StoreInward;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.StoreInward
{
    public class StoreInwardService : IStoreInwardService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _storeInwardRepository;

        private readonly IMapper _mapper;

        public StoreInwardService(IGenericRepository genericRepository, IRepository<AccessLevel> storeInwardRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _storeInwardRepository = storeInwardRepository;
            _mapper = mapper;
        }

        public async Task<GetStoreIssueResponseModel> GetByReceiptIdAsync(int receiptId)
        {
            var entity = await _storeInwardRepository.GetAsync(receiptId);
            GetStoreIssueResponseModel response = _mapper.Map<GetStoreIssueResponseModel>(entity);
            return response;
        }

        public async Task<IEnumerable<GetStoreIssueResponseModel>> GetStoreInwardAsync(StoreInwardGetModel request)
        {
            var entity = await _storeInwardRepository.GetAllAsync();
            IEnumerable<GetStoreIssueResponseModel> response = _mapper.Map<IEnumerable<GetStoreIssueResponseModel>>(entity);
            return response;
        }

        public async Task<ServiceResponse<string>> SaveStoreInwardAsync(List<StoreInwardPostModel> storeInwardPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(storeInwardPostModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_StoreInward_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Store Inward", JsonConvert.SerializeObject(storeInwardPostModel));
        }

        public async Task<ServiceResponse<string>> UpdateStoreInwardAsync(List<StoreInwardPutModel> storeInwardPutModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(storeInwardPutModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_StoreInward_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Store Inward", JsonConvert.SerializeObject(storeInwardPutModel));
        }

        public async Task<ServiceResponse<string>> ELRCStoreInwardSerachByDepIdAndDate(int locationid, int deptid, string srdate)
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
                    cmd.CommandText = "USP_Search_ELRCStoreInwardData";
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

        public async Task<ServiceResponse<string>> StoreInwardSearchByRefNo(string refno)
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
                    cmd.CommandText = "USP_Search_ELRCStoreInwardDataByRecieptNo";
                    cmd.Parameters.AddWithValue("RecieptNo", refno);
                    cmd.Parameters["RecieptNo"].Direction = ParameterDirection.Input;
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

//ELRC STORE INWARD DATA BY JOB CARD NUMBER
 public async Task<ServiceResponse<string>> ElrcDataByJobcard(string refno)
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
                    cmd.CommandText = "ElrcStoreDataByJobCardNo";
                    cmd.Parameters.AddWithValue("RefNo", refno);
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

//Job Card For ELRC STORE INWRAD
  public async Task<ServiceResponse<string>> JobcardNoForElrcStore(int locationid)
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
                    cmd.CommandText = "USP_GetJobCardForElrcStoreInward";
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
        public async Task<ServiceResponse<string>> saveStoreInwardNew(List<StoreInwardPostModel> HeaderData, string TrMode)
        {
            StoreInwardPostModel obj = HeaderData[0]; // Assuming you need the first item
            string trtypeno = "0";
            string TxnNo = "0";
            string loginlocation = obj.companyid;
            if (HeaderData == null || !HeaderData.Any())
            {
                return new ServiceResponse<string>
                {
                    Data = "",
                    Success = false,
                    Message = "No data available"
                };
            }



            ServiceResponse<string> response = new ServiceResponse<string>();
            string mydata = JsonConvert.SerializeObject(HeaderData);

            if (string.IsNullOrEmpty(trtypeno))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Data = "",
                    Message = "trtypeno is null"
                };
            }

            if (string.IsNullOrEmpty(loginlocation))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Data = "",
                    Message = "location id is null"
                };
            }

            TransactionNumberGenerator tng = new TransactionNumberGenerator(loginlocation);
            //int itrtypeno = Convert.ToInt32(trtypeno);
            int masterid = tng.fnGetFormId("frmWIPoutputinward", "WIP Output Inward", "inventory");
            TxnNo = tng.GetPrefixAndSlNo(masterid, DateOnly.FromDateTime(DateTime.Now));
            if (TxnNo.Contains("Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Data = "",
                    Message = "Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"
                };
            }
            using (SqlConnection con = new SqlConnection(new DBConfig().GetMainConnectionString()))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SP_StoreInward_Save", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("TxnNo", TxnNo);
                    cmd.Parameters.AddWithValue("HeaderData", mydata);
                    cmd.Parameters.AddWithValue("TrMode", TrMode);
                    cmd.Parameters.Add("NewTxnNo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("perrormsg", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();

                        string NewTxnNo = cmd.Parameters["NewTxnNo"].Value.ToString();
                        string errorMessage = cmd.Parameters["perrormsg"].Value.ToString();

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            response.Success = true;
                            response.Data = NewTxnNo;
                            response.Message = "Record inserted successfully";
                        }
                        else
                        {
                            response.Success = false;
                            response.Data = errorMessage;
                            response.Message = "Failed to update record: " + errorMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Data = null;
                        response.Message = "An error occurred while inserting: " + ex.Message;
                    }
                }
            }

            return response;
        }

    }
}
