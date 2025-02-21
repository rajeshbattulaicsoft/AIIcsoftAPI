namespace AIIcsoftAPI.Dto.Battery
{
    public class BatteryCellPostModel
    {
        public required string assetType { get; set; }

        public required string createdBy { get; set; }

        public required CellAssetData assetData { get; set; }
    }
    public class BatteryBmcuPostModel
    {
        public required string assetType { get; set; }

        public required string createdBy { get; set; }

        public required BmcuAssetData assetData { get; set; }
    }
    public class BatteryTiuPostModel
    {
        public required string assetType { get; set; }

        public required string createdBy { get; set; }

        public required TiuAssetData assetData { get; set; }
    }
    public class BatteryChogoriPostModel
    {
        public required string assetType { get; set; }

        public required string createdBy { get; set; }

        public required ChogoriAssetData assetData { get; set; }
    }

    public class StationPostModel
    {
        public required string assetType { get; set; }

        public required string createdBy { get; set; }

        public required CellAssetData assetData { get; set; }
    }

    public class CellAssetData
    {
        public required string OrderLineNumber { get; set; }
        public required string PartNumber { get; set; }
        public required string SpecId { get; set; }
        public required string SerialNumber { get; set; }
        public required string Class { get; set; }
        public required string ManufacturingDate { get; set; }
        public required string WarrantyExpiryDate { get; set; }
        public required double MeasuredVoltage { get; set; }
        public required double MeasuredResistance { get; set; }
        public required double CellDischargevalue { get; set; }
        public required double Capacity { get; set; }
        public required double CellChargevalue { get; set; }
        public required string Status { get; set; }
        public required bool IsAccepted { get; set; }
        public required string AcceptedBy { get; set; }
        public required string AcceptedDate { get; set; }
        public required string WorkOrderNumber { get; set; }
    }

    public class BmcuAssetData
    {
        public required string SerialNumber { get; set; }
        public required string FwVersion { get; set; }
        public required string BootloaderVersion { get; set; }
        public required string OrderLineNumber { get; set; }
        public required string CommunicationVersion { get; set; }
        public required string PartNumber { get; set; }
        public required string AcceptedBy { get; set; }
        public required string SpecId { get; set; }
        public required string Status { get; set; }
        public required bool IsAccepted { get; set; }
        public required string ManufacturingDate { get; set; }
        public required string WarrantyExpiryDate { get; set; }
        public required string AcceptedDate { get; set; }
        public required string WorkOrderNumber { get; set; }     
    }

    public class TiuAssetData
    {
        public required int OrderLineNumber { get; set; }
        public required string SpecId { get; set; }
        public required string PartNumber { get; set; }
        public required string SerialNumber { get; set; }
        public required string ManufacturingDate { get; set; }
        public required string WarrantyExpiryDate { get; set; }
        public required string BootloaderVersion { get; set; }
        public required string FwVersion { get; set; }
        public required string BLEMacAddress { get; set; }
        public required string IMEI { get; set; }
        public required string MobileNumber { get; set; }
        public required string CommunicationVersion { get; set; }
        public required string APN { get; set; }
        public required string Status { get; set; }
        public required bool IsAccepted { get; set; }
        public required string AcceptedBy { get; set; }
        public required string AcceptedDate { get; set; }
        public required string WorkOrderNumber { get; set; }
    }
    public class ChogoriAssetData
    {
        public required int OrderLineNumber { get; set; }        
        public required string PartNumber { get; set; }
        public required string SpecId { get; set; }
        public required string SerialNumber { get; set; }
        public required string ManufacturingDate { get; set; }
        public required string WarrantyExpiryDate { get; set; }        
        public required string Status { get; set; }
        public required bool IsAccepted { get; set; }
        public required string AcceptedBy { get; set; }
        public required string AcceptedDate { get; set; }
        public required string WorkOrderNumber { get; set; }
    }
}
