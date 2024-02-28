using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class KeToa
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    public int? Idbs { get; set; }

    public int? Idbn { get; set; }

    public string? MaToaThuoc { get; set; }

    public DateTime? NgayLap { get; set; }

    public string? ChuanDoan { get; set; }

    public virtual BenhNhan? IdbnNavigation { get; set; }

    public virtual BacSi? IdbsNavigation { get; set; }

    public virtual List<ToaThuocChiTiet> MasterTTCT { get; set; } = new List<ToaThuocChiTiet>();
}
