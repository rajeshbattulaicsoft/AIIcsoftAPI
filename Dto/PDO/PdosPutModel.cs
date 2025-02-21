namespace AIIcsoftAPI.Dto.PDO
{
    public class PdosPutModel 
    {
        public string companyid { get; set; }
        public required int grnno { get; set; }
        public required int warehouseid { get; set; }
        public required int pcid { get; set; }
        public required string pdorefno { get; set; }
        public required DateTime pdorefdate { get; set; }
        public required int loginid { get; set; }
        public required int entryempid { get; set; }
        public required string entrycomputer { get; set; }
        public required string description { get; set; }
        public required int jobcardid { get; set; }
        public List<PdoUpdateGridData> griddata { get; set; }
    }
    public class PdoUpdateGridData
    {
        public int rawmatid { get; set; }
        public decimal quantity { get; set; }
        public string? uom { get; set; }
        public string? referenceno { get; set; }
        public DateTime pdoentrydate { get; set; }
        public string materialsrno {get;set;}
        public string warrentytype {get;set;}
        public int rejectiontype {get;set;}
        public int reasonid {get;set;}
        public int gridwarehouseid {get;set;}
        public string supplier {get;set;}
        public int qacode {get;set;}
    }
}
