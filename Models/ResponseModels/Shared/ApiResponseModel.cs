namespace AIIcsoftAPI.Models.ResponseModels.Shared
{
    public class ApiResponseModel
    {
        public string? version { get; set; }
        public int statusCode { get; set; }
        public string? message { get; set; }
        public ResponseException? responseException { get; set; }
        public string? result { get; set; }

        public bool IsError { get; set; }
    }

    public class ResponseException
    {
        public string ExceptionMessage { get; set; }
    }
}
