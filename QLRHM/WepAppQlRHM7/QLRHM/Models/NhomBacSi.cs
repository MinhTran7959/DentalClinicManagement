using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public partial class NhomBacSi
{
    [Key]
    public int Idnbs { get; set; }

    public string MaNbs { get; set; } = null!;

    public string TenNbs { get; set; } = null!;

    public string? Ntao { get; set; }

    public string? Nsua { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgaySua { get; set; }

    public int Active { get; set; }

    public virtual ICollection<BacSi> BacSis { get; set; } = new List<BacSi>();
}
