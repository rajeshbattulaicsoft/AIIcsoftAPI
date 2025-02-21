namespace AIIcsoftAPI.Dto
{
    public class PreInspectionApprovaInsertDto
    {
        public int companyid { get; set; }
        public int approvedby { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        public DateTime entrydatetime { get; set; }
        public List<PreInspectionApproval> griddata { get; set; }
    }
    public class PreInspectionApproval
    {
        public int inspid { get; set; }
        public int cmrid { get; set; }

        public int serialnoid { get; set; }
        public string? reverifycomments { get; set; }
       
    }
}
