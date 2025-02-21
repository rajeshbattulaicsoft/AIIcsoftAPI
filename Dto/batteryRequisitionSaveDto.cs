namespace AIIcsoftAPI.Dto
{
    public class batteryRequisitionSaveDto
    {
        public string companyid { get; set; }
        public int deptid { get; set; }
        public int catid{get;set;}
        public int reqno { get; set; }
        public string reqrefno { get; set; }
        public DateOnly reqdate { get; set; }
        public int empid { get; set; }
        public int pcid { get; set; }
        public string description { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        public DateTime entrydatetime { get; set; }
        public List<GridDataDTO> griddata { get; set; }
    }
    // DTO class for the GridData section
    public class GridDataDTO
    {
       // public int reqid { get; set; }
        public int rawmatid { get; set; }
        public decimal quantity { get; set; }
        public string uom { get; set; }
        public decimal availablestock {get;set;}
        public int serialNumberid {get;set;}
    }
}
