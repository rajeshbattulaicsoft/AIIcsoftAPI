using Azure;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using AIIcsoftAPI.Models.SMIcsoftDataModels;
using System.Data;

namespace AIIcsoftAPI.DAL
{
    public class GenericRepository : IGenericRepository
    {
        private readonly SMDBIcsoftMainContext _context;

        public GenericRepository(SMDBIcsoftMainContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Dictionary<string, object> inputParams, string outputParamName, int outputParamSize = 500)
        {
            var response = new ServiceResponse<T>();
            try
            {
                var sqlParameters = inputParams.Select(kvp =>
                    new SqlParameter(kvp.Key, kvp.Value ?? DBNull.Value)).ToList();
                var outputParam = new SqlParameter(outputParamName, SqlDbType.VarChar, outputParamSize)
                {
                    Direction = ParameterDirection.Output
                };

                sqlParameters.Add(outputParam);
                var parameterPlaceholders = string.Join(", ", sqlParameters.Select(p => "@" + p.ParameterName));
                var sql = $"EXEC {storedProcedureName} {parameterPlaceholders}";
                var result = await _context.Database.ExecuteSqlRawAsync(sql, sqlParameters.ToArray());
                var outputValue = outputParam.Value?.ToString() ?? "";
                //if (result > 0 && string.IsNullOrEmpty(outputValue))
                //{
                    response.Success = true;
                    response.Data = (T)Convert.ChangeType(outputValue, typeof(T))!;
                    response.Message = "Stored procedure executed successfully.";
                //}
                //else
                //{
                //    response.Success = false;
                //    response.Message = $"Stored procedure failed: {outputValue}";
                //}
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error executing stored procedure: {ex.Message}";
            }
            return response;
        }

        public async Task<ServiceResponse<string>> GetResponseAsync(bool status,string operation,string entity,string data)
        {
            var response = new ServiceResponse<string>();
            response.Data = data;
            response.Success = status;
            response.Message = $"{entity} {operation} {(status ? "successfully" : "Failed")}";

            return response;
        }

    }
}
