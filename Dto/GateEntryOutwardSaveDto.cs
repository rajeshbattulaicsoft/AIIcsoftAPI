namespace AIIcsoftAPI.Dto
{
    public class GateEntryOutwardSaveDto
    {
        public string companyid { get; set; }
        public DateTime gateEntryDate { get; set; }
        public string vehicleNo { get; set; }
        public string gateEntry_Ref_No { get; set; }
        public int noOfPersons { get; set; }
        public string descriptions { get; set; }
        public int entryEmpId { get; set; }
        public string entryComputer { get; set; }
        public DateTime entrydateTime { get; set; }
        public string addlnParameter { get; set; }
        public string inWard { get; set; }

        public List<GriddataOutward> gridData { get; set; }


    }
    public class GriddataOutward
    {
        public int partyId { get; set; }
        public string? invoiceNo { get; set; }
        public DateTime? invoiceDate { get; set; }
        public string lrNo { get; set; }
        public DateTime? lrDate { get; set; }
        public string partyname { get; set; }



    }
}

