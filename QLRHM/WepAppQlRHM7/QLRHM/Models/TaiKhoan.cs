using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class TaiKhoan
{
    [Key]
    public int Idtk { get; set; }

    public int Idbs { get; set; }

    public string? TenTaiKhoan { get; set; }
    public string? MatKhau { get; set; }
    [NotMapped]
    public string? MatKhau2 { get; set; }
    public string? Quyen { get; set; }

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }

    public virtual BacSi IdbsNavigation { get; set; } = null!;
}
