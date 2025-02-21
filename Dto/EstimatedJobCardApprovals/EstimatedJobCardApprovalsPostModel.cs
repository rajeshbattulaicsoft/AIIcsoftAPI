namespace AIIcsoftAPI.Dto.EstimatedJobCardApprovals
{
    public class EstimatedJobCardApprovalsPostModel
    {
        public required int autoid { get; set; }
        public required int ApprovedBy { get; set; }
        public required string IsApprovedRejected { get; set; }
        public string? Remarks { get; set; }

        public required string ApprovedComputer { get; set; }
    }
}
