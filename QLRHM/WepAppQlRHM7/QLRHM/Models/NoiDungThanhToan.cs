using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class NoiDungThanhToan
{
    [Key]
    public int Idndtt { get; set; }
    [ForeignKey("ThanhToan")]
    public int Idtt { get; set; }
    [ForeignKey("NoiDungKeHoach")]
    public int Idndkh { get; set; }
    [ForeignKey("HinhThucThanhToan")]
    public int Idhttt { get; set; }

    public string ThanhToan { get; set; } = null!;
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public double SoTienThanhToan { get; set; }
    public string FormattedSoTienThanhToan
    {
        get { return $"{SoTienThanhToan:N0}"; }
    }
    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
    //[DataType(DataType.Date)]
    public DateTime? NgayTao { get; set; }
    public string FormatNgayTao
    {
        get { return NgayTao.HasValue ? NgayTao.Value.ToString("dd-MM-yyyy HH:mm") : string.Empty; }
    }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? NgaySua { get; set; }

    [NotMapped]
    public bool? IsDelete { get; set; } = false;

    public virtual HinhThucThanhToan IdhtttNavigation { get; set; } = null!;

    public virtual NoiDungKeHoach IdndkhNavigation { get; set; } = null!;

    public virtual ThanhToan IdttNavigation { get; set; } = null!;
}
