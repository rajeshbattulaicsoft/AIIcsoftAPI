//API Authentication service

using AIIcsoftAPI.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Data;
using Microsoft.Data.SqlClient;
using AIIcsoftAPI.HelperClasses;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using AIIcsoftAPI.Enums;



namespace AIIcsoftAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SMDBIcsoftMainContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
       


        public AuthenticationService(SMDBIcsoftMainContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<ServiceResponse<string>> AuthenticateUsingAPItoken(string token)
        {
            string emailid;
            ServiceResponse<string> serviceResponse;

            if (string.IsNullOrEmpty(token))
            {
                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid token", Success = false };
                return serviceResponse;
            }
            try
            {
                var TokenInfo = new Dictionary<string, string>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims.ToList();

                foreach (var claim in claims)
                {
                    TokenInfo.Add(claim.Type, claim.Value);
                }

                emailid = TokenInfo["emailid"].ToString();
                string userid = TokenInfo["userid"].ToString();

                string ConStr = _configuration.GetSection("ConnectionStrings:IcsoftConnection").Value;
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandText = "employee_getByEmailId";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("pemail", emailid);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.ReadAsync().Wait();
                        long empid = Convert.ToInt64(dr["empid"]);
                        string p = dr["rowid"].ToString();

                        LoginDto loginDto = new LoginDto();
                        loginDto.Email = emailid;
                        loginDto.Password = p;
                        //not required to generate sah512 as password will be in sha512 only.


                        serviceResponse = await AuthenticateClient(loginDto);
                        if (serviceResponse.Success)
                        {
                            string createdtoken = serviceResponse.Data;
                            if (createdtoken != null && createdtoken.Equals(token))
                            {
                                return serviceResponse;
                            }
                            else
                            {
                                throw new Exception("Invalid Token");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Token");

                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Token");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Data = "", Code = 0, Message = ex.Message, Success = false };
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<string>> AuthenticateClient(LoginDto obj)
        {
            //string y = Helper.Encrypt(obj.Password);
            string e = obj.Email;
            //string p = Helper.Decrypt(obj.Password);
            string p = obj.Password;
            string externalapiusername = obj.Username;


            var response = new ServiceResponse<string>();

            var employee = await _context.Employees.FirstOrDefaultAsync(t => t.EmailId.ToLower() == e.ToLower() && t.Resign == "N");

            //ServiceResponse<EmployeeDTO> serviceResponse = await GetEmployeeByEmailId(e);
            //EmployeeDTO employee = serviceResponse.Data;

            if (employee is null)
            {
                response.Success = false;
                response.Message = "Error: Invalid Username or Pasword";
            }
            else
            {
                string userid = employee.EmpId.ToString();
                //code will execute for new user
                if (employee.PasswordHash == null || employee.PasswordHash.Length <= 0 || employee.PasswordSalt == null || employee.PasswordSalt.Length <= 0)
                {
                    CreatePasswordHashHMACSHA256(p, out byte[] passwordHash, out byte[] passwordSalt);
                    employee.PasswordHash = passwordHash;
                    employee.PasswordSalt = passwordSalt;
                    await _context.SaveChangesAsync();
                }

                if (VerifyPasswordHashHMACSHA256(p, employee.PasswordHash, employee.PasswordSalt))
                {
                    response.Success = true;
                    //response.Data = CreateToken(employee, externalapiusername);
                    response.Data = CreateClientToken256(userid, e, employee);        //e stores emailid
                }
                else
                {
                    if (VerifyPasswordHashHMACSHA512(p, employee.PasswordHash, employee.PasswordSalt))
                    {
                        response.Success = true;
                        //response.Data = CreateIcSoftToken256(employee, externalapiusername);
                        response.Data = CreateClientToken256(userid, e, employee);
                    }
                    else
                    {
                       response.Success = true;
                        //response.Data = CreateIcSoftToken256(employee, externalapiusername);
                        response.Data = CreateClientToken256(userid, e, employee);
                    }
                }
            }

            return response;
        }


        //used for directing from operations module to finance module
        public async Task<ServiceResponse<string>> sAuthenticateClient(string token)
        {
            string emailid, userid;
            ServiceResponse<string> serviceResponse;

            if (string.IsNullOrEmpty(token))
            {
                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid token", Success = false };
                return serviceResponse;
            }
            try
            {
                var TokenInfo = new Dictionary<string, string>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims.ToList();

                foreach (var claim in claims)
                {
                    TokenInfo.Add(claim.Type, claim.Value);
                }

                string issuer = TokenInfo["iss"].ToString();
                emailid = TokenInfo["emailid"].ToString();
                //userid = TokenInfo["userid"].ToString();
                if (issuer == _configuration.GetSection("AppSettings:jwtIssuerclient").Value)
                {
                    int count = _context.Employees.Where(x => x.EmailId == emailid).Count();
                    if (count > 0)
                    {
                        serviceResponse = new ServiceResponse<string>() { Data = token, Code = 0, Message = "valid token", Success = true };
                    }
                    else
                    {
                        serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid etoken", Success = false };
                    }

                }
                else
                {
                    serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid itoken", Success = false };
                }
            }
            catch (Exception ex)
            {
                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid itoken", Success = false };
            }

            return serviceResponse;
        }
        private string? GetDataBaseNames(DataBaseType dataBaseType)
        {
            var databaseNames = _configuration.GetSection("DatabaseNames");

            string dbname = dataBaseType switch
            {
                DataBaseType.ICSOFT => databaseNames["IcSoft"],
                DataBaseType.BIZSOFT => databaseNames["BizSoft"],
                DataBaseType.REPORT => databaseNames["Report"],
                DataBaseType.LEDZER => databaseNames["Ledger"],
                _ => string.Empty
            };
            return dbname;
        }


        #region general methods

        ////to validate Authorization token claims before calling api
        public bool ValidateTokenClaims(int papicallerempid, string papicalleremailid)
        {
            string? ConStr = _configuration.GetSection("ConnectionStrings:IcsoftConnection").Value;

            ////use this code only when you are using single email id for accessing api not for all registered employees having email and password.
            //string? apiemailid = _configuration.GetSection("AppSettings:apiemailid").Value;
            //if (papicalleremailid.Equals(apiemailid) == false)
            //{
            //    return false;
            //}

            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();

                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "select count(*) from "+ GetDataBaseNames(DataBaseType.ICSOFT) + ".dbo.employee where empid=@empid and emailid=@emailid";
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@empid", papicallerempid);
                    cmd.Parameters["@empid"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@emailid", papicalleremailid);
                    cmd.Parameters["@emailid"].Direction = ParameterDirection.Input;

                    int recordcount = Convert.ToInt32(cmd.ExecuteScalar());

                    if (recordcount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }


        private string CreateOurToken(string emailid, string block, int empid, string loginname)
        {
            string role = block;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, empid.ToString()),
                new Claim(ClaimTypes.Name, loginname),
                new Claim(ClaimTypes.Email, emailid),
                new Claim(ClaimTypes.Role, role)
            };

            double tokenExpirationMinutes = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationMinutes").Value);
            //double tokenExpirationDays = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationDays").Value);
            //double tokenExpirationHours = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationHours").Value);
            //var appSettingsSecretkey = _configuration.GetSection("AppSettings:Secretkeyclient").Value;
            //var appSettingsIssuer = _configuration.GetSection("AppSettings:jwtIssuerclient").Value;

            var appSettingsSecretkey = _configuration.GetSection("AppSettings:Secretkey").Value;
            var appSettingsIssuer = _configuration.GetSection("AppSettings:jwtIssuer").Value;

            if (appSettingsSecretkey == null)
            {
                throw new Exception("appsettings token in null");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsSecretkey));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = appSettingsIssuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(tokenExpirationMinutes),
                //Expires = DateTime.Now.AddHours(tokenExpirationHours),
                //Expires = DateTime.Now.AddDays(tokenExpirationDays),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// ////
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<string>> CreateOurTokenfromZvolvToken(string token)
        {
            string emailid, userid;
            ServiceResponse<string> serviceResponse;

            if (string.IsNullOrEmpty(token))
            {
                serviceResponse= new ServiceResponse<string>() { Code = 0, Message = "invalid token", Success = false };
                return serviceResponse;
            }
            try
            {
                var TokenInfo = new Dictionary<string, string>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims.ToList();

                foreach (var claim in claims)
                {
                    TokenInfo.Add(claim.Type, claim.Value);
                }

                string issuer = TokenInfo["iss"].ToString();
                if (issuer == _configuration.GetSection("AppSettings:jwtIssuerclient").Value)
                {
                    emailid = TokenInfo["emailid"].ToString();
                    userid = TokenInfo["userid"].ToString();

                    string ConStr = _configuration.GetSection("ConnectionStrings:IcsoftConnection").Value;
                    using (SqlConnection con = new SqlConnection(ConStr))
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandText = "employee_getByEmailId";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("pemail", emailid);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.ReadAsync().Wait();
                            int empid = Convert.ToInt32(dr["empid"]);
                            string loginname = dr["loginname"].ToString();
                            string block = dr["block"].ToString();
                            //string p = dr["rowid"].ToString();
                            //string p = "Secret#2023!$";

                            string ourtoken = CreateOurToken(emailid, block, empid, loginname);

                            if(string.IsNullOrEmpty(ourtoken))
                            {
                                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "cannot create token", Success = false };
                            }
                            else
                            {
                                serviceResponse = new ServiceResponse<string>() { Data = ourtoken, Code = 0, Message = "token generated successfully", Success = true };
                            }
                        }
                        else
                        {
                            serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid emailid", Success = false };
                        }
                    }
                }
                else
                {
                    serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid issuer", Success = false };
                }
            }
            catch (Exception ex)
            {
                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = ex.Message, Success = false };
            }

            return serviceResponse;
        }


        public static void CreatePasswordHashHMACSHA256(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHashHMACSHA256(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (computedHash.SequenceEqual(passwordHash));
            }
        }

        private bool VerifyPasswordHashHMACSHA512(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (computedHash.SequenceEqual(passwordHash));
            }
        }

        public async Task<ServiceResponse<string>> ValidateToken(string token)
        {
            string emailid;
            ServiceResponse<string> serviceResponse;

            if (string.IsNullOrEmpty(token))
            {
                serviceResponse = new ServiceResponse<string>() { Code = 0, Message = "invalid token", Success = false };
                return serviceResponse;
            }
            try
            {
                var TokenInfo = new Dictionary<string, string>();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                var claims = jwtSecurityToken.Claims.ToList();

                foreach (var claim in claims)
                {
                    TokenInfo.Add(claim.Type, claim.Value);
                }

                emailid = TokenInfo["emailid"].ToString();
                string userid = TokenInfo["userid"].ToString();

                string ConStr = _configuration.GetSection("ConnectionStrings:IcsoftConnection").Value;
                using (SqlConnection con = new SqlConnection(ConStr))
                {
                    con.Open();
                    try
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandText = "employee_getByEmailId";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("pemail", emailid);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.ReadAsync().Wait();
                            long empid = Convert.ToInt64(dr["empid"]);
                            string p = dr["rowid"].ToString();

                            LoginDto loginDto = new LoginDto();
                            loginDto.Email = emailid;
                            loginDto.Password = p;
                            //not required to generate sah512 as password will be in sha512 only.


                            serviceResponse = await AuthenticateClient(loginDto);
                            dr.CloseAsync();
                            if (serviceResponse.Success)
                            {
                                return serviceResponse;
                            }
                            else
                            {
                                throw new Exception("Invalid Token");

                            }
                        }
                        else
                        {
                            throw new Exception("Invalid Token");
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ServiceResponse<string>() { Data = "", Code = 0, Message = ex.Message, Success = false };
                    }
                    finally { con.Close(); }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>() { Data = "", Code = 0, Message = ex.Message, Success = false };
            }
        }

        private string CreateClientToken256(string userid, string emailid, Employee employee)
        {
            var claims = new List<Claim>
            {
                //taken from createtoken method
                new Claim(ClaimTypes.NameIdentifier, employee.EmpId.ToString()),
                new Claim(ClaimTypes.Name, employee.EmpName),
                new Claim(ClaimTypes.Email, employee.EmailId),
                //new Claim(ClaimTypes.GivenName, externalapiusername),
                new Claim(ClaimTypes.Role, employee.Block)

                //below is Zvolv original claim
                //new Claim("userid", userid),
                ////new Claim(ClaimTypes.Name, employee.LoginName),
                ////new Claim(ClaimTypes.GivenName, externalapiusername),
                ////new Claim(ClaimTypes.Role, role),
                //new Claim("emailid", emailid),
                //new Claim("orgzviceid", "3000279084")
            };



            //double tokenExpirationDays = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationDays").Value);
            //double tokenExpirationHours = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationHours").Value);


            double tokenExpirationMinutes = Convert.ToDouble(_configuration.GetSection("AppSettings:tokenExpirationMinutes").Value);

            var appSettingsSecretkey = _configuration.GetSection("AppSettings:Secretkeyclient").Value;
            var appSettingsIssuer = _configuration.GetSection("AppSettings:jwtIssuerclient").Value;

            if (appSettingsSecretkey == null)
            {
                throw new Exception("appsettings token in null");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsSecretkey));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            //SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = appSettingsIssuer,
                Expires = DateTime.Now.AddMinutes(tokenExpirationMinutes),
                Subject = new ClaimsIdentity(claims),
                //Expires = DateTime.Now.AddHours(tokenExpirationHours),
                //Expires = DateTime.Now.AddDays(tokenExpirationDays),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        public bool GetEmpIdEmailIdFromToken(string token, ref int empid, ref string emailid)
        {
            var TokenInfo = new Dictionary<string, string>();

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();

            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            emailid = TokenInfo["emailid"].ToString();
            string userid = TokenInfo["userid"].ToString();

            string ConStr = _configuration.GetSection("ConnectionStrings:IcsoftConnection").Value;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "employee_getByEmailId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("pemail", emailid);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.ReadAsync().Wait();
                    empid = Convert.ToInt32(dr["empid"]);
                    return true;
                }
                else
                {
                    empid = 0;
                    return false;
                }
            }
        }

        public async Task<ServiceResponse<EmployeeDTO>> GetEmployeeByEmailId(string emailid)
        {
            string ConStr;
            SqlConnection con;
            DataTable dt = null;
            EmployeeDTO empdto = null;
            ServiceResponse<EmployeeDTO> serviceResponse = new ServiceResponse<EmployeeDTO>();

            DBConfig db = new DBConfig();
            ConStr = db.GetMainConnectionString();

            try
            {
                con = new SqlConnection(ConStr);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "employee_getByEmailId";
                cmd.Parameters.AddWithValue("@pemail", emailid);
                cmd.Parameters["@pemail"].Direction = ParameterDirection.Input;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();

                da.Fill(dt);

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    empdto = new EmployeeDTO();
                    empdto.EmpId = Convert.ToInt32(dt.Rows[0]["empid"]);
                    empdto.empCode = dt.Rows[0]["empcode"].ToString();
                    empdto.empName = dt.Rows[0]["empname"].ToString();
                    empdto.LoginName = dt.Rows[0]["loginname"].ToString();
                    empdto.EmailId = dt.Rows[0]["emailid"].ToString();
                    empdto.Block = dt.Rows[0]["block"].ToString();
                    empdto.PasswordHash = (byte[])dt.Rows[0]["b1"];
                    empdto.PasswordSalt = (byte[])dt.Rows[0]["b2"];


                    serviceResponse.Data = empdto;
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Record fetched successfully";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Data = empdto;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                serviceResponse.Code = 611;
            }

            return serviceResponse;
        }

        #endregion



    }
}
