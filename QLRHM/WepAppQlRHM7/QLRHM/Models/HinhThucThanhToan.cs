using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public partial class HinhThucThanhToan
{
    [Key]
    public int Idhttt { get; set; }

    public int Idnh { get; set; }

    public string MaHttt { get; set; } = null!;

    public string TenHttt { get; set; } = null!;

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public string? NgayTao { get; set; }

    public string? NgaySua { get; set; }

    public virtual NganHang IdnhNavigation { get; set; } = null!;

    public virtual ICollection<NoiDungThanhToan> NoiDungThanhToans { get; set; } = new List<NoiDungThanhToan>();
}
