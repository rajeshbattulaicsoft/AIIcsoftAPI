using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AIIcsoftAPI.Dto
{
    public class EmployeeDtoForUpdateAPI
    {
        //public string EmpName { get; set; } = null!;
        //public string? Password { get; set; }

        public string EmpName { get; set; } = null!;
        //  public string? Password { get; set; }
        public string? LoginName { get; set; }

        [EmailAddress]
        public string EmailId { get; set; }
        public string LocationCode { get; set; }
        public string EntryEmployeeid { get; set; }
        public string EntryComputer { get; set; }
    }
}
