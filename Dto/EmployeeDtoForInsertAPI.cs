using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AIIcsoftAPI.Dto
{
    public class EmployeeDtoForInsertAPI
    {
        //public string EmpName { get; set; } = null!;

        //public string? LoginName { get; set; }

        //public string? Password { get; set; }

        //[EmailAddress]
        //public string EmailId { get; set; }

        public string EmpCode { get; set; }
        public string EmpName { get; set; }

        public string? LoginName { get; set; }

        public string? Password { get; set; }

        [EmailAddress]
        public string EmailId { get; set; }
        public string LocationCode { get; set; }
        public string EntryEmployeeid { get; set; }
        public string EntryComputer { get; set; }




        //public string Block { get; set; } = null!;

        //public string EntryEmpId { get; set; } = null!;

        //public string EntryComputer { get; set; } = null!;

        //public byte[] PasswordHash { get; set; } = null;

        //public byte[] PasswordSalt { get; set; } = null;
    }
}
