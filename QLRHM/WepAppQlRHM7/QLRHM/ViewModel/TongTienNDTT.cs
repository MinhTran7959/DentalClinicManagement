using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLRHM.Models
{
    public partial class TongTienNDTT
    {
        [Key]
        public int Idndtt { get; set; }
        [ForeignKey("ThanhToan")]
        public int Idtt { get; set; }
        [ForeignKey("NoiDungKeHoach")]
        public int Idndkh { get; set; }
        [ForeignKey("HinhThucThanhToan")]
        public int Idhttt { get; set; }

        public string ThanhToan { get; set; } = null!;
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double SoTienThanhToan { get; set; }
        public string FormattedSoTienThanhToan
        {
            get { return $"{SoTienThanhToan:N0}"; }
        }
        public string? Ntao { get; set; }

        public string? Nsua { get; set; }

        public string? NgayTao { get; set; }

        public string? NgaySua { get; set; }

        public virtual HinhThucThanhToan IdhtttNavigation { get; set; } = null!;
      
        public virtual KeHoachDieuTri KeHoachDieuTri { get; set; } = null!;

        public virtual NoiDungKeHoach IdndkhNavigation { get; set; } = null!;

        public virtual ThanhToan IdttNavigation { get; set; } = null!;



        public string TotalSoCV { get; set; }
        public string TotalSoKH { get; set; }
        public string TotalTongKHDT { get; set; }
        public double TotalSoTienThanhToan { get; set; }
        public double TotalSoGoc { get; set; }
        public double TotalSoUocTinh { get; set; }
 
        public double TotalTienBs { get; set; }



        public string FormattedTotalSoTienThanhToan
        {
            get { return $"{TotalSoTienThanhToan:N0}"; }
        }
        public string FormattedTotalTienBs
        {
            get { return $"{TotalTienBs:N0}"; }
        } 
        public string FormattedTotalSoUocTinh
        {
            get { return $"{TotalSoUocTinh:N0}"; }
        } 
        public string FormattedTotalSoGoc
        {
            get { return $"{TotalSoGoc:N0}"; }
        }
        public string? TenCV { get; set; }
        public string? TenBS { get; set; }
        public string? MaCV { get; set; }
        public string? MaBS { get; set; }



    }
}
