using Microsoft.AspNetCore.Mvc;

namespace AIIcsoftAPI.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public int Code { get; set; } = 0;
    }
}
