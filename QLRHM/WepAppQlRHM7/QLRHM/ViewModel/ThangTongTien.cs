using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace QLRHM.Models;

public class ThangTongTien
{
    public int Thang { get; set; }
    public double TongTien { get; set; }
    public double TongBN { get; set; }
    public int TongKeHoach { get; set; }
    public int TongCV { get; set; }
    public double TongLH { get; set; }
}