using QLRHM7.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class BenhNhan
{
    [Key]
    public int Idbn { get; set; }

    public string MaBenhNhan { get; set; } = null!;

    public string TenBn { get; set; } = null!;

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime NgaySinh { get; set; }
    public string FormatNgaySinh
    {
        get { return $"{NgaySinh:dd/MM/yyyy}"; }
    }
    public string? AnhBn { get; set; }
    [NotMapped]
    public IFormFile? FrontImage { get; set; }

    public string Cccd { get; set; } = null!;

    public string GioiTinh { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public string? Zalo { get; set; }

    public string? FaceBook { get; set; }

    public string? GhiChu { get; set; }

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? NgayTao { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? NgaySua { get; set; }
    public string GioTiepNhan
    {
        get { return $"{NgaySua:dd/MM/yyyy HH:mm}"; }
    }
    public int Active { get; set; }
    public int? Phong { get; set; }
    public virtual PhongKham? PhongNavigation { get; set; }
    public virtual ICollection<KeHoachDieuTri> KeHoachDieuTris { get; set; } = new List<KeHoachDieuTri>();
    //public virtual ICollection<PhongKham> PhongKhams { get; set; } = new List<PhongKham>();
    public virtual ICollection<TienSuBenhNhan> TienSuBenhNhans { get; set; } = new List<TienSuBenhNhan>();
    public virtual ICollection<KeToa> KeToas { get; set; } = new List<KeToa>();
}
