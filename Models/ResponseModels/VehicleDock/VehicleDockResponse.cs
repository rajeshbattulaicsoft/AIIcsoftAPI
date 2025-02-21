namespace AIIcsoftAPI.Models.ResponseModels.VehicleDock
{
    public class VehicleDockResponse
    {
        public string? version { get; set; }
        public int statusCode { get; set; }
        public string? message { get; set; }
        public string? responseException { get; set; }
        public string? result { get; set; }
    }
}
