using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models;

public partial class ToaThuocChiTiet
{
    [Key]
    public int? Id { get; set; }
    [ForeignKey("KeToa")]
    public int? Idtoa { get; set; }

    public int? Idthuoc { get; set; }

    public int? SoLuong { get; set; }

    public string? CachDung { get; set; }

    public virtual Thuoc? IdthuocNavigation { get; set; }

    public virtual KeToa? IdtoaNavigation { get; set; }

    [NotMapped]
    public bool? IsDelete { get; set; } = false;
}
