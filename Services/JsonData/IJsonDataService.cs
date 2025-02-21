namespace AIIcsoftAPI.Services.JsonData
{
    public interface IJsonDataService
    {
        Task<string> GetJsonDataAsync(string dataType);
    }
}
