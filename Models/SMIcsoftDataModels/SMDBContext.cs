using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AIIcsoftAPI.Models.SMIcsoftDataModels;

public partial class SMDBContext : DbContext
{
    public SMDBContext()
    {
    }

    public SMDBContext(DbContextOptions<SMDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLevel> AccessLevels { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<InventSr> InventSrs { get; set; }

    public virtual DbSet<InventSrmaterial> InventSrmaterials { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLevel>(entity =>
        {
            entity.HasKey(e => e.AccessLevelId).HasName("pk_AccessLevel_AccessLevelID");

            entity.ToTable("AccessLevel", tb =>
                {
                    tb.HasTrigger("Trg_AccessLevel_Changes");
                    tb.HasTrigger("del_AccessLevel");
                });

            entity.HasIndex(e => new { e.Loginid, e.LocationId, e.FormId }, "NCI_AccessLevel_1");

            entity.Property(e => e.AccessLevelId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("AccessLevelID");
            entity.Property(e => e.EntryComputer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EntryDateTime)
                .HasDefaultValueSql("('')")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryEmpId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.FormAdd)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.FormDelete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.FormEdit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.FormId)
                .HasDefaultValueSql("('0')")
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("FormID");
            entity.Property(e => e.FormView)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.GroupId)
                .HasDefaultValueSql("('0')")
                .HasColumnType("numeric(9, 0)");
            entity.Property(e => e.Levelno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasComment("From which level like sales,genmaster,PPC")
                .HasColumnName("levelno");
            entity.Property(e => e.LocationId)
                .HasDefaultValueSql("('0')")
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Loginid)
                .HasComment("LoginIdNo")
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("loginid");
            entity.Property(e => e.Pcids)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PCIds");
            entity.Property(e => e.TabPages)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.ToTable("Employee", tb =>
                {
                    tb.HasTrigger("Trg_Employee_Changes");
                    tb.HasTrigger("del_Employee");
                });

            entity.HasIndex(e => e.DeptId, "NCI_Employee_1");

            entity.HasIndex(e => e.LocationId, "NCI_Employee_2");

            entity.HasIndex(e => e.Resign, "NCI_employee_3");

            entity.HasIndex(e => e.Exclusiveuser, "NCI_employee_4");

            entity.Property(e => e.EmpId)
                .ValueGeneratedOnAdd()
                .HasComment("Employee Id")
                .HasColumnType("numeric(10, 0)");
            entity.Property(e => e.AadharNo).HasColumnType("numeric(30, 0)");
            entity.Property(e => e.Absconding).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.AccBonusAcId).HasColumnType("numeric(9, 0)");
            entity.Property(e => e.AccBonusSubId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AccBonusSubID");
            entity.Property(e => e.AccExpenseId)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("AccExpenseID");
            entity.Property(e => e.AccExpenseSubId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AccExpenseSubID");
            entity.Property(e => e.AccountHeadId).HasColumnType("numeric(9, 0)");
            entity.Property(e => e.AccountId)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("AccountID");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.AdminAccess)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.Apistatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("APIStatus");
            entity.Property(e => e.AttachPhoto)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Attachment)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.AttachmentForEarlierEmprDetails)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Authority)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.AutoAttReq)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.BankBranchName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Bank_BranchName");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.BankNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasComment("Bank Account Number");
            entity.Property(e => e.BirthDate)
                .HasComment("Date Of Birth")
                .HasColumnType("datetime");
            entity.Property(e => e.Block)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.CardId).HasColumnType("numeric(9, 0)");
            entity.Property(e => e.CardNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Cases)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.CatId)
                .HasComment("Employee Category Id")
                .HasColumnType("numeric(10, 0)");
            entity.Property(e => e.Cityid).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.CompanyHead)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");
            entity.Property(e => e.ContBadgeNo)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Cont_BadgeNo");
            entity.Property(e => e.ContractorTypeId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.CostCenter).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.DefaultEss)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("DefaultESS");
            entity.Property(e => e.DeptId)
                .HasComment("Department Id")
                .HasColumnType("numeric(9, 0)");
            entity.Property(e => e.Deptids)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Did)
                .HasComment("Designation Id")
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("DId");
            entity.Property(e => e.DocCtrlPwd)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.EarlierDesignation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EarlierEmployerLocation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EarlierEmployerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EarlierReportingTo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EffectiveDate).HasDefaultValueSql("('')");
            entity.Property(e => e.EmailId)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("EMailId");
            entity.Property(e => e.EmailPwd)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("EMailPwd");
            entity.Property(e => e.EmailUserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EmpAttDesc)
                .HasMaxLength(800)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EmpCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Employee Code");
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Employee Name");
            entity.Property(e => e.EntryComputer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EntryEmpId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Etseadequate)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("ETSEAdequate");
            entity.Property(e => e.Etsecomments)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("ETSEComments");
            entity.Property(e => e.ExFromPayRoll)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.Exclusiveuser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.ExpId)
                .HasComment("Experience Id")
                .HasColumnType("numeric(9, 0)");
            entity.Property(e => e.FatherName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Form5Remarks)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Grade)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("IFSCCode");
            entity.Property(e => e.IncrementedDate).HasColumnType("datetime");
            entity.Property(e => e.Interviewed)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.IsContractEmp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.IsHandicapped)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.IsSrCitizen)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.JoinDate)
                .HasComment("Employee Join Date")
                .HasColumnType("datetime");
            entity.Property(e => e.LocationId)
                .HasDefaultValue(1m)
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LoginName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasComment("Marital Status (Single/Married)");
            entity.Property(e => e.MaximumPovalue)
                .HasDefaultValue(0.0)
                .HasColumnName("MaximumPOValue");
            entity.Property(e => e.MobileAppLocUpdateMins)
                .HasDefaultValue(15m)
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.MobileAppRegId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.MonthDay).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.MonthDayId)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("W");
            entity.Property(e => e.NacCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Nac_Code");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.OpPermission)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("OP_Permission");
            entity.Property(e => e.Paddress)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PanNo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("PANNOTAVBL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.PasswordHash).HasMaxLength(1000);
            entity.Property(e => e.PasswordSalt).HasMaxLength(1000);
            entity.Property(e => e.PayrollLocationId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.PinCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Porelease)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("PORelease");
            entity.Property(e => e.Ppserv)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("ppserv");
            entity.Property(e => e.Prapproval)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("PRApproval");
            entity.Property(e => e.Qid)
                .HasComment("Qualification Id")
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("QID");
            entity.Property(e => e.ReasonForLeaving)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.ReportTo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ReportUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.Resign)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasComment("Resignation");
            entity.Property(e => e.ResignDate).HasColumnType("datetime");
            entity.Property(e => e.Rfid)
                .HasMaxLength(1000)
                .HasDefaultValue("")
                .HasColumnName("RFID");
            entity.Property(e => e.RfideffFrom).HasColumnName("RFIDEffFrom");
            entity.Property(e => e.RfideffTo).HasColumnName("RFIDEffTo");
            entity.Property(e => e.RotationInterval).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.RunOnce)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.SalOnWeeklyOff)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.SalOnWeeklyOffGo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.ServiceId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ServiceID");
            entity.Property(e => e.Setters)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.ShiftRotate)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
            entity.Property(e => e.ShiftRotationEndDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftRotationPattern)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.SignatureImage).IsUnicode(false);
            entity.Property(e => e.StandardHours)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("8,8,8,8,8,8,8");
            entity.Property(e => e.StatusEffectiveDate)
                .HasColumnType("datetime")
                .HasColumnName("Status_EffectiveDate");
            entity.Property(e => e.StatusId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.StockReportReminder)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.StockReportReminderForMg)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("StockReportReminderForMG");
            entity.Property(e => e.SubIds)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.Territory)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UnAvailGenHolAbsent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("UnAvailGenHol_Absent");
            entity.Property(e => e.UnAvailGenHolPresent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("UnAvailGenHol_Present");
            entity.Property(e => e.UserGroupId)
                .HasDefaultValueSql("('0')")
                .HasColumnType("numeric(9, 0)");
            entity.Property(e => e.Wages)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.WeekOffHourly)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("WeekOff_Hourly");
            entity.Property(e => e.WeekOffHourlyGo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("WeekOff_HourlyGo");
            entity.Property(e => e.WorkDaysBasedOnAbsent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
        });

        modelBuilder.Entity<InventSr>(entity =>
        {
            entity.HasKey(e => e.SrRefNo).HasName("Pk_Invent_Sr_Sr_Ref_No");

            entity.ToTable("Invent_SR", tb => tb.HasTrigger("del_Invent_SR"));

            entity.HasIndex(e => e.SrNo, "NCI_Invent_SR_1");

            entity.Property(e => e.SrRefNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sr_Ref_No");
            entity.Property(e => e.ApprovedBy).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ApprovedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CommentsForRejected)
                .HasMaxLength(500)
                .HasDefaultValue("");
            entity.Property(e => e.CostCenterId)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("CostCenterID");
            entity.Property(e => e.DeptId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("DeptID");
            entity.Property(e => e.Empid).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.EntryComputer)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.EntryDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EntryEmpId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Isproduct)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasColumnName("ISProduct");
            entity.Property(e => e.Locationid)
                .HasDefaultValue(1m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("locationid");
            entity.Property(e => e.SrNo)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Srdate)
                .HasColumnType("datetime")
                .HasColumnName("SRDate");
            entity.Property(e => e.Srdesc)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("SRDesc");
            entity.Property(e => e.Srquantity)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SRQuantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue(" ");
        });

        modelBuilder.Entity<InventSrmaterial>(entity =>
        {
            entity.HasKey(e => e.Srid).HasName("pk_Invent_SRMaterial_SRId");

            entity.ToTable("Invent_SRMaterial", tb => tb.HasTrigger("del_Invent_SRMaterial"));

            entity.HasIndex(e => e.Wono, "NCI_Invent_SRMaterial_1");

            entity.Property(e => e.Srid)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SRId");
            entity.Property(e => e.CharVals)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GradeId)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("GradeID");
            entity.Property(e => e.Grnid)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IssueQty)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Issue_Qty");
            entity.Property(e => e.MaterialSpec)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasDefaultValue(" ");
            entity.Property(e => e.Processid)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ProdBatchno)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ProdId)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(9, 0)")
                .HasColumnName("ProdID");
            entity.Property(e => e.Puom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("PUom");
            entity.Property(e => e.RawMatId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("RawMatID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Srno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("SRNo");
            entity.Property(e => e.Srqty).HasColumnName("SRQty");
            entity.Property(e => e.Sruom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SRUom");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Approved");
            entity.Property(e => e.StkQty)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StkUom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.ToolId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.Weight).HasDefaultValue(0.0);
            entity.Property(e => e.Wono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("WONo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
