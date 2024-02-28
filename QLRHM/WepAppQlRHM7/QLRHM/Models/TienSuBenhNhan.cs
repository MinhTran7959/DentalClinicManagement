using System;
using System.Collections.Generic;

namespace QLRHM.Models;

public partial class TienSuBenhNhan
{
    public int Id { get; set; }

    public int? Idbn { get; set; }

    public int? TimMach { get; set; }

    public int? TieuDuong { get; set; }

    public int? CaoHuyetAp { get; set; }

    public int? TruyenNhiem { get; set; }

    public string? DiUngThuoc { get; set; }

    public string? Khac { get; set; }

    public virtual BenhNhan? IdbnNavigation { get; set; }
}
