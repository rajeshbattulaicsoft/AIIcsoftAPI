using System;
using System.Collections.Generic;

namespace AIIcsoftAPI.Models.SMIcsoftDataModels;

public partial class InventSr
{
    public decimal SrNo { get; set; }

    public string SrRefNo { get; set; } = null!;

    public decimal DeptId { get; set; }

    public decimal Empid { get; set; }

    public DateTime? Srdate { get; set; }

    public string Status { get; set; } = null!;

    public string Srdesc { get; set; } = null!;

    public decimal EntryEmpId { get; set; }

    public string EntryComputer { get; set; } = null!;

    public string Isproduct { get; set; } = null!;

    public decimal CostCenterId { get; set; }

    public decimal ApprovedBy { get; set; }

    public DateTime ApprovedDate { get; set; }

    public decimal Locationid { get; set; }

    public DateTime EntryDateTime { get; set; }

    public decimal Srquantity { get; set; }

    public string CommentsForRejected { get; set; } = null!;
}
