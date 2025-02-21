using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AIIcsoftAPI.Models.ResponseModels
{
    public class GetMrsApprovalResponseModel
    {
        public required int SrNo { get; set; }
        public string? ReferenceNo { get; set; }

        public DateTime ReferenceDate { get; set; }
        public string? JobCardNumber { get; set; }
        public DateTime JobCardDate { get; set; }
        public string? Material { get; set; }

        public string? MaterialSno { get; set; }
        public string? ProfitCenter { get; set; }

    }
}
