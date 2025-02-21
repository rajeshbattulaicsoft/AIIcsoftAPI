using System;
using System.Collections.Generic;

namespace AIIcsoftAPI.Models.SMIcsoftDataModels;

public partial class AccessLevel
{
    /// <summary>
    /// LoginIdNo
    /// </summary>
    public decimal Loginid { get; set; }

    /// <summary>
    /// From which level like sales,genmaster,PPC
    /// </summary>
    public string? Levelno { get; set; }

    public string TabPages { get; set; } = null!;

    public decimal GroupId { get; set; }

    public decimal FormId { get; set; }

    public string FormView { get; set; } = null!;

    public string FormAdd { get; set; } = null!;

    public string FormEdit { get; set; } = null!;

    public string FormDelete { get; set; } = null!;

    public decimal AccessLevelId { get; set; }

    public decimal LocationId { get; set; }

    public string Pcids { get; set; } = null!;

    public decimal EntryEmpId { get; set; }

    public string EntryComputer { get; set; } = null!;

    public DateTime EntryDateTime { get; set; }
}
