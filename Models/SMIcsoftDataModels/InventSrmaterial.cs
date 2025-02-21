using System;
using System.Collections.Generic;

namespace AIIcsoftAPI.Models.SMIcsoftDataModels;

public partial class InventSrmaterial
{
    public decimal Srid { get; set; }

    public decimal Srno { get; set; }

    public decimal RawMatId { get; set; }

    public double Srqty { get; set; }

    public string Sruom { get; set; } = null!;

    public string? CharVals { get; set; }

    public string MaterialSpec { get; set; } = null!;

    public double? Price { get; set; }

    public double? Weight { get; set; }

    public decimal? ProdId { get; set; }

    public decimal? GradeId { get; set; }

    public string? Wono { get; set; }

    public decimal? StkQty { get; set; }

    public string? StkUom { get; set; }

    public string? Puom { get; set; }

    public decimal? Grnid { get; set; }

    public decimal? Processid { get; set; }

    public string? ProdBatchno { get; set; }

    public string? ToolId { get; set; }

    public decimal? IssueQty { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }
}
