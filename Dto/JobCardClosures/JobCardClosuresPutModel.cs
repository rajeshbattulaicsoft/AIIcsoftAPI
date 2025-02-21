﻿namespace AIIcsoftAPI.Dto.JobCardClosures
{
    public class JobCardClosuresPutModel : JobCardClosuresPostModel
    {
        public int autoid { get; set; }
        public string? companyid { get; set; }
        public string? closurerefno { get; set; }
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

        public decimal? estimatedhours { get; set; }
        public int? reasonid { get; set; }
        public int? deptid { get; set; }

        public List<JobCardClouserGridData> griddata { get; set; }
    }
    public class JobCardClouserGridData
    {
        public int autoid { get; set; }
        public required string qualityapproved { get; set; }
       // public  decimal repaircost { get; set; }
        public  decimal servicehrs { get; set; }
        public  DateTime closeddate { get; set; }
    }
}
