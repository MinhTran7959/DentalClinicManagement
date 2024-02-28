using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class NhomCongViec
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Idncv { get; set; }
    [Required]
    [StringLength(50)]
    public string MaNcv { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string TenNcv { get; set; } = null!;
  
    [StringLength(50)]
    public string? Ntao { get; set; }
   
    [StringLength(50)]
    public string? Nsua { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime? NgayTao { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    public DateTime? NgaySua { get; set; }

    public int Active { get; set; }

    //public virtual ICollection<CongViec> CongViecs { get; set; } = new List<CongViec>();
  
    public virtual List<CongViec> MasterCV { get; set; } = new List<CongViec>(); //detail very important
    
}
