namespace AIIcsoftAPI.DAL
{
    public interface IGenericRepository
    {
        Task<ServiceResponse<T>> ExecuteStoredProcedureAsync<T>(
                string storedProcedureName,
                Dictionary<string, object> inputParams,
                string outputParamName,
                int outputParamSize = 500);
        Task<ServiceResponse<string>> GetResponseAsync(bool status, string operation, string entity, string data);

    }
}
