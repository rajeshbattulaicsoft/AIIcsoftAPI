namespace AIIcsoftAPI.Dto
{
    public class GateEntryUpdateOutwardDTO
    {
        public string? companyid { get; set; }
        public int? gateEntryNo { get; set; }
        public DateTime? gateEntryDate { get; set; }
        public string? vehicleNo { get; set; }
        public string? gateEntry_Ref_No { get; set; }
        public int? noOfPersons { get; set; }
        public string? descriptions { get; set; }
        public int? entryEmpId { get; set; }
        public string? entryComputer { get; set; }
        public DateTime? entrydateTime { get; set; }


        public List<GriddataupdateOutward> gridData { get; set; }
    }

    public class GriddataupdateOutward
    {

        public int? gateentryid { get; set; }
        public int? partyId { get; set; }
        public string? invoiceNo { get; set; }
      
        public DateTime? invoiceDate { get; set; }
       
        public string? lrNo { get; set; }

        public DateTime? lrDate { get; set; }


        public string? partyname { get; set; }



    }
}

