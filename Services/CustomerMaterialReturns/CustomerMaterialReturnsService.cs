using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto.CustomerMaterialReturns;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.CustomerMaterialReturns
{
    public class CustomerMaterialReturnsService : ICustomerMaterialReturnsService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _customerMaterialReturnsRepository;

        private readonly IMapper _mapper;
        public CustomerMaterialReturnsService(IGenericRepository genericRepository, IRepository<AccessLevel> customerMaterialReturnsRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _customerMaterialReturnsRepository = customerMaterialReturnsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPdoResponseModel>> GetBatterySerialNumberAsync()
        {
            var entity = await _customerMaterialReturnsRepository.GetAllAsync();
            IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
            return response;
        }

        public async Task<IEnumerable<GetPdoResponseModel>> GetCustomerMaterialReturnsAsync(CustomerMaterialReturnsGetModel request)
        {
            var entity = await _customerMaterialReturnsRepository.GetAllAsync();
            IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
            return response;
        }

        public async Task<ServiceResponse<string>> UpdateCustomerMaterialReturnsAsyncNew(List<CustomerMaterialReturnsPutModel> HeaderData, string TrMode)
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
                cmd.CommandText = "SP_CustomerMaterialReturns_Save";
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
        public async Task<ServiceResponse<string>> SaveCustomerMaterialReturnsAsyncNew(List<CustomerMaterialReturnsPostModel> HeaderData, string TrMode)
        {
            CustomerMaterialReturnsPostModel obj = HeaderData[0]; // Assuming you need the first item
            string trtypeno = "926";
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
            int masterid = tng.fnGetFormId("frmCustMatReturn", "CMR", "inventory");
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
                using (SqlCommand cmd = new SqlCommand("SP_CustomerMaterialReturns_Save", con))
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
                            response.Message = "Failed to insert record: " + errorMessage;
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

        public async Task<ServiceResponse<string>> SaveCustomerMaterialReturnsAsync(List<CustomerMaterialReturnsPostModel> customerMaterialReturnsPostModel, string trMode)
        {
            var inputParams = new Dictionary<string, object>
            {
                                    { "HeaderData", JsonConvert.SerializeObject(customerMaterialReturnsPostModel) },{ "TrMode", trMode }
                                };


            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_CustomerMaterialReturns_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Customer Material Returns", JsonConvert.SerializeObject(customerMaterialReturnsPostModel));
        }

        public async Task<ServiceResponse<string>> UpdateCustomerMaterialReturnsAsync(List<CustomerMaterialReturnsPutModel> customerMaterialReturnsPutModel, string trMode)
        {

            var inputParams = new Dictionary<string, object>
                               {
                                    { "HeaderData", JsonConvert.SerializeObject(customerMaterialReturnsPutModel) },{ "TrMode", trMode }
                                };

            var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("SP_CustomerMaterialReturns_Save", inputParams, "perrormsg");
            return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Customer Material Returns", JsonConvert.SerializeObject(customerMaterialReturnsPutModel));
        }

        public async Task<ServiceResponse<string>> getRefernceNoFromdate(int custid, string fromdate,int locationId)
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
                    cmd.CommandText = "USP_GetReferenceNumberForCMMR";
                    cmd.Parameters.AddWithValue("CustId", custid);
                    cmd.Parameters["CustId"].Direction = ParameterDirection.Input;
                     cmd.Parameters.AddWithValue("ReturndDate", fromdate);
                    cmd.Parameters["ReturndDate"].Direction = ParameterDirection.Input;
                    //cmd.Parameters.Add(new SqlParameter("ReturndDate", SqlDbType.Date) { Value = HelperFunctions.GetDateOnlyFromYYYYMMDD(fromdate) });
                    cmd.Parameters.AddWithValue("locationid", locationId);
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

        public async Task<ServiceResponse<string>> GetCMRRDataByCustomerIdAsync(int custId, int locationId,int WarehouseId)
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
                    cmd.CommandText = "USP_GetCMRRDataByCustomerId";
                    cmd.Parameters.AddWithValue("CustId", custId);
                    cmd.Parameters["CustId"].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("LocationId", locationId);
                    cmd.Parameters["LocationId"].Direction = ParameterDirection.Input;
                     cmd.Parameters.AddWithValue("WarehouseId", WarehouseId);
                    cmd.Parameters["WarehouseId"].Direction = ParameterDirection.Input;


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

        public async Task<ServiceResponse<string>> PopulateDataByRefNumber(string referenceNo)
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
                    cmd.CommandText = "USP_GetCmmrDataByRefNo";
                    cmd.Parameters.AddWithValue("ReferenceNo", referenceNo);
                    cmd.Parameters["ReferenceNo"].Direction = ParameterDirection.Input;
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
