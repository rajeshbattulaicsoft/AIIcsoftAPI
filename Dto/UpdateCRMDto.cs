namespace AIIcsoftAPI.Dto
{
    public class UpdateCRMDto
    {
       
       
        public int cmrno { get; set; }
       // public DateOnly cmrdate { get; set; }
     //   public int custid { get; set; }
        public int transporterid { get; set; }
       
        public string gateentryrefno { get; set; }
        public string invoiceno { get; set; }
        public DateOnly invoicedate { get; set; }
        public string dcno { get; set; }
       // public DateOnly dcdate { get; set; }
        public string lrno { get; set; }
        public DateOnly lrdate { get; set; }
        public int pcid { get; set; }
        public string description { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        public DateTime entrydatetime { get; set; }
       
        public List<CMRGridDataUpdate> griddata { get; set; }
    }

    public class CMRGridDataUpdate
    {
        public int cmrid { get; set; }
        public decimal quantity { get; set; }

       
    }

   
}

