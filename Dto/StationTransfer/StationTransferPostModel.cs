namespace AIIcsoftAPI.Dto.StationTransfer
{
    public class StationTransferPostModel
    {
        public required string stationSerialNumber { get; set; }

        public required string serviceLocation { get; set; }

        public required string legalEntity { get; set; }

        public required bool operationStatus { get; set; }

        public required string updatedBy { get; set; }
    }
}
