namespace AIIcsoftAPI.Dto.StationComponent
{
    public class StationComponentPutModel
    {
        public required string stationSerialNumber { get; set; }

        public required string componentType { get; set; }

        public required string hardwareId { get; set; }

        public required string serialNumber { get; set; }

        public required string manufacturer { get; set; }

        public required string model { get; set; }

        public required string updatedBy { get; set; }
    }
}
