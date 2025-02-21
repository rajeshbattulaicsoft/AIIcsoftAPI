using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto;
using AIIcsoftAPI.Dto.PDO;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.PDO
{
    public class PdosService : IPdosService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _pdoRepository;

        private readonly IMapper _mapper;

        public PdosService(IGenericRepository genericRepository, IRepository<AccessLevel> storeIssuesRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _pdoRepository = storeIssuesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPdoResponseModel>> GetJobCardNumberAsync()
        {
            var entity = await _pdoRepository.GetAllAsync();
            IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
            return response;
        }

        //public async Task<IEnumerable<GetPdoResponseModel>> GetPdoAsync(DateTime fromDate)
        //{
        //    var entity = await _pdoRepository.GetAllAsync();
        //    IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
        //    return response;
        //}

        //public async Task<ServiceResponse<string>> SavePdoAsync(List<PdosPostModel> pdosPostModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //    {
        //                            { "HeaderData", JsonConvert.SerializeObject(pdosPostModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_PDO_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Saved", "PDO", JsonConvert.SerializeObject(pdosPostModel));
        //}

        public async Task<ServiceResponse<string>> SavePdoAsync(List<PdosPostModel> HeaderData, string TrMode)
        {
            PdosPostModel obj = HeaderData[0]; // Assuming you need the first item
            string trtypeno = "926";
            string TxnNo = "0";
            string InvTxnNo="0";
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
// //inventory integration for pdo screen
// TransactionNumberGenerator pdo = new TransactionNumberGenerator(loginlocation);
//             //int itrtypeno = Convert.ToInt32(trtypeno);
//             int masteridpdo = pdo.fnGetMasterId(10000);
           
//             InvTxnNo = pdo.GetPrefixAndSlNo(masteridpdo, DateOnly.FromDateTime(DateTime.Now));
//             if (InvTxnNo.Contains("Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"))
//             {
//                 return new ServiceResponse<string>
//                 {
//                     Success = false,
//                     Data = "",
//                     Message = "Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"
//                 };
//             }
            //pdo number generator trnasction number genrations
            TransactionNumberGenerator tng = new TransactionNumberGenerator(loginlocation);
            int masterid = tng.fnGetFormId("frmStockTransfer", "Within Rejection Warehouse", "inventory");
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
                using (SqlCommand cmd = new SqlCommand("SP_PDO_Save", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("TxnNo", TxnNo);
                    cmd.Parameters.AddWithValue("HeaderData", mydata);
                    cmd.Parameters.AddWithValue("TrMode", TrMode);
                    //  cmd.Parameters.AddWithValue("InvTxnNo", InvTxnNo);
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
//Job card dropdown for pdo screen
public async Task<ServiceResponse<string>> GetJobCardDropdwonforPdo(int locationid)
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
                    cmd.CommandText = "USP_GetJobCardForPdo";

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


        //public async Task<ServiceResponse<string>> UpdatePdoAsync(List<PdosPutModel> pdosPutModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //                       {
        //                            { "HeaderData", JsonConvert.SerializeObject(pdosPutModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_PDO_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Updated", "PDO", JsonConvert.SerializeObject(pdosPutModel));
        //}

        public async Task<ServiceResponse<string>> UpdatePdoAsync(List<PdosPutModel> HeaderData, string TrMode)
        {
            string ConStr;
            SqlConnection con;

            DBConfig db = new DBConfig();
            ConStr = db.GetMainConnectionString();
            con = new SqlConnection(ConStr);
            con.Open();
            ServiceResponse<string> response = new ServiceResponse<string>();
            string mydata = string.Empty;
            string errormsg = string.Empty;
            if (HeaderData != null)
            {
                mydata = JsonConvert.SerializeObject(HeaderData);
                mydata = mydata;

            }
            else
            {
                response.Data = "";
                response.Success = false;
                response.Message = "No data available";

                return response;

            }

            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_PDO_Save";
                cmd.Parameters.AddWithValue("HeaderData", mydata);
                cmd.Parameters["HeaderData"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("TrMode", TrMode);
                cmd.Parameters["TrMode"].Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("TxnNo", "");
                cmd.Parameters.Add("NewTxnNo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("perrormsg", SqlDbType.VarChar, 500);
                cmd.Parameters["perrormsg"].Direction = ParameterDirection.Output;



                int savedcount = await cmd.ExecuteNonQueryAsync();
                errormsg = cmd.Parameters["perrormsg"].Value.ToString();
                if (errormsg == "")
                {
                    response.Success = true;
                    response.Data = mydata;
                    response.Message = "Record Updated successfully";

                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to Update record";
                }

            }



            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occured while Updating " + ex.Message;



            }
            finally
            {
                con.Close();
            }



            return response;
        }


        public async Task<ServiceResponse<string>> SearchDataByPdoGrnDate(string GrnDate,int locationid)
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
                    cmd.CommandText = "USP_GetSearchReferenceNumberPDO";
                    cmd.Parameters.AddWithValue("GrnDate", GrnDate);
                    cmd.Parameters["GrnDate"].Direction = ParameterDirection.Input;
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

        public async Task<ServiceResponse<string>> GetPdoRefNoByGrnDate(string RefNo)
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
                    cmd.CommandText = "USP_GetAutoPopulateDataByPDORefNumber";
                    cmd.Parameters.AddWithValue("RefNo", @RefNo);
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

        public async Task<ServiceResponse<string>> WareHouseDropdownForPDO(int plocationid)
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
                    cmd.CommandText = "USP_GetPdoWarehouse";
                    cmd.Parameters.AddWithValue("CompanyId", plocationid);
                    cmd.Parameters["CompanyId"].Direction = ParameterDirection.Input;


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

        public async Task<ServiceResponse<string>> GetRejectiontypeDropdwon()
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
                    cmd.CommandText = "USP_GetRejectionType";
                  

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

        public async Task<ServiceResponse<string>> QaDropdwonGet()
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
                    cmd.CommandText = "USP_GetQACode";


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
//Pdo method for get data By jobcard Number
 public async Task<ServiceResponse<string>> PdodataByJobcardNumber(string refno)
        {
            var serviceResponse = new ServiceResponse<string>();

            string ConStr = string.Empty;
            DBConfig db = new DBConfig();
            ConStr = db.GetMainConnectionString();


            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                try
                {
                    SqlCommand cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PDODataByJobCardNo";
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

    }
}
