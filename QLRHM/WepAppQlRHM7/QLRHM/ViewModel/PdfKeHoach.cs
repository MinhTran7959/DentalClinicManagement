using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QLRHM.Models;

public partial class PdfKeHoach
{
    public KeHoachDieuTri KeHoachDieuTri { get; set; }
    public List<BacSi> BacSiList { get; set; }
    public List<CongViec> CongViecList { get; set; }
    public BenhNhan BenhNhan { get; set; }

}
