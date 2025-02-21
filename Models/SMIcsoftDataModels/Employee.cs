using System;
using System.Collections.Generic;

namespace AIIcsoftAPI.Models.SMIcsoftDataModels;

public partial class Employee
{
    /// <summary>
    /// Employee Id
    /// </summary>
    public decimal EmpId { get; set; }

    /// <summary>
    /// Employee Code
    /// </summary>
    public string EmpCode { get; set; } = null!;

    /// <summary>
    /// Employee Name
    /// </summary>
    public string EmpName { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PinCode { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Employee Join Date
    /// </summary>
    public DateTime? JoinDate { get; set; }

    /// <summary>
    /// Department Id
    /// </summary>
    public decimal DeptId { get; set; }

    /// <summary>
    /// Employee Category Id
    /// </summary>
    public decimal CatId { get; set; }

    public string Attachment { get; set; } = null!;

    public string AttachPhoto { get; set; } = null!;

    public string BloodGroup { get; set; } = null!;

    public double BasicPay { get; set; }

    public string Gender { get; set; } = null!;

    /// <summary>
    /// Bank Account Number
    /// </summary>
    public string BankNo { get; set; } = null!;

    public string Wages { get; set; } = null!;

    /// <summary>
    /// Marital Status (Single/Married)
    /// </summary>
    public string MaritalStatus { get; set; } = null!;

    /// <summary>
    /// Resignation
    /// </summary>
    public string Resign { get; set; } = null!;

    public DateTime? ResignDate { get; set; }

    public decimal AccountId { get; set; }

    public decimal AccountHeadId { get; set; }

    public decimal AccExpenseId { get; set; }

    public string? LoginName { get; set; }

    public string? Password { get; set; }

    public string RunOnce { get; set; } = null!;

    public string StockReportReminder { get; set; } = null!;

    public string AdminAccess { get; set; } = null!;

    public decimal CardId { get; set; }

    public string Description { get; set; } = null!;

    public double? MaximumPovalue { get; set; }

    public decimal AccBonusAcId { get; set; }

    public DateTime? IncrementedDate { get; set; }

    public string DocCtrlPwd { get; set; } = null!;

    public string StockReportReminderForMg { get; set; } = null!;

    /// <summary>
    /// Designation Id
    /// </summary>
    public decimal Did { get; set; }

    public string Grade { get; set; } = null!;

    /// <summary>
    /// Qualification Id
    /// </summary>
    public decimal Qid { get; set; }

    /// <summary>
    /// Experience Id
    /// </summary>
    public decimal ExpId { get; set; }

    public string CompanyHead { get; set; } = null!;

    public string Authority { get; set; } = null!;

    public string Interviewed { get; set; } = null!;

    public string ReasonForLeaving { get; set; } = null!;

    public decimal CostCenter { get; set; }

    public DateTime? ConfirmationDate { get; set; }

    public string ShiftRotate { get; set; } = null!;

    public string Prapproval { get; set; } = null!;

    public string Porelease { get; set; } = null!;

    public string CardNo { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public string Paddress { get; set; } = null!;

    public string Block { get; set; } = null!;

    public string Cases { get; set; } = null!;

    public string OpPermission { get; set; } = null!;

    public string NacCode { get; set; } = null!;

    public decimal Absconding { get; set; }

    public string Etseadequate { get; set; } = null!;

    public string Etsecomments { get; set; } = null!;

    public string? ShiftRotationPattern { get; set; }

    public DateTime? ShiftRotationEndDate { get; set; }

    public decimal RotationInterval { get; set; }

    public string AutoAttReq { get; set; } = null!;

    public string PanNo { get; set; } = null!;

    public string Setters { get; set; } = null!;

    public string EmpAttDesc { get; set; } = null!;

    public string Ppserv { get; set; } = null!;

    public string MonthDayId { get; set; } = null!;

    public string SalOnWeeklyOff { get; set; } = null!;

    public string SalOnWeeklyOffGo { get; set; } = null!;

    public string WeekOffHourlyGo { get; set; } = null!;

    public string WeekOffHourly { get; set; } = null!;

    public string StandardHours { get; set; } = null!;

    public string Form5Remarks { get; set; } = null!;

    public string UnAvailGenHolPresent { get; set; } = null!;

    public string UnAvailGenHolAbsent { get; set; } = null!;

    public double ActualRent { get; set; }

    public decimal EntryEmpId { get; set; }

    public string EntryComputer { get; set; } = null!;

    public string Exclusiveuser { get; set; } = null!;

    public string WorkDaysBasedOnAbsent { get; set; } = null!;

    public string IsHandicapped { get; set; } = null!;

    public decimal AccExpenseSubId { get; set; }

    public decimal AccBonusSubId { get; set; }

    public string ReportUser { get; set; } = null!;

    public decimal UserGroupId { get; set; }

    public decimal LocationId { get; set; }

    public decimal MonthDay { get; set; }

    public string EmailPwd { get; set; } = null!;

    public decimal PayrollLocationId { get; set; }

    public decimal ServiceId { get; set; }

    public string Ifsccode { get; set; } = null!;

    public string BankBranchName { get; set; } = null!;

    public decimal ReportTo { get; set; }

    public string EarlierEmployerName { get; set; } = null!;

    public string EarlierEmployerLocation { get; set; } = null!;

    public string EarlierDesignation { get; set; } = null!;

    public DateOnly? EarlierWorkedFrom { get; set; }

    public DateOnly? EarlierWorkedTo { get; set; }

    public double EarlierSalaryDrawn { get; set; }

    public string EarlierReportingTo { get; set; } = null!;

    public string AttachmentForEarlierEmprDetails { get; set; } = null!;

    public string IsSrCitizen { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string SubIds { get; set; } = null!;

    public decimal Cityid { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public double Ctc { get; set; }

    public string ExFromPayRoll { get; set; } = null!;

    public string? SignatureImage { get; set; }

    public string EmailUserName { get; set; } = null!;

    public string IsContractEmp { get; set; } = null!;

    public string Rfid { get; set; } = null!;

    public DateOnly? RfideffFrom { get; set; }

    public DateOnly? RfideffTo { get; set; }

    public decimal ContractorTypeId { get; set; }

    public string ContBadgeNo { get; set; } = null!;

    public DateTime? StatusEffectiveDate { get; set; }

    public decimal StatusId { get; set; }

    public string Territory { get; set; } = null!;

    public string? MobileAppRegId { get; set; }

    public decimal MobileAppLocUpdateMins { get; set; }

    public string? Deptids { get; set; }

    public string DefaultEss { get; set; } = null!;

    public decimal AadharNo { get; set; }

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public string? Apistatus { get; set; }
}
