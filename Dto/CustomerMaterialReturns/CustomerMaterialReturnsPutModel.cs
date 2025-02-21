namespace AIIcsoftAPI.Dto.CustomerMaterialReturns
{
    public class CustomerMaterialReturnsPutModel
    {
        public int dcid { get; set; }
        public required int custid { get; set; }
       // public required string custname { get; set; }
        public required string refno { get; set; }
        public required DateOnly returndate { get; set; }
        public required int pcid { get; set; }
        public required int loginid { get; set; }
        public required int entryempid { get; set; }
        public required string entrycomputer { get; set; }
        public required int companyid { get; set; }
        public List<CmrrUpdateGridData> griddata { get; set; }
    }
    public class CmrrUpdateGridData
    {
        public required string batterypartname { get; set; }
        public required string uom { get; set; }
        public required string batterysrno { get; set; }
       
        public required int grnno { get; set; }
        public required DateOnly grndate { get; set; }
        public required decimal stockquantity { get; set; }
        public required decimal issuequantity { get; set; }
    }
}
