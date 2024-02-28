using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class BaoHanh
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Idbh { get; set; }
   
    [StringLength(50)]
    public string? MaBaoHanh { get; set; }
  
    [StringLength(50)]
    public string? TenBaoHanh { get; set; }

  
    public int? SoNgay { get; set; }

    [StringLength(50)]
    public string? Ntao { get; set; }

    [StringLength(50)]
    public string? Nsua { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? NgayTao { get; set; }
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? NgaySua { get; set; }

    public int? Active { get; set; }

    //public virtual ICollection<CongViec> CongViecs { get; set; } = new List<CongViec>();
}
