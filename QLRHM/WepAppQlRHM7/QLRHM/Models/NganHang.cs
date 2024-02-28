using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public partial class NganHang
{
    [Key]
    public int Idnh { get; set; }

    public string MaNganHang { get; set; } = null!;

    public string TenNganHang { get; set; } = null!;

    public string SoTk { get; set; } = null!;

    public string TenTk { get; set; } = null!;

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }

    public int Active { get; set; }

    public virtual ICollection<HinhThucThanhToan> HinhThucThanhToans { get; set; } = new List<HinhThucThanhToan>();
}
