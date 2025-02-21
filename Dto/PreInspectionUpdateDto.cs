namespace AIIcsoftAPI.Dto
{
    public class PreInspectionUpdateDto
    {
        public string companyid { get; set; }
        public int cmrno { get; set; }
        public string insprefno { get; set; }
        public DateTime inspdate { get; set; }
        public int profitcenterid { get; set; }
        public int createdby { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        //public DateTime entrydatetime { get; set; }
         public int approvedby { get; set; }
        public List<PreInspectionUpdate> griddata { get; set; }
    }
    public class PreInspectionUpdate
    {
        public int inspid { get; set; }
        public int cmrid { get; set; }
        public int rawmatid { get; set; }
        public decimal quantity { get; set; }
        public string uom { get; set; }
         public int repairtypeid{get;set;}
        public string warrentytype { get; set; }
        public string serialno { get; set; }
    }

}
