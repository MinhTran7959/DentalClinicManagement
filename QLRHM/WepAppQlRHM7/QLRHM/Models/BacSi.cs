using QLRHM7.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class BacSi
{
    [Key]
    public int Idbs { get; set; }

    public int? Idnbs { get; set; }

    public string MaBacSi { get; set; } = null!;

    public string TenBs { get; set; } = null!;

    public string? AnhBs { get; set; }
    [NotMapped]
    public IFormFile? FrontImage { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime NgaySinh { get; set; }
    public string FormatNgaySinh
    {
        get { return $"{NgaySinh:dd/MM/yyyy}"; }
    }
    public string Cccd { get; set; } = null!;

    public string GioiTinh { get; set; } = null!;

    public string? QueQuan { get; set; }

    public string? DiaChi { get; set; }

    public string Sdt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Zalo { get; set; }

    public string? Facebook { get; set; }

    public string? GhiChu { get; set; }

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }

    public int Active { get; set; }


    public virtual NhomBacSi? IdnbsNavigation { get; set; }
 
    public virtual ICollection<KeHoachDieuTri> KeHoachDieuTris { get; set; } = new List<KeHoachDieuTri>();
 
    public virtual ICollection<NoiDungKeHoach> NoiDungKeHoaches { get; set; } = new List<NoiDungKeHoach>();
 
    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    public virtual ICollection<KeToa> KeToas { get; set; } = new List<KeToa>();
}
