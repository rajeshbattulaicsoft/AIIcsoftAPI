namespace AIIcsoftAPI.Dto
{
    public class EmployeeDTO
    {
		public int EmpId { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string LoginName { get; set; }
        public string EmailId { get; set; }
        public string Block { get; set; }
        public byte[] PasswordHash { get; set; } = null;  //b1
        public byte[] PasswordSalt { get; set; } = null;  //b2
    }
}
