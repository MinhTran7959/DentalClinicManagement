using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace QLRHM.Models;

public partial class CongViec
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Idcvdt { get; set; }
  
    [ForeignKey("NhomCongViec")]
    public int Idncv { get; set; }
    [ForeignKey("BaoHanh")]
    public int Idbh { get; set; }
  
    [StringLength(50)]
    public string? MaCongViec { get; set; }
    [Required]
    [StringLength(50)]
    public string? TenCongViec { get; set; }
    [Required]
    [StringLength(100)]
    public string? MoTa { get; set; }
    [Required]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
    public double? DonGiaSuDung { get; set; }
    public string FormattedDonGiaSuDung
    {
        get { return $"{DonGiaSuDung:N0}"; }
    }
    [StringLength(50)]
    public string? Ntao { get; set; }

    [StringLength(50)]
    public string? Nsua { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? NgayTao { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? NgaySua { get; set; }

    public int? Active { get; set; }
  
    //public virtual BaoHanh IdbhNavigation { get; set; } = null!;


    //public virtual NhomCongViec IdncvNavigation { get; set; } = null!;

    public virtual ICollection<NoiDungKeHoach> NoiDungKeHoaches { get; set; } = new List<NoiDungKeHoach>();
  
    public virtual NhomCongViec NhomCongViec { get; set; } //very important
    public virtual BaoHanh BaoHanh { get; set; } //very important

}
