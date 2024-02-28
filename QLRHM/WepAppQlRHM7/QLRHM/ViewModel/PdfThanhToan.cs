using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models
{
    public partial class PdfThanhToan
    {
        public List<NoiDungThanhToan> noiDungThanhToan { get; set; }
        public NoiDungThanhToan noiDungThanhToan1 { get; set; }
        public KeHoachDieuTri KeHoachDieuTri { get; set; }
        public ThanhToan thanhToan { get; set; }
        public List<BacSi> bacSis { get; set; }
        public List<NoiDungKeHoach> noiDungKeHoaches { get; set; }
        public List<CongViec> CongViecList { get; set; }
        public BenhNhan benhNhan { get; set; }

    }
}
