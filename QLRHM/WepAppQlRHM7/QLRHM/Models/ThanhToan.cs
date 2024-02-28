using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public partial class ThanhToan
{
    [Key]
    public int Idtt { get; set; }

    public int Idbs { get; set; }

    public string MaThanhToan { get; set; } = null!;
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime NgayThanhToan { get; set; }

    public string? Nsua { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? NgaySua { get; set; }

    public virtual BacSi IdbsNavigation { get; set; } = null!;

    //public virtual ICollection<NoiDungThanhToan> NoiDungThanhToans { get; set; } = new List<NoiDungThanhToan>();
    public virtual List<NoiDungThanhToan> MasterNdtt { get; set; } = new List<NoiDungThanhToan>(); //detail very important
}
