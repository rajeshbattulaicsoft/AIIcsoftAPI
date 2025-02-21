using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

namespace AIIcsoftAPI.Services.JobCardClosures
{
    public class JobCardClosuresService : IJobCardClosuresService
    {
        private readonly IGenericRepository _genericRepository;

        private readonly IRepository<AccessLevel> _jobCardClosuresRepository;

        private readonly IMapper _mapper;

        public JobCardClosuresService(IGenericRepository genericRepository, IRepository<AccessLevel> jobCardClosuresRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _jobCardClosuresRepository = jobCardClosuresRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPdoResponseModel>> GetJobCardClosuresAsync(JobCardClosuresGetModel request)
        {
            var entity = await _jobCardClosuresRepository.GetAllAsync();
            IEnumerable<GetPdoResponseModel> response = _mapper.Map<IEnumerable<GetPdoResponseModel>>(entity);
            return response;
        }

        //public async Task<ServiceResponse<string>> SaveJobCardClosuresAsync(List<JobCardClosuresPostModel> jobCardClosuresPostModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //    {
        //                            { "HeaderData", JsonConvert.SerializeObject(jobCardClosuresPostModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_JobCardClouser_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Saved", "Job Card  Clouser", JsonConvert.SerializeObject(jobCardClosuresPostModel));
        //}

       public async Task<ServiceResponse<string>> SaveJobCardClosuresAsyncnew(List<JobCardClosuresPostModel> HeaderData, string trMode)
        {
            JobCardClosuresPostModel obj = HeaderData[0]; // Assuming you need the first item
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

 TransactionNumberGenerator jobcardclsoure = new TransactionNumberGenerator(loginlocation);
            //int itrtypeno = Convert.ToInt32(trtypeno);
            int masteridjobcardclsore = jobcardclsoure.fnGetMasterId(10000);
           
            InvTxnNo = jobcardclsoure.GetPrefixAndSlNo(masteridjobcardclsore, DateOnly.FromDateTime(DateTime.Now));
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
            int masterid = tng.fnGetFormId("frmJobCardClosure", "Job Card Closure", "inventory");
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
                using (SqlCommand cmd = new SqlCommand("USP_JobCardClouser_Save", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("TxnNo", TxnNo);
                    cmd.Parameters.AddWithValue("HeaderData", mydata);
                    cmd.Parameters.AddWithValue("TrMode", trMode);
                    cmd.Parameters.AddWithValue("InvTxnNo", InvTxnNo);
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




        //public async Task<ServiceResponse<string>> UpdateJobCardClosuresAsync(List<JobCardClosuresPutModel> jobCardClosuresPutModel, string trMode)
        //{
        //    var inputParams = new Dictionary<string, object>
        //                       {
        //                            { "HeaderData", JsonConvert.SerializeObject(jobCardClosuresPutModel) },{ "TrMode", trMode }
        //                        };

        //    var result = await _genericRepository.ExecuteStoredProcedureAsync<string>("USP_JobCardClouser_Save", inputParams, "perrormsg");
        //    return await _genericRepository.GetResponseAsync(result.Success, "Updated", "Job Card Creation", JsonConvert.SerializeObject(jobCardClosuresPutModel));
        //}

        public async Task<ServiceResponse<string>> UpdateJobCardClosuresAsync(List<JobCardClosuresPutModel> HeaderData, string TrMode)
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
                cmd.CommandText = "USP_JobCardClouser_Save";

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


        public async Task<ServiceResponse<string>> GetJobCardRefNoByDeptID(int deptid)
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
                    cmd.CommandText = "USP_GetJobClouserCardByDepartment";
                    cmd.Parameters.AddWithValue("deptId", deptid);
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

        public async Task<ServiceResponse<string>> GetJobcardClosureDataByRefNo(string refno)
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
                    cmd.CommandText = "USP_GetJobCardClouserByRefNo";
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


        public async Task<ServiceResponse<string>> GetJobCardRefNoDropDown()
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
                    cmd.CommandText = "USP_GetReferenceNumberForJobCard";
               

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



        public async Task<ServiceResponse<string>> JobcardClosureRefNoByDeptId(int deptId,int locationid)
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
                    cmd.CommandText = "USP_GetJobCardClosureRefNoByDeptId";
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

        //Job Card Closure Job card Dropdwon new added 

    public async Task<ServiceResponse<string>> GetJobCardDropdwonInJobCardClosure(int locationid)
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
                    cmd.CommandText = "USP_GetJobCardForClosure";
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

        //Closuredata By JobcardNumber With Completed IssueTo Repair 
        public async Task<ServiceResponse<string>> ClosuredataByJobcardNo(string refno)
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
                    cmd.CommandText = "GetClosureByJobCardRefNumber";
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
