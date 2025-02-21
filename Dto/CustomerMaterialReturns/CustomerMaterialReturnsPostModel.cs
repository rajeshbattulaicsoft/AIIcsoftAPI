namespace AIIcsoftAPI.Dto.CustomerMaterialReturns
{
    public class CustomerMaterialReturnsPostModel
    {
        public required int custid { get; set; }
       
        public  string refno { get; set; }
        public  DateOnly returndate { get; set; }
        public required int pcid { get; set; }
        public required int loginid { get; set; }
        public required int entryempid { get; set; }
        public required string entrycomputer { get; set; }
        public required string companyid { get; set; }
        public int warehouseid {get;set;}
        public List<CmrrGridData> griddata { get; set; }
    }
    public class CmrrGridData
    {
        public required string batterypartname { get; set; }
        public required string uom { get; set; }
        public required string batterysrno { get; set; }
        public required int grnid { get; set; }
        public  DateOnly grndate { get; set; }
        public required int rawmatid { get; set; }
        public int grnSerialNoId {get;set;}
        public int storeentryid {get;set;}
        public required decimal stockquantity { get; set; }
        public required decimal issuequantity { get; set; }
    }
}
