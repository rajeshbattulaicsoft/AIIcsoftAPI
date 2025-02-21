namespace AIIcsoftAPI.Dto
{
  public class SaveStoreAllocationDto
    {
        public string companyid {  get; set; }
        public DateTime alldate { get; set; }
        public int entryempid { get; set; }
        public string entrycomputer {  get; set; }
        public DateTime entrydatetime {get;set;}
        public int partyid {get;set;}
        public int whid { get; set; }
        public int rawmatid {get;set;}
        public int cmrid {get;set;}

        public decimal quantity {get;set;}

        public string refno {get;set;}
    }

}
