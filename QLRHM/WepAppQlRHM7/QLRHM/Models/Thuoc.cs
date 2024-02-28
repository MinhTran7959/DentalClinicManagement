using System;
using System.Collections.Generic;

namespace QLRHM.Models;

public partial class Thuoc
{
    public int Id { get; set; }

    public string? MaThuoc { get; set; }

    public string? TenThuoc { get; set; }

    public string? DonViTinh { get; set; }
    public int? active { get; set; }
}
