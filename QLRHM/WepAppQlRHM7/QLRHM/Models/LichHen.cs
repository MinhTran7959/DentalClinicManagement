using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public partial class LichHen
{
    [Key]
    public int Idlh { get; set; }

    public int? Idndkh { get; set; } 

    public string MaLichHen { get; set; } = null!;

    public string LyDo { get; set; } = null!;
    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    //[DataType(DataType.Date)]
    public DateTime NgayHen { get; set; }
    public TimeSpan? GioHen { get; set; }

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }
    public DateTime? NgayDen { get; set; }
    public int? TrangThai { get; set; }
    public int? Active { get; set; }

    public virtual NoiDungKeHoach IdndkhNavigation { get; set; } = null!;


    

    public string FormatNgayHen
    {
        get { return NgayHen.ToString("dd/MM/yyyy"); }
    }

    public string FormatNgaySua
    {
        get { return NgaySua.HasValue ? NgaySua.Value.ToString("dd/MM/yyyy HH:mm") : ""; }
    }

    public string FormatNgayDen
    {
        get { return NgayDen.HasValue ? NgayDen.Value.ToString("dd/MM/yyyy HH:mm") : ""; }
    }


}
