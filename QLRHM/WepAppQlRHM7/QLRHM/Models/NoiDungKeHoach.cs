using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class NoiDungKeHoach
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Idndkh { get; set; }
    [ForeignKey("KeHoachDieuTri")]
    public int Idkhdt { get; set; }
    [ForeignKey("CongViec")]
    public int Idcvdt { get; set; }
    [ForeignKey("BacSi")]
    public int Idbsdt { get; set; }

    public int SoLan { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public double DonGia { get; set; }
    public string FormattedDonGia
    {
        get { return $"{DonGia:N0}"; }
    }
   

    public string? GhiChu { get; set; }

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
    [DataType(DataType.Date)]
    public DateTime? NgayTao { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
    [DataType(DataType.Date)]
    public DateTime? NgaySua { get; set; }

    public virtual BacSi IdbsdtNavigation { get; set; } = null!;

    public virtual CongViec IdcvdtNavigation { get; set; } = null!;

    public virtual KeHoachDieuTri IdkhdtNavigation { get; set; }
    [NotMapped]
    public bool? IsDelete { get; set; } = false;

    public virtual ICollection<LichHen> LichHens { get; set; } = new List<LichHen>();

    public virtual ICollection<NoiDungThanhToan> NoiDungThanhToans { get; set; } = new List<NoiDungThanhToan>();
    public virtual ICollection<TongTienNDTT> TongTienNDTT { get; set; } = new List<TongTienNDTT>();
    //public virtual KeHoachDieuTri IdkhdtNavigation { get; set; } //very important
}
