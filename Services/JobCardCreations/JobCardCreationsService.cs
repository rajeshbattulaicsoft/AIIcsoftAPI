using AutoMapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.DAL;
using AIIcsoftAPI.Dto;
using AIIcsoftAPI.Dto.JobCardClosures;
using AIIcsoftAPI.Dto.JobCardCreations;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.ResponseModels;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.Services.JobCardCreations
{
    public class JobCardCreationsService : IJobCardCreationsService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _jobCardCreationsRepository;

        private readonly IMapper _mapper;

        public JobCardCreationsService(IGenericRepository genericRepository, IRepository<AccessLevel> jobCardCreationsRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _jobCardCreationsRepository = jobCardCreationsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPdoResponseModel>> GetJobCardCreationsAsync()
        {
            var entity = await _jobCardCreationsRepository.GetAllAsync();
            IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
            return response;
        }


  public async Task<ServiceResponse<string>> DepartementDropdownJobcard(int plocationid)
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
                    cmd.CommandText = "getDepartmentElrcDataForJobcard";
                    cmd.Parameters.AddWithValue("plocationid", plocationid);
                    cmd.Parameters["plocationid"].Direction = ParameterDirection.Input;

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

        public async Task<ServiceResponse<string>> GetRepairProcessDropdown()
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
                    cmd.CommandText = "USP_getjobcardrepairprocess";

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


        //public async Task<ServiceResponse<string>> SaveJobCardCreationsAsync(List<JobCardCreationsPostModel> jobCardCreationsPostModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //    {
        //                            { "HeaderData", JsonConvert.SerializeObject(jobCardCreationsPostModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_JobCard_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Job Card Creation", JsonConvert.SerializeObject(jobCardCreationsPostModel));
        //}
       public async Task<ServiceResponse<string>> SaveJobCardCreationsAsyncNew(List<JobCardCreationsPostModel> HeaderData, string trMode)
        {
            JobCardCreationsPostModel obj = HeaderData[0]; // Assuming you need the first item
            string trtypeno = "926";
            string TxnNo = "0";
            string MrsTxnNo="0";
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
            //For Invenroy Integrations
TransactionNumberGenerator jobcard = new TransactionNumberGenerator(loginlocation);
            //int itrtypeno = Convert.ToInt32(trtypeno);
            int masteridjobcard = jobcard.fnGetMasterId(10000);
           
            InvTxnNo = jobcard.GetPrefixAndSlNo(masteridjobcard, DateOnly.FromDateTime(DateTime.Now));
            if (InvTxnNo.Contains("Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Data = "",
                    Message = "Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"
                };
            }
            TransactionNumberGenerator tng = new TransactionNumberGenerator(loginlocation);
            //int itrtypeno = Convert.ToInt32(trtypeno);
            int masterid = tng.fnGetFormId("frmJobCard", "Job Card", "inventory");
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

            int mrsmasterid = tng.fnGetFormId("frmStoreRequisition", "", "inventory");
            MrsTxnNo = tng.GetPrefixAndSlNo(mrsmasterid, DateOnly.FromDateTime(DateTime.Now));
              if (MrsTxnNo.Contains("Prefix is not defined in PrefixGlobalSetting table. Please set it and then continue...!"))
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
                using (SqlCommand cmd = new SqlCommand("USP_JobCard_Save", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("TxnNo", TxnNo);
                     cmd.Parameters.AddWithValue("MrsTxnNo", MrsTxnNo);
                    cmd.Parameters.AddWithValue("HeaderData", mydata);
                    cmd.Parameters.AddWithValue("TrMode", trMode);
                    cmd.Parameters.AddWithValue("InvTxnNo", InvTxnNo);
                    cmd.Parameters.Add("NewTxnNo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MrsNewTxnNo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("perrormsg", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();

                        string NewTxnNo = cmd.Parameters["NewTxnNo"].Value.ToString();
                        string MrsNewTxnNo=cmd.Parameters["MrsNewTxnNo"].Value.ToString();
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


        //public async Task<ServiceResponse<string>> UpdateJobCardCreationsAsync(List<JobCardCreationsPutModel> jobCardCreationsPutModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //                       {
        //                            { "HeaderData", JsonConvert.SerializeObject(jobCardCreationsPutModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_JobCard_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Job Card Creation", JsonConvert.SerializeObject(jobCardCreationsPutModel));
        //}

        public async Task<ServiceResponse<string>> UpdateJobCardCreationsAsync(List<JobCardCreationsPutModel> HeaderData, string TrMode)
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
                cmd.CommandText = "USP_JobCard_Save";

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
                    response.Message = "Failed to insert record " + errormsg;

                }

            }



            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = "An error occured while inserting " + ex.Message;



            }
            finally
            {
                con.Close();
            }

            return response;
        }

        public async Task<ServiceResponse<string>> FillDataByRefNumber(string refno)
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
                    cmd.CommandText = "USP_GetJobCardByRefNo";
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

        public async Task<ServiceResponse<string>> BpPartCodeDropdown(string keystroke)
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
                    cmd.CommandText = "SP_PartDetails";
                    cmd.Parameters.AddWithValue("keystroke", keystroke);
                    cmd.Parameters["keystroke"].Direction = ParameterDirection.Input;



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

        public async Task<ServiceResponse<string>> ReasonFillingDropdown()
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
                    cmd.CommandText = "SP_ReasonDetails";
                   

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

        public async Task<ServiceResponse<string>> AutoPopulateDatabySerialNo(int rawmatId, string serialNo)
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
            cmd.CommandText = "SP_PartBomDetails";
            cmd.Parameters.AddWithValue("hRawMatId", rawmatId);
            cmd.Parameters["hRawMatId"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("hSerialNo", serialNo);
            cmd.Parameters["hSerialNo"].Direction = ParameterDirection.Input;

            string DBData = "[]";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Check if rows are returned
            if (dt.Rows.Count > 0)
            {
                // Use a HashSet to collect unique data
                HashSet<string> uniqueData = new HashSet<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string data = row["DATA"].ToString();
                    // Add data to the HashSet (duplicates will be automatically ignored)
                    if (!string.IsNullOrEmpty(data))
                    {
                        uniqueData.Add(data);
                    }
                }

                // Create JSON-like structure for unique data
                if (uniqueData.Count > 0)
                {
                    DBData = "[" + string.Join(",", uniqueData) + "]";
                }

                serviceResponse.Data = DBData;
                serviceResponse.Success = true;
                serviceResponse.Message = "Data fetched successfully";
            }
            else
            {
                DBData = "[]";
                serviceResponse.Data = DBData;
                serviceResponse.Success = false;
                serviceResponse.Message = "Data not found";
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

        public async Task<ServiceResponse<string>> serialNoFillDropdown()
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
                    cmd.CommandText = "SP_PartSlNoDetails";
                    // cmd.Parameters.AddWithValue("keystroke", keystroke);
                    // cmd.Parameters["keystroke"].Direction = ParameterDirection.Input;


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

        public async Task<ServiceResponse<string>> JobcardRefNoByDeptId(int deptId,int locationid) 
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
                    cmd.CommandText = "USP_GetJobCardRefNoByDeptId";
                    cmd.Parameters.AddWithValue("deptId", deptId);
                    cmd.Parameters["deptId"].Direction = ParameterDirection.Input;

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

        public async Task<ServiceResponse<string>> getSerialNumberbyBpPartCode(int rawmatid,int locationid,int deptid) 
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
                    cmd.CommandText = "USP_GetJobCardSerialNumberByBPPartId";
                    cmd.Parameters.AddWithValue("RawMatId", rawmatid);
                    cmd.Parameters["RawMatId"].Direction = ParameterDirection.Input;
                       cmd.Parameters.AddWithValue("locationid", locationid);
                    cmd.Parameters["locationid"].Direction = ParameterDirection.Input;
                       cmd.Parameters.AddWithValue("deptid", deptid);
                    cmd.Parameters["deptid"].Direction = ParameterDirection.Input;


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

         public async Task<ServiceResponse<string>> JobcardAddGridBymaterial(int materialid) 
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
                    cmd.CommandText = "JobCardDataByMaterialId";
                     cmd.Parameters.AddWithValue("materialid", materialid);
                    cmd.Parameters["materialid"].Direction = ParameterDirection.Input;


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
