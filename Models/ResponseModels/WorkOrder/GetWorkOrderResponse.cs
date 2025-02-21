using Newtonsoft.Json;
namespace AIIcsoftAPI.Models.ResponseModels.GetWorkOrderResponse
{
    public class GetWorkOrderResponse
    {
        public string WorkOrderNumber { get; set; }

        public decimal WoQty { get; set; }

        public string? RawmatCode { get; set; }
        
        public int BpType { get; set; }
    }
}