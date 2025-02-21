namespace AIIcsoftAPI.Dto
{
    public class GateEntryUpdateDTO
    {
        public string companyid { get; set; }
        public int gateEntryNo { get; set; }
        public DateTime gateEntryDate { get; set; }
        public string vehicleNo { get; set; }
        public string gateEntry_Ref_No { get; set; }
        public int noOfPersons { get; set; }
        public string descriptions { get; set; }

        public int entryEmpId { get; set; }
        public string entryComputer { get; set; }
        public DateTime entrydateTime { get; set; }
        public List<Griddataupdate> gridData { get; set; }
    }

    public class Griddataupdate
    {

        public int? gateentryid { get; set; }
        public int? partyId { get; set; }
        public string? invoiceNo { get; set; }
        public string? dcNo { get; set; }
        public DateTime? invoiceDate { get; set; }
        public DateTime? dcDate { get; set; }
        public string? lrNo { get; set; }

        public DateTime? lrDate { get; set; }

        public string? poNo { get; set; }
        public string? outwardDcNo { get; set; }

        public string? partyname { get; set; }



    }
}


 