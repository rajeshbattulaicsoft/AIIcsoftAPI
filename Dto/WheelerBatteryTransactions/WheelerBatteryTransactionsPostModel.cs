using AIIcsoftAPI.Dto.Shared;

namespace AIIcsoftAPI.Dto.WheelerBatteryTransactions
{
    public class WheelerBatteryTransactionsPostModel : JvcoBaseModel
    {
        public required string batterySerialNumber { get; set; }

        public required string requesterName { get; set; }

        public required string toZone { get; set; }

        public required string toSubZone { get; set; }

        public required string fromSubZone { get; set; }

        public required string fromZone { get; set; }
    }
    public class ZoneTransferPostModel : JvcoBaseModel
    {
        public required string batterySerialNumber { get; set; }

        public required string requesterName { get; set; }

        public required string toZone { get; set; }

        public required string toSubZone { get; set; }

        public required string fromSubZone { get; set; }

        public required string fromZone { get; set; }

        public required bool deployedStatus { get; set; }
    }
    public class StationTransactionsPostModel : JvcoBaseModel
    {
        public required string stationSerialNumber { get; set; }

        public required string zone { get; set; }

        public required string servicelocation { get; set; }
    }
}
