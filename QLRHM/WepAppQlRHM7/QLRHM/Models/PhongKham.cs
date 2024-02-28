using QLRHM.Models;
using System;
using System.Collections.Generic;

namespace QLRHM7.Models;

public partial class PhongKham
{
    public int Id { get; set; }

    public int? Idbn { get; set; }

    public string? MaPhong { get; set; }

    public string? TenPhong { get; set; }

    public DateTime? NgayThem { get; set; }

    public DateTime? NgaySua { get; set; }

    //public virtual BenhNhan? IdbnNavigation { get; set; }
    public virtual ICollection<BenhNhan> BenhNhans { get; set; } = new List<BenhNhan>();
}
