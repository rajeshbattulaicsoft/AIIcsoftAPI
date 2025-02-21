namespace AIIcsoftAPI.Dto
{
    public class CustomerMaterialRecieptSaveDto
    {
        public string companyid {  get; set; }
        public int cmrno { get; set; }
        public string cmrrefno { get; set; }
        public DateOnly cmrdate { get; set; }
        public int custid { get; set; }
        public int transporterid { get; set; }
        public string gateentryrefno { get; set; }
        public string invoiceno { get; set; }
        public DateOnly invoicedate { get; set; }
        public string dcno { get; set; }
        public DateOnly dcdate { get; set; }
        public string lrno { get; set; }
        public DateOnly lrdate { get; set; }
        public int pcid { get; set; }
        public string description { get; set;}
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        public DateTime entrydatetime { get; set; }
        public string addnlparameter { get; set; }
       
        public List<CMRGridData> griddata { get; set; }
    }

    public class CMRGridData
    {
        public int rawmatid { get; set; }
        public decimal quantity { get; set; }
        public string uom { get; set; }

        public List<modalgriddata> modalgriddata { get; set; }
    }

    public class modalgriddata
    {
        public string serialno { get; set; }
        public string frfno { get; set; }
    }
}
