namespace AIIcsoftAPI.Dto.VehicleDock
{
    public class VehicleDockPostModel
    {
        public required string SerialNumber { get; set; }

        public required bool IsMaster { get; set; }

        public required string createdBy { get; set; }
    }
}
