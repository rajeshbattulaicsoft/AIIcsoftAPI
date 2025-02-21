namespace AIIcsoftAPI.Dto.MRSApprovals
{
    public class MrsApprovalsPostModel
    {
        public required int SrNo { get; set; }
        public required int ApprovedBy { get; set; }
        public required string IsApprovedRejected { get; set; }

        public string? Remarks { get; set; }
    }
}
