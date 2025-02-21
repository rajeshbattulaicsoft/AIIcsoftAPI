namespace AIIcsoftAPI.Dto.JobCardCreations
{
    public class JobCardCreationsPostModel
    {
        public string? jobcardtype { get; set; }
        public string? companyid { get; set; }
        public string? refno { get; set; }
        public DateOnly? refdate { get; set; }
        public int? rawmatid { get; set; }
       
        public int? pcid { get; set; }
        public int? entryempid { get; set; }
        public string? entrycomputer { get; set; }
        public DateTime? entrydatetime { get; set; }
        public string? serialno { get; set; }

        public string? warrenty { get; set; }
        public int? warehouseid { get; set; }
        public int? repairprocessid { get; set; }
        public int? createdby { get; set; }
        public decimal? quotprice { get; set; }
        public decimal? estimatedhours { get; set; }
        public int? reasonid { get; set; }
        public int? deptid { get; set; }
        public string narration{get;set;}

        public int inventsrid {get;set;}
        public List<JobCardGridData> griddata { get; set; }
    }
    public class JobCardGridData
    {
        public int? rawmatid { get; set; }
        public string? replaceitem { get; set; }
        public string? warrentyitem { get; set; }
        public string? serialnumber{get;set;}
        public decimal bomquantity{get;set;}
        public string uom{get;set;}
         public DateTime? replacedate {get;set;}
        public int? replacerawmatid { get; set; }
        public decimal? replaceqty { get; set; }
        public string? replacereason { get; set; }
        public decimal? avlstock { get; set; }
        public string bomconsumed{get;set;}
    }
}
