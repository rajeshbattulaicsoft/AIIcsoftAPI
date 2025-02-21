using Microsoft.Identity.Client;

namespace AIIcsoftAPI.Dto.StoreIssues
{
    public class StoreIssuesPostModel
    {
        public int warehouseid { get; set; }
        public string companyid { get; set; }
        public DateOnly refdate { get; set; }
        public int pcid { get; set; }
        public int deptid { get; set; }
        public int storeid { get; set; }
        public int receivedby { get; set; }
        public int issuedby { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer { get; set; }
        public int jobcardid {get;set;}

        public List<Storeissuegriddata> griddata { get; set; }
    }

    public class Storeissuegriddata()
    {
        public int rawmatid { get; set; }
        public int srid { get; set; }
        public string issserialno { get; set; }
        public int grnid { get; set; }
        public int storeentryid { get; set; }
        public decimal quantity { get; set; }
        public string uom {  get; set; }
        public int serialnoid {get;set;}
    }

}
