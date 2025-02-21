namespace AIIcsoftAPI.Dto
{
    public class ProductionDataDtoForSave
    {
        public string plantLocation {  get; set; }
        public string lineItem { get; set; }
        public string machine { get; set; }
        public string shift { get; set; }
        public string operatorName { get; set; }
        public string workorderNo { get; set; }
        public string batteryPartCode { get; set; }
        public string batterySerialNumber { get; set; }

        //public int location { get; set; }
        //public int entryempid { get; set; }
        //public string entrycomputer { get; set; }

        public List<CELL> cell {  get; set; }
        public List<TIU> tiu {  get; set; }
        public List<BMCU> bmcu {  get; set; }
        public List<CHOGORI> chogori {  get; set; }
    }

    public class CELL
    {
        public string partNumber { get; set; }
        public string serialNumber { get; set; }
    }

    public class TIU
    {
        public string partNumber { get; set; }
        public string serialNumber { get; set; }
        public string mobileNumber { get; set; }
    }

    public class BMCU
    {
        public string partNumber { get; set; }
        public string serialNumber { get; set; }
    }

    public class CHOGORI
    {
        public string partNumber { get; set; }
        public string serialNumber { get; set; }
    }

    public class StationProductionDataDtoForSave
    {
        public string plantLocation { get; set; }
        public string lineItem { get; set; }
        public string machine { get; set; }
        public string shift { get; set; }
        public string operatorName { get; set; }
        public string workorderNo { get; set; }
        public string StationPartCode { get; set; }
        public string StationSerialNumber { get; set; }        

        public List<Components> components { get; set; }        
    }

    public class Components
    {
        public string componentttype { get; set; }
        public string hardwareid { get; set; }
        public string serialNumber { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
    }
}
