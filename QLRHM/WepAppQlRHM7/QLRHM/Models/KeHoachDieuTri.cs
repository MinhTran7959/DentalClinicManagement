using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QLRHM.Models;

public partial class KeHoachDieuTri
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Idkhdt { get; set; }

    public int Idbn { get; set; }

    public int Idbs { get; set; }

    public string MaKeHoacDieuTri { get; set; } = null!;
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime NgayLap { get; set; }

    public string? DieuTri { get; set; }

    public string? Nsua { get; set; }
    //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? NgaySua { get; set; }

    public virtual BenhNhan IdbnNavigation { get; set; } = null!;

    public virtual BacSi IdbsNavigation { get; set; } = null!;

    //public virtual ICollection<NoiDungKeHoach> NoiDungKeHoaches { get; set; } = new List<NoiDungKeHoach>();
    public virtual List<NoiDungKeHoach> MasterNd { get; set; } = new List<NoiDungKeHoach>(); //detail very important
 

}
