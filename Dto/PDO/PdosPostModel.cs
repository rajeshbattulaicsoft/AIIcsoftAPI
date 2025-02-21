using AIIcsoftAPI.Dto.JobCardCreations;

namespace AIIcsoftAPI.Dto.PDO
{
    public class PdosPostModel
    {
        public string companyid { get; set; }
        public int? warehouseid { get; set; }
        public int? pcid { get; set; }
        public string? pdorefno { get; set; }
        public DateOnly pdorefdate { get; set; }
        public int? loginid { get; set; }
        public int? entryempid { get; set; }
        public string? entrycomputer { get; set; }
        public string? description { get; set; }
        public int jobcardid { get; set; }
        public List<PdoGridData> griddata { get; set; }
    }
    public class PdoGridData
    {
        public int rawmatid { get; set; }
        public decimal quantity { get; set; }
        public string? uom { get; set; }
        public string? referenceno { get; set; }
        public DateOnly pdoentrydate { get; set; }
        public string materialsrno {get;set;}
        public string warrentytype {get;set;}
        public int rejectiontype {get;set;}
        public int reasonid {get;set;}
        public int gridwarehouseid {get;set;}
        public int supplier {get;set;}
        public int qacode {get;set;}
    }
}
