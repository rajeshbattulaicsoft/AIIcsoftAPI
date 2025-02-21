using AIIcsoftAPI.Dto.PDO;

namespace AIIcsoftAPI.Dto.StoreInward
{
    public class StoreInwardPostModel
    {
        public string companyid { get; set; }
        public DateOnly receiptdate { get; set; }
        public int? storeid { get; set; }
        public int? receivedby { get; set; }
        public int issuedby { get; set; }
        public int? entryempid { get; set; }
        public string? entrycomputer { get; set; }
        public string? addnlparameter { get; set; }
         public int? deptid { get; set; }
        public int? partyid {get;set;}

        public List<storeinwraddata> griddata { get; set; }
    }

    public class storeinwraddata
    {
        public int? jobcardid { get; set; }
        public int rawmatid { get; set; }
        public string serialno { get; set;}
        public decimal quantity { get; set; }

        public string uom { get; set; }
    }

}
