using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using System.Globalization;
using System.Collections.Immutable;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ClosedXML.Excel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class ThongKeController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public ThongKeController(DatnqlrhmContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> ThongKeChung(string fromDay, string toDay, string thongkeOption , CongViec congViec)
        //{
        //    var now = DateTime.Now.Date;

        //    CultureInfo culture1 = CultureInfo.CurrentCulture;
        //    int currentWeek1 = culture1.Calendar.GetWeekOfYear(now, culture1.DateTimeFormat.CalendarWeekRule, culture1.DateTimeFormat.FirstDayOfWeek);
        //    DateTime startOfWeek1 = now.AddDays(-(int)now.DayOfWeek);DateTime endOfWeek1 = startOfWeek1.AddDays(6);
        //    int currentMonth1 = now.Month;
        //    DateTime startOfMonth1 = new DateTime(now.Year, 1, 1);DateTime endOfMonth1 = new DateTime(now.Year, 3, 30);DateTime startOfQuy2 = new DateTime(now.Year, 4, 1); DateTime endOfQuy2 = new DateTime(now.Year, 6, 30);
        //    DateTime startOfQuy3 = new DateTime(now.Year, 7, 1);DateTime endOfQuy3 = new DateTime(now.Year, 9, 30);DateTime startOfQuy4 = new DateTime(now.Year, 10, 1); DateTime endOfQuy4 = new DateTime(now.Year, 12, 31);

        //    ViewBag.NgayHienTai = $"{now.ToString("dd/MM/yyyy")}";ViewBag.TuanHienTai = $"{startOfWeek1.ToString("dd/MM/yyyy")} - {endOfWeek1.ToString("dd/MM/yyyy")}";ViewBag.ThangHienTai = $"Tháng: {now.ToString("MM/yyyy")}"; 
        //    ViewBag.Quy1HienTai = $"{startOfMonth1.ToString("dd/MM/yyyy")} - {endOfMonth1.ToString("dd/MM/yyyy")}";ViewBag.Quy2HienTai = $"{startOfQuy2.ToString("dd/MM/yyyy")} - {endOfQuy2.ToString("dd/MM/yyyy")}";
        //    ViewBag.Quy3HienTai = $"{startOfQuy3.ToString("dd/MM/yyyy")} - {endOfQuy3.ToString("dd/MM/yyyy")}";ViewBag.Quy4HienTai = $"{startOfQuy4.ToString("dd/MM/yyyy")} - {endOfQuy4.ToString("dd/MM/yyyy")}";

        //    var TienNgay = await _context.NoiDungThanhToans.Include(x=>x.IdttNavigation).Include(x=>x.IdndkhNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x=>x.IdndkhNavigation.IdcvdtNavigation).Include(x=>x.IdndkhNavigation.IdbsdtNavigation).Where(x=>x.ThanhToan=="1"|| x.ThanhToan=="0").ToListAsync();
        //    var KHNgay = await _context.KeHoachDieuTris.Where(x=>x.DieuTri=="1" | x.DieuTri=="3").ToListAsync();
        //    var BNNgay = await _context.BenhNhans.ToListAsync();

        //    if (thongkeOption == "tuan")
        //    {
        //        CultureInfo culture = CultureInfo.CurrentCulture;
        //        int currentWeek = culture.Calendar.GetWeekOfYear(now, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);

        //        // Tính ngày đầu và cuối của tuần hiện tại
        //        DateTime startOfWeek = now.AddDays(-(int)now.DayOfWeek); // Ngày đầu tuần (Chủ Nhật)
        //        DateTime endOfWeek = startOfWeek.AddDays(6); // Ngày cuối tuần (Thứ Bảy)

        //        TienNgay = TienNgay.Where(x => x != null && x.NgayTao != null && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) >= startOfWeek && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) <= endOfWeek && x.ThanhToan == "1").ToList();
        //        var totalTienNgay = TienNgay.Sum(x => x.SoTienThanhToan);
        //        var TienNgay1 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
        //                                    .Select(group => new TongTienNDTT
        //                                    {
        //                                        Idndkh = group.Key,
        //                                        TenCV = group.First().IdndkhNavigation.IdcvdtNavigation.TenCongViec,
        //                                        MaCV = group.First().IdndkhNavigation.IdcvdtNavigation.MaCongViec,
        //                                        TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                                        TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
        //                                        TotalSoUocTinh = group.Where(x => x.ThanhToan == "0").Sum(item => item.SoTienThanhToan),



        //                                        TotalSoTienThanhToan = group.Sum(item => item.SoTienThanhToan),
        //                                    })
        //                                  .ToList();

        //        var TienNgay2 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
        //            .Select(group => new TongTienNDTT
        //            {
        //                Idndkh = group.Key,
        //                TenBS = group.First().IdndkhNavigation.IdbsdtNavigation.TenBs,
        //                MaBS = group.First().IdndkhNavigation.IdbsdtNavigation.MaBacSi,
        //                TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                TotalSoKH = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idnbs).Count().ToString(),

        //                TotalTienBs = group.Sum(item => item.SoTienThanhToan)
        //            })
        //            .ToList();

        //        ViewBag.TopBacSi = TienNgay2;
        //        ViewBag.TopCongViec = TienNgay1;
        //        int count = KHNgay.Where(x => x.NgayLap.Date >= startOfWeek && x.NgayLap.Date <= endOfWeek).Count(x => x.Idkhdt != null);
        //        int count1 = BNNgay.Where(x => x.NgayTao.Value.Date >= startOfWeek && x.NgayTao.Value.Date <= endOfWeek).Count(x => x.Idbn != null);
        //        ViewBag.KHNgay = count.ToString();
        //        ViewBag.BNNgay = count1.ToString();
        //        ViewBag.TienNgay = totalTienNgay.ToString("N0");
        //        var FMstartOfWeek = startOfWeek.ToString("dd/MM/yyyy");
        //        var FMendOfWeek = endOfWeek.ToString("dd/MM/yyyy");
        //        ViewBag.FMstartOfWeek = FMstartOfWeek;
        //        ViewBag.FMendOfWeek = FMendOfWeek;
        //    }
        //    else if (thongkeOption == "thang")
        //    {
        //        int currentMonth = now.Month;
        //        DateTime startOfMonth = new DateTime(now.Year, currentMonth, 1);
        //        DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        //        TienNgay = TienNgay.Where(x => x != null && x.NgayTao != null && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) >= startOfMonth && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) <= endOfMonth && x.ThanhToan == "1").ToList();
        //        var TienNgay1 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
        //                     .Select(group => new TongTienNDTT
        //                     {
        //                         Idndkh = group.Key,
        //                         TenCV = group.First().IdndkhNavigation.IdcvdtNavigation.TenCongViec,
        //                         MaCV = group.First().IdndkhNavigation.IdcvdtNavigation.MaCongViec,
        //                         TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                         TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
        //                         TotalSoUocTinh = group.Where(x => x.ThanhToan == "0").Sum(item => item.SoTienThanhToan),



        //                         TotalSoTienThanhToan = group.Sum(item => item.SoTienThanhToan),
        //                     })
        //                   .ToList();

        //        var TienNgay2 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
        //            .Select(group => new TongTienNDTT
        //            {
        //                Idndkh = group.Key,
        //                TenBS = group.First().IdndkhNavigation.IdbsdtNavigation.TenBs,
        //                MaBS = group.First().IdndkhNavigation.IdbsdtNavigation.MaBacSi,
        //                TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                TotalSoKH = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idnbs).Count().ToString(),

        //                TotalTienBs = group.Sum(item => item.SoTienThanhToan)
        //            })
        //            .ToList();

        //        ViewBag.TopBacSi = TienNgay2;
        //        ViewBag.TopCongViec = TienNgay1;

        //        var totalTienNgay = TienNgay.Sum(x => x.SoTienThanhToan);
        //        int count = KHNgay.Where(x => x.NgayLap.Date >= startOfMonth && x.NgayLap.Date <= endOfMonth).Count(x => x.Idkhdt != null);
        //        int count1 = BNNgay.Where(x => x.NgayTao.Value.Date >= startOfMonth && x.NgayTao.Value.Date <= endOfMonth).Count(x => x.Idbn != null);
        //        ViewBag.KHNgay = count.ToString();
        //        ViewBag.BNNgay = count1.ToString();
        //        ViewBag.TienNgay = totalTienNgay.ToString("N0");
        //        var FMstart = startOfMonth.ToString("dd/MM/yyyy");
        //        var FMend = endOfMonth.ToString("dd/MM/yyyy");
        //        ViewBag.FMstart = FMstart;
        //        ViewBag.FMend = FMend;
        //    }
        //    else if (thongkeOption == "quy1" || thongkeOption == "quy2" || thongkeOption == "quy3" || thongkeOption == "quy4")
        //    {
        //        int currentYear = now.Year;
        //        int currentMonth = now.Month;
        //        DateTime startQuarter, endQuarter;

        //        if (thongkeOption == "quy1")
        //        {
        //            startQuarter = new DateTime(currentYear, 1, 1);
        //            endQuarter = new DateTime(currentYear, 3, 31);
        //        }
        //        else if (thongkeOption == "quy2")
        //        {
        //            startQuarter = new DateTime(currentYear, 4, 1);
        //            endQuarter = new DateTime(currentYear, 6, 30);
        //        }
        //        else if (thongkeOption == "quy3")
        //        {
        //            startQuarter = new DateTime(currentYear, 7, 1);
        //            endQuarter = new DateTime(currentYear, 9, 30);

        //        }
        //        else
        //        {
        //            startQuarter = new DateTime(currentYear, 10, 1);
        //            endQuarter = new DateTime(currentYear, 12, 31);
        //        }

        //        TienNgay = TienNgay.Where(x => x != null && x.NgayTao != null && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) >= startQuarter 
        //                                 && DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) <= endQuarter && x.ThanhToan == "1").ToList();

        //        var TienNgay1 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
        //                      .Select(group => new TongTienNDTT
        //                      {
        //                          Idndkh = group.Key,
        //                          TenCV = group.First().IdndkhNavigation.IdcvdtNavigation.TenCongViec,
        //                          MaCV = group.First().IdndkhNavigation.IdcvdtNavigation.MaCongViec,
        //                          TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                          TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
        //                          TotalSoUocTinh = group.Where(x => x.ThanhToan == "0").Sum(item => item.SoTienThanhToan),



        //                          TotalSoTienThanhToan = group.Sum(item => item.SoTienThanhToan),
        //                      })
        //                    .ToList();

        //        var TienNgay2 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
        //            .Select(group => new TongTienNDTT
        //            {
        //                Idndkh = group.Key,
        //                TenBS = group.First().IdndkhNavigation.IdbsdtNavigation.TenBs,
        //                MaBS = group.First().IdndkhNavigation.IdbsdtNavigation.MaBacSi,
        //                TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                TotalSoKH = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idnbs).Count().ToString(),

        //                TotalTienBs = group.Sum(item => item.SoTienThanhToan)
        //            })
        //            .ToList();

        //        ViewBag.TopBacSi = TienNgay2;
        //        ViewBag.TopCongViec = TienNgay1;
        //        var totalTienNgay = TienNgay.Sum(x => x.SoTienThanhToan);
        //        int count = KHNgay.Where(x => x.NgayLap.Date >= startQuarter && x.NgayLap.Date <= endQuarter).Count(x => x.Idkhdt != null);
        //        int count1 = BNNgay.Where(x => x.NgayTao.Value.Date >= startQuarter && x.NgayTao.Value.Date <= endQuarter).Count(x => x.Idbn != null);
        //        ViewBag.KHNgay = count.ToString();
        //        ViewBag.BNNgay = count1.ToString();
        //        ViewBag.TienNgay = totalTienNgay.ToString("N0");
        //        var FMstart = startQuarter.ToString("dd/MM/yyyy");
        //        var FMend = endQuarter.ToString("dd/MM/yyyy");
        //        ViewBag.FMstart = FMstart;
        //        ViewBag.FMend = FMend;
        //    }


        //    else
        //    {
        //        TienNgay = TienNgay.Where(x =>DateTime.ParseExact(x.NgayTao.Substring(0, 10), "dd/MM/yyyy", null) == now.Date &&x.ThanhToan=="1").ToList();

        //        int count = KHNgay.Where(x => x.NgayLap.Date == now).Count(x => x.Idkhdt != null);
        //        int count1 = BNNgay.Where(x =>  x.NgayTao.Value.Date == now.Date).Count(x => x.Idbn != null);
        //        //TotalSoUocTinh = group.Select(item => item.IdndkhNavigation.DonGia).Distinct().Sum(),

        //        var TienNgay1 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
        //                      .Select(group => new TongTienNDTT
        //                      {
        //                          Idndkh = group.Key,
        //                          TenCV = group.First().IdndkhNavigation.IdcvdtNavigation.TenCongViec,
        //                          MaCV = group.First().IdndkhNavigation.IdcvdtNavigation.MaCongViec,
        //                          TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                          TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
        //                          TotalSoUocTinh = group.Where(x=>x.ThanhToan=="0").Sum(item => item.SoTienThanhToan),



        //                          TotalSoTienThanhToan = group.Sum(item => item.SoTienThanhToan),
        //                      })
        //                    .ToList();

        //        var TienNgay2 = TienNgay.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
        //            .Select(group => new TongTienNDTT
        //            {
        //                Idndkh = group.Key,
        //                TenBS = group.First().IdndkhNavigation.IdbsdtNavigation.TenBs,
        //                MaBS = group.First().IdndkhNavigation.IdbsdtNavigation.MaBacSi,
        //                TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
        //                TotalSoKH = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idnbs).Count().ToString(),

        //                TotalTienBs = group.Sum(item => item.SoTienThanhToan)                      
        //            })
        //            .ToList();


        //        ViewBag.TopBacSi = TienNgay2;
        //        //TotalSoCV = group.Select(item => item.IdndkhNavigation.Idcvdt).Distinct().Count().ToString(),
        //        ViewBag.TopCongViec = TienNgay1;
        //        var totalTienNgay = TienNgay.Sum(x => x.SoTienThanhToan);
        //        ViewBag.KHNgay = count.ToString();ViewBag.BNNgay = count1.ToString();ViewBag.TienNgay = totalTienNgay.ToString("N0");
        //        var fortmatnow = now.ToString("dd/MM/yyyy");
        //        ViewBag.fortmatnow = fortmatnow;
        //        return View();
        //    }

        //    return View();
        //}


        //public async Task<IActionResult> ThongKeChiTiet()
        //{


        //    var result = _context.NoiDungKeHoaches
        //        .Where(ndkh => ndkh.IdkhdtNavigation.DieuTri == "1")
        //        .Join(_context.CongViecs, ndkh => ndkh.IdcvdtNavigation.Idcvdt, cv => cv.Idcvdt, (ndkh, cv) => new { NDKH = ndkh, CV = cv })
        //        .Join(_context.NhomCongViecs, joined => joined.CV.NhomCongViec.Idncv, ncv => ncv.Idncv, (joined, ncv) => new { joined.NDKH, joined.CV, NCV = ncv })
        //        .Join(_context.BacSis, joined => joined.NDKH.IdbsdtNavigation.Idbs, bs => bs.Idbs, (joined, bs) => new { joined.NDKH, joined.CV, joined.NCV, BS = bs })
        //        .Join(_context.NoiDungThanhToans, joined => joined.NDKH.Idndkh, ndtt => ndtt.Idndkh, (joined, ndtt) => new { joined.NDKH, joined.CV, joined.NCV, joined.BS, NDTT = ndtt })
        //        .Where(joined => joined.NDTT.ThanhToan == "1" && joined.NDKH.IdkhdtNavigation.DieuTri == "1")
        //        .GroupBy(joined => new
        //        {

        //            joined.NDKH.IdbsdtNavigation.Idbs,
        //            joined.BS.TenBs,
        //            joined.NCV.TenNcv,
        //            joined.CV.TenCongViec,
        //            joined.NDTT.ThanhToan
        //        })
        //        .Select(group => new
        //        {
        //            ID_BacSiDieuTri = group.Key.Idbs,
        //            Ten_BacSiDieuTri = group.Key.TenBs,
        //            Ten_NhomCongViec = group.Key.TenNcv,
        //            Ten_CongViec = group.Key.TenCongViec,
        //            Thanh_Toan = group.Key.ThanhToan,
        //            Tong_Tien_ThanhToan = group.Sum(j => j.NDTT.SoTienThanhToan)
        //        })
        //        .ToList();

        //    return View();
        //}


       
        public async Task<IActionResult> ThongKeCV(string? fo, string? to)
        {
           
            var data = await _context.NoiDungThanhToans.Include(x=>x.IdndkhNavigation.IdcvdtNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation).Include(x=>x.IdttNavigation).ToListAsync();
            DateTime fromDate, toDate;
            if (fo != null && to != null)
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                fromDate = toDate = DateTime.Now.Date;
                             
            }
            
            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
            var TongTienCV = data2.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
                          .Select(group => new TongTienNDTT
                          {
                              Idndkh = group.Key,
                              TenCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.TenCongViec,
                              MaCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.MaCongViec,
                              TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                              TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
                              TotalSoUocTinh = group.Where(x => x.ThanhToan != "1" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri=="1").Sum(item => item.SoTienThanhToan),
                              TotalSoTienThanhToan = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                              NgayTao = fromDate.ToString("dd/MM/yyyy"),
                              NgaySua = toDate.ToString("dd/MM/yyyy"),
                          })
                                .ToList();
            var DanhSachCV = data2
                .GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.TenCongViec)
                .Select(group => $"{group.Key}")
                .ToList();
            ViewBag.DanhSachCV = DanhSachCV;
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            ViewBag.TopCongViec = TongTienCV;
            ViewBag.ChiTietCV = data2;
            return View();
        }
     
        public async Task<IActionResult> PdfThongKeCV(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo,"dd/MM/yyyy", CultureInfo.InvariantCulture);         
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x=>x.IdndkhNavigation.IdcvdtNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation).Include(x=>x.IdttNavigation).ToListAsync();
           
                var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
                var TongTienCV = data2.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
                              .Select(group => new TongTienNDTT
                              {
                                  Idndkh = group.Key,
                                  TenCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.TenCongViec,
                                  MaCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.MaCongViec,
                                  TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                                  TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
                                  TotalSoUocTinh = group.Where(x => x.ThanhToan != "1").Sum(item => item.SoTienThanhToan),
                                  TotalSoTienThanhToan = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                                  NgayTao = fromDate.ToString("dd/MM/yyyy"),
                                  NgaySua = toDate.ToString("dd/MM/yyyy"),
                              })
                                    .ToList();
          
            
            return new ViewAsPdf("PdfThongKeCV", TongTienCV)

            {

                FileName = $"ThongKeCongViec.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4

            };
            //return View(TongTienCV);
        }

        public async Task<IActionResult> ExportExcel(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
            var TongTienCV = data2.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.Idcvdt)
                          .Select(group => new TongTienNDTT
                          {
                              Idndkh = group.Key,
                              TenCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.TenCongViec,
                              MaCV = group.FirstOrDefault()?.IdndkhNavigation.IdcvdtNavigation.MaCongViec,
                              TotalSoCV = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                              TotalSoGoc = (double)group.Select(item => item.IdndkhNavigation.IdcvdtNavigation.DonGiaSuDung).Distinct().Sum(),
                              TotalSoUocTinh = group.Where(x => x.ThanhToan != "1").Sum(item => item.SoTienThanhToan),
                              TotalSoTienThanhToan = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                              NgayTao = fromDate.ToString("dd/MM/yyyy"),
                              NgaySua = toDate.ToString("dd/MM/yyyy"),
                          })
                                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoDoanhThuCongViec");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:G1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH CÔNG VIỆC " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy")+ ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "  Mã công việc";
                worksheet.Cell("B2").Value = " Tên công việc";
                worksheet.Cell("C2").Value = " Số lần";
                worksheet.Cell("D2").Value = "Tiền theo bảng giá";
                worksheet.Cell("E2").Value = " Tiền theo nội dung kế hoạch";
                worksheet.Cell("F2").Value = " Tiền chưa thanh toán";
                worksheet.Cell("G2").Value = "Thực thu";
              
                worksheet.Range("A2:G2").Style.Font.Bold = true;
                worksheet.Range("A2:G2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                var stt = 1; var SoLan = 0; var TienDieuTri = 0; var TongTiLe = 0; var TienChuaTT = 0; var TienDaTT = 0; decimal TienBangGia = 0; // Khởi tạo biến ngoài vòng lặp
                for (var i = 0; i < TongTienCV.Count; i++)
                {
                    var bs = TongTienCV[i];
                    var currentRow = i + 3; 
                    double TotalSoCV = double.Parse(bs.TotalSoCV);
                    decimal TienUocTinh = (decimal)(bs.TotalSoUocTinh + bs.TotalSoTienThanhToan);
                    decimal tiecgoc = (decimal)(TotalSoCV * bs.TotalSoGoc);
                    worksheet.Cell(currentRow, 1).Value = bs.MaCV;
                    worksheet.Cell(currentRow, 2).Value = bs.TenCV;
                    worksheet.Cell(currentRow, 3).Value = bs.TotalSoCV;
                    worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    for (int col = 4; col <= 7; col++)
                    {
                        worksheet.Cell(currentRow, col).Value = col switch
                        {
                            4 => tiecgoc,
                            5 => string.Format("{0:#,0}", TienUocTinh),
                            6 => string.Format("{0:#,0}", bs.TotalSoUocTinh),
                            7 => string.Format("{0:#,0}", bs.TotalSoTienThanhToan),
                            _ => string.Empty
                        };

                        worksheet.Cell(currentRow, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }


                    TienBangGia += Convert.ToDecimal(tiecgoc);
                    SoLan += Convert.ToInt32(bs.TotalSoCV);
                    TienDieuTri += Convert.ToInt32(TienUocTinh);
                    TienChuaTT += Convert.ToInt32(bs.TotalSoUocTinh);
                    TienDaTT += Convert.ToInt32(bs.TotalSoTienThanhToan);
                }
                var lastRow = TongTienCV.Count + 3;
                worksheet.Range($"A{lastRow}:B{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 3).Value = string.Format("{0:#,0}", SoLan);
                worksheet.Cell(lastRow, 4).Value = string.Format("{0:#,0}", TienBangGia);
                worksheet.Cell(lastRow, 5).Value = string.Format("{0:#,0}", TienDieuTri);
                worksheet.Cell(lastRow, 6).Value = string.Format("{0:#,0}", TienChuaTT);
                worksheet.Cell(lastRow, 7).Value = string.Format("{0:#,0}", TienDaTT);

                var style = worksheet.Range($"A{lastRow}:G{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 15;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachCongViec_" + fromDate.ToString("dd/MM/yyyy")+"_"+toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }

        public async Task<IActionResult> ChiTietBaoCaoCV( int id , string fo, string to)
        {
           
           
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation) .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                    .Include(x => x.IdttNavigation).Where(x => x.ThanhToan == "1")
                .ToListAsync();
            var ChiTietBaoCaoCV = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdcvdtNavigation.Idcvdt == id).OrderByDescending(x=>x.IdndkhNavigation.IdcvdtNavigation.Idcvdt).ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

            return View(ChiTietBaoCaoCV);
        }

         public async Task<IActionResult> PdfChiTietBaoCaoCV( int id , string fo, string to)
        {
           
           
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation) .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdttNavigation).Where(x=>x.ThanhToan=="1")
                .ToListAsync();
            var ChiTietBaoCaoCV = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdcvdtNavigation.Idcvdt == id).ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            var TenCV = ChiTietBaoCaoCV.GroupBy(x => x.IdndkhNavigation.IdcvdtNavigation.MaCongViec)
                .Select(group => group.Key)
                .FirstOrDefault();


            return new ViewAsPdf("PdfChiTietBaoCaoCV", ChiTietBaoCaoCV)

            {
               
                FileName = $"ThongKeChiTiet"+ TenCV+".pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") }
                    }

            };

            //return View(ChiTietBaoCaoCV);
        }
        public async Task<IActionResult> ExcelChiTietBaoCaoCV(int id, string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdttNavigation)
                .ToListAsync();
            var ChiTietBaoCaoCV = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdcvdtNavigation.Idcvdt == id).ToList();
           

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoChiTietCongViec");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:H1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH CÔNG VIỆC " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "  Mã Bệnh nhân";
                worksheet.Cell("B2").Value = " Tên Bệnh nhân";
                worksheet.Cell("C2").Value = " Ngày sinh";
                worksheet.Cell("D2").Value = "Số điện thoại";
                worksheet.Cell("E2").Value = " Mã công việc";
                worksheet.Cell("F2").Value = "Tên công việc";
                worksheet.Cell("G2").Value = "Số tiền thanh toán";
                worksheet.Cell("H2").Value = "Ngày thanh toán";
                //worksheet.Cell("A5:F5").Value = "Tổng";
                //worksheet.Cell("G5:H5").Value = "Tổng";

                worksheet.Range("A2:H2").Style.Font.Bold = true;
                worksheet.Range("A2:H2").Style.Font.FontSize = 13;
                var TongTien = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < ChiTietBaoCaoCV.Count; i++)
                {
                    var bs = ChiTietBaoCaoCV[i];
                    var currentRow = i + 3;
                    
                    worksheet.Cell(currentRow, 1).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 2).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.FormatNgaySinh;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Sdt;
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdcvdtNavigation.MaCongViec;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 7).Value = bs.FormattedSoTienThanhToan;
                    worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 8).Value = bs.FormatNgayTao;
                    TongTien += Convert.ToInt32(bs.SoTienThanhToan);
                }
                var lastRow = ChiTietBaoCaoCV.Count + 3;
                worksheet.Range($"A{lastRow}:F{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 7).Value = string.Format("{0:#,0}", TongTien);

                var style = worksheet.Range($"A{lastRow}:H{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 15;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachChiTietCongViec_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }

        public async Task<IActionResult> PdfChiTietBaoCaoCV2( string fo, string to)
        {


            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdttNavigation)
                .ToListAsync();
            var ChiTietBaoCaoCV = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) ).ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

            return new ViewAsPdf("PdfChiTietBaoCaoCV2", ChiTietBaoCaoCV)

            {

                FileName = $"DanhSachChiTietCongViec.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") }
                    }

            };

            //return View(ChiTietBaoCaoCV);
        }
        public async Task<IActionResult> ExcelChiTietBaoCaoCV2( string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdttNavigation)
                .ToListAsync();
            var ChiTietBaoCaoCV = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoChiTietCongViec");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:H1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH CHI TIẾT CÁC CÔNG VIỆC " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "  Mã Bệnh nhân";
                worksheet.Cell("B2").Value = " Tên Bệnh nhân";
                worksheet.Cell("C2").Value = " Ngày sinh";
                worksheet.Cell("D2").Value = "Số điện thoại";
                worksheet.Cell("E2").Value = " Mã công việc";
                worksheet.Cell("F2").Value = "Tên công việc";
                worksheet.Cell("G2").Value = "Số tiền thanh toán";
                worksheet.Cell("H2").Value = "Số tiền thực thu";
                worksheet.Cell("I2").Value = "Ngày thanh toán";
                //worksheet.Cell("A5:F5").Value = "Tổng";
                //worksheet.Cell("G5:H5").Value = "Tổng";

                worksheet.Range("A2:I2").Style.Font.Bold = true;
                worksheet.Range("A2:I2").Style.Font.FontSize = 13;
                var TongTien = 0; double TongTienThieuVaTT = 0; double TongTienThieuVaTT2 = 0;
                double TienTT = 0; double TienTT2 = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < ChiTietBaoCaoCV.Count; i++)
                {
                    
                    var bs = ChiTietBaoCaoCV[i];
                    var currentRow = i + 3;
                    if (bs.ThanhToan == "1") { TienTT = bs.SoTienThanhToan; }
                    else { TienTT = 0; }
                    if (bs.ThanhToan == "0") { TienTT2 = bs.SoTienThanhToan; }
                    else { TienTT2 = 0; }
                    worksheet.Cell(currentRow, 1).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 2).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.FormatNgaySinh;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Sdt;
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdcvdtNavigation.MaCongViec;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 7).Value = bs.IdndkhNavigation.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 8).Value = bs.FormattedSoTienThanhToan;
                    worksheet.Cell(currentRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;


                    worksheet.Cell(currentRow, 9).Value = bs.FormatNgayTao;
                    TongTien += Convert.ToInt32(TienTT);
                    TongTienThieuVaTT += Convert.ToInt32(bs.SoTienThanhToan);
                }
                var lastRow = ChiTietBaoCaoCV.Count + 3;
                TongTienThieuVaTT2 = TongTienThieuVaTT - TongTien;
                worksheet.Range($"A{lastRow}:B{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 3).Value = string.Format("{0:#,0}", TongTienThieuVaTT);
                worksheet.Range($"D{lastRow}:E{lastRow}").Merge().Value = "Đã thanh roán";
                worksheet.Cell(lastRow, 6).Value = string.Format("{0:#,0}", TongTien);
                 worksheet.Range($"G{lastRow}:H{lastRow}").Merge().Value = "Còn thiếu";
                worksheet.Cell(lastRow, 9).Value = string.Format("{0:#,0}", TongTienThieuVaTT2);


                var style = worksheet.Range($"A{lastRow}:I{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 15;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachChiTietCongViec_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }


        public async Task<IActionResult> ThongKeKHDT(string? fo, string? to)
        {
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();
            DateTime fromDate, toDate;
            if (fo != null && to != null)
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               
            }
            else
            {
                fromDate = toDate = DateTime.Now.Date;
               
            }
            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
            var HuyKHDT = await _context.NoiDungKeHoaches.Include(x=>x.IdkhdtNavigation).Include(x=>x.IdcvdtNavigation).Include(x=>x.IdbsdtNavigation)
                .Where(x => x.IdkhdtNavigation.DieuTri == null   && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToListAsync();
            var BoKHDT = data2.Where(x=> 
                            ((x.ThanhToan == "0"  &&x.IdndkhNavigation.IdkhdtNavigation.DieuTri =="3")
                            || x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2")
                            && x.NgayTao != null 
                            && (x.IdndkhNavigation.IdkhdtNavigation.NgayLap.Date >= fromDate && x.IdndkhNavigation.IdkhdtNavigation.NgayLap.Date <= toDate)).ToList();
           
            var ChiTietKHDT = data2.OrderByDescending(x => x.IdndkhNavigation.IdkhdtNavigation.Idkhdt).ToList();
            var TongND_KHDT = data2.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
                              .Select(group => new TongTienNDTT
                              {
                                  Idndkh = group.Key,
                                  TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.TenBs,
                                  MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.MaBacSi,
                                  TotalSoCV = group.Where(x => x.ThanhToan == "1").Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                                  TotalTongKHDT = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                                  TotalSoKH = group.Where(x =>( x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                                      .Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),
                                  TotalTienBs = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                                  NgayTao = fromDate.ToString("dd/MM/yyyy"),
                                  NgaySua = toDate.ToString("dd/MM/yyyy"),
                              })
                              .ToList();

            var TongKHDT = data2.GroupBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs)
                             .Select(group => new TongTienNDTT
                             {
                                 Idndkh = group.Key,
                                 TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs,
                                 MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.MaBacSi,
                                 TotalSoCV = group.Where(x => x.ThanhToan == "1" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3")
                                     .Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),
                                 TotalTongKHDT = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),
                                 TotalSoKH = group.Where(x => (x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                                     .Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),
                                 TotalTienBs = group.Sum(item => item.SoTienThanhToan),
                                 NgayTao = fromDate.ToString("dd/MM/yyyy"),
                                 NgaySua = toDate.ToString("dd/MM/yyyy"),
                             })
                             .ToList();





                ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
                ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
                ViewBag.TongND_KHDT = TongND_KHDT;
                ViewBag.TongKHDT = TongKHDT;
                ViewBag.BoKHDT = BoKHDT;
                ViewBag.ChiTietKHDT = ChiTietKHDT;
                ViewBag.HuyKHDT = HuyKHDT;
                return View();
          


            
        }
        public async Task<IActionResult> PdfHuyKHDT(string fo, string to)
        {

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var HuyKHDT = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdbsdtNavigation)
                .Where(x => x.IdkhdtNavigation.DieuTri == null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToListAsync();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");



            //return View();
            return new ViewAsPdf("PdfHuyKHDT", HuyKHDT)

            {

                FileName = $"ThongKeKHDTBiHuy.pdf",
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "HuyKHDT", HuyKHDT },

                    }

            };
        }

        public async Task<IActionResult> PdfBoKHDT(string fo, string to)
        {

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var HuyKHDT = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdbsdtNavigation).Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbsNavigation)
                .Where(x =>(x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) 
                && ( x.IdkhdtNavigation.DieuTri == "2")).ToListAsync();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            
            //return View();
            return new ViewAsPdf("PdfBoKHDT", HuyKHDT)

            {

                FileName = $"ThongKeKHDTBiBo.pdf",
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "HuyKHDT", HuyKHDT },

                    }

            };
        }

        public async Task<IActionResult> ExcelBoKHDT(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var HuyKHDT = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdbsdtNavigation).Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbsNavigation)
                 .Where(x => (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)
                 && (x.IdkhdtNavigation.DieuTri == "2")).ToListAsync();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ThongKeKeHoachDieuTriBiHuy");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:K1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH KẾ HOẠCH ĐIỀU TRỊ " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bác sĩ KH";
                worksheet.Cell("B2").Value = "Tên bác sĩ KH";
                worksheet.Cell("C2").Value = "Mã kế hoạch";
                worksheet.Cell("D2").Value = "Ngày lập KH";
                worksheet.Cell("E2").Value = "Mã bệnh nhân";
                worksheet.Cell("F2").Value = "Tên bệnh nhân";
                worksheet.Cell("G2").Value = "Mã bác sĩ điều trị";
                worksheet.Cell("H2").Value = "Tên bác sĩ điều trị";
                worksheet.Cell("I2").Value = "Tên CVDT";
                worksheet.Cell("J2").Value = "Đơn giá gốc";
                worksheet.Cell("K2").Value = "Đơn giá TT";
                worksheet.Cell("L2").Value = "Lý do huỷ";




                worksheet.Range("A2:L2").Style.Font.Bold = true;
                worksheet.Range("A2:L2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác

                for (var i = 0; i < HuyKHDT.Count; i++)
                {
                    var bs = HuyKHDT[i];
                    var currentRow = i + 3;

                    worksheet.Cell(currentRow, 1).Value = bs.IdkhdtNavigation.IdbsNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 2).Value = bs.IdkhdtNavigation.IdbsNavigation.TenBs;
                    worksheet.Cell(currentRow, 3).Value = bs.IdkhdtNavigation.MaKeHoacDieuTri;
                    worksheet.Cell(currentRow, 4).Value = bs.IdkhdtNavigation.NgayLap.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(currentRow, 5).Value = bs.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 6).Value = bs.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 7).Value = bs.IdbsdtNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 8).Value = bs.IdbsdtNavigation.TenBs;
                    worksheet.Cell(currentRow, 9).Value = bs.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 10).Value = bs.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 11).Value = bs.FormattedDonGia;
                    worksheet.Cell(currentRow, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 12).Value = bs.IdkhdtNavigation.Nsua;

                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeKeHoachDieuTriBiHuy_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> PdfThongKeChiTietKHDT(string fo, string to)
        {

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
               .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
               .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation)
               .ToListAsync();

            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();

            var ChiTietKHDT = data2.OrderByDescending(x => x.IdndkhNavigation.IdkhdtNavigation.Idkhdt).ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");



            //return View();
            return new ViewAsPdf("PdfThongKeChiTietKHDT", ChiTietKHDT)

            {

                FileName = $"ThongKeChiTietKHDT.pdf",
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "ChiTietKHDT", ChiTietKHDT },

                    }

            };
        }
        public async Task<IActionResult> PdfThongKeKHDT(  string fo, string to)
        {

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();
           
                var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();

                var TongKHDT = data2.GroupBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs)
                             .Select(group => new TongTienNDTT
                             {
                                 Idndkh = group.Key,
                                 TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs,
                                 MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.MaBacSi,
                                 TotalSoCV = group.Where(x => x.ThanhToan == "1" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3").Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// số khdt đã điều trị
                                 TotalTongKHDT = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// tổng số khdt
                                 TotalSoKH = group.Where(x => (x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                                 .Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// số khdt bị huỷ

                                 TotalTienBs = group.Sum(item => item.SoTienThanhToan),
                                 NgayTao = fromDate.ToString("dd/MM/yyyy"),
                                 NgaySua = toDate.ToString("dd/MM/yyyy"),
                             })
                                   .ToList();
                ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
                ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

                ViewBag.TongKHDT = TongKHDT;

            //return View();
            return new ViewAsPdf("PDFThongKeKHDT", TongKHDT)

            {

                FileName = $"ThongKeKHDT.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "TongKHDT", TongKHDT },

                    }

            };
        }
        public async Task<IActionResult> ExcelThongKeKHDT(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();

            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();

            var TongKHDT = data2.GroupBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs)
                         .Select(group => new TongTienNDTT
                         {
                             Idndkh = group.Key,
                             TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs,
                             MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.MaBacSi,
                             TotalSoCV = group.Where(x => x.ThanhToan == "1" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3").Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// số khdt đã điều trị
                             TotalTongKHDT = group.Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// tổng số khdt
                             TotalSoKH = group.Where(x =>( x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                             .Select(item => item.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count().ToString(),// số khdt bị huỷ

                             TotalTienBs = group.Sum(item => item.SoTienThanhToan),
                             NgayTao = fromDate.ToString("dd/MM/yyyy"),
                             NgaySua = toDate.ToString("dd/MM/yyyy"),
                         })
                               .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ThongKeSoKHDTTheoBS");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:F1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH TÓM TẮT SỐ LẦN KẾ HOẠCH ĐIỀU TRỊ CỦA CÁC BÁC SĨ" + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bác sĩ ";
                worksheet.Cell("B2").Value = "Tên bác sĩ ";
                worksheet.Cell("C2").Value = "KHĐT điều trị";
                worksheet.Cell("D2").Value = "KHĐT đã thực hiện";
                worksheet.Cell("E2").Value = "KHĐT bị huỷ bỏ";
                worksheet.Cell("F2").Value = "KHĐT đang hoạt động";




                worksheet.Range("A2:F2").Style.Font.Bold = true;
                worksheet.Range("A2:F2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                var TongKHDT1 = 0; var TongKHDT_Dang = 0; var TongKHDT_Huy = 0; var TongKHDT_DaX = 0;
                for (var i = 0; i < TongKHDT.Count; i++)
                {
                    var bs = TongKHDT[i];
                    var currentRow = i + 3;
                  
                    worksheet.Cell(currentRow, 1).Value = bs.MaBS;
                    worksheet.Cell(currentRow, 2).Value = bs.TenBS;
                    worksheet.Cell(currentRow, 3).Value = bs.TotalTongKHDT;
                    worksheet.Cell(currentRow, 4).Value = bs.TotalSoCV;
                    worksheet.Cell(currentRow, 5).Value = bs.TotalSoKH;
                    var Khdt_DangCo = (Convert.ToInt32(bs.TotalTongKHDT)) - (Convert.ToInt32(bs.TotalSoKH) + Convert.ToInt32(bs.TotalSoCV));
                    worksheet.Cell(currentRow, 6).Value = Khdt_DangCo;
                    for (int col = 3; col <= 6; col++)
                    {
                        worksheet.Cell(currentRow, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                    TongKHDT1 += Convert.ToInt32(bs.TotalTongKHDT);
                    TongKHDT_DaX += Convert.ToInt32(bs.TotalSoCV);
                    TongKHDT_Huy += Convert.ToInt32(bs.TotalSoKH);
                    TongKHDT_Dang += Convert.ToInt32(Khdt_DangCo);
                }
                var lastRow = TongKHDT.Count + 3;
                worksheet.Range($"A{lastRow}:B{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 3).Value = string.Format("{0:#,0}", TongKHDT1);
                worksheet.Cell(lastRow, 4).Value = string.Format("{0:#,0}", TongKHDT_DaX);
                worksheet.Cell(lastRow, 5).Value = string.Format("{0:#,0}", TongKHDT_Huy);
                worksheet.Cell(lastRow, 6).Value = string.Format("{0:#,0}", TongKHDT_Dang);
               

                var style = worksheet.Range($"A{lastRow}:F{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 15;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeTomTatSoLanLenKeHoachDieuTri" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> ExcelThongKeND_KHDT(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();

            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();

            var TongND_KHDT = data2.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
                           .Select(group => new TongTienNDTT
                           {
                               Idndkh = group.Key,
                               TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.TenBs,
                               MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.MaBacSi,
                               TotalSoCV = group.Where(x => x.ThanhToan == "1").Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// số cv đã điều trị
                               TotalTongKHDT = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// Tổng số cv  điều trị
                               TotalSoKH = group.Where(x => (x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                               .Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// số ndkh bị huỷ

                               TotalTienBs = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                               NgayTao = fromDate.ToString("dd/MM/yyyy"),
                               NgaySua = toDate.ToString("dd/MM/yyyy"),
                           })
                                 .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ThongKeTomTatSoLanDieuTriBSĐT");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:F1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH TÓM TẮT SỐ LẦN ĐIỀU TRỊ CỦA CÁC BÁC SĨ ĐIỀU TRỊ " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bác sĩ ";
                worksheet.Cell("B2").Value = "Tên bác sĩ ";
                worksheet.Cell("C2").Value = "Tổng số CV điều trị";
                worksheet.Cell("D2").Value = "Số CV đã điều trị";
                worksheet.Cell("E2").Value = "Số CV bị huỷ bỏ";
                worksheet.Cell("F2").Value = "Số CV đang điều trị";




                worksheet.Range("A2:F2").Style.Font.Bold = true;
                worksheet.Range("A2:F2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                var TongND_KHDT1 = 0; var TongND_KHDT_Dang = 0; var TongND_KHDT_Huy = 0; var TongND_KHDT_DaX = 0;
                for (var i = 0; i < TongND_KHDT.Count; i++)
                {
                    var bs = TongND_KHDT[i];
                    var currentRow = i + 3;

                    worksheet.Cell(currentRow, 1).Value = bs.MaBS;
                    worksheet.Cell(currentRow, 2).Value = bs.TenBS;
                    worksheet.Cell(currentRow, 3).Value = bs.TotalTongKHDT;
                    worksheet.Cell(currentRow, 4).Value = bs.TotalSoCV;
                    worksheet.Cell(currentRow, 5).Value = bs.TotalSoKH;
                    var Khdt_DangCo = (Convert.ToInt32(bs.TotalTongKHDT)) - (Convert.ToInt32(bs.TotalSoKH) + Convert.ToInt32(bs.TotalSoCV));
                    if (Khdt_DangCo < 0) { Khdt_DangCo = 0; }
                    worksheet.Cell(currentRow, 6).Value = Khdt_DangCo;
                    for (int col = 3; col <= 6; col++)
                    {
                        worksheet.Cell(currentRow, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                    TongND_KHDT1 += Convert.ToInt32(bs.TotalTongKHDT);
                    TongND_KHDT_Dang += Convert.ToInt32(bs.TotalSoCV);
                    TongND_KHDT_Huy += Convert.ToInt32(bs.TotalSoKH);
                    TongND_KHDT_DaX += Convert.ToInt32(Khdt_DangCo);
                }
                var lastRow = TongND_KHDT.Count + 3;
                worksheet.Range($"A{lastRow}:B{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 3).Value = string.Format("{0:#,0}", TongND_KHDT1);
                worksheet.Cell(lastRow, 4).Value = string.Format("{0:#,0}", TongND_KHDT_Dang);
                worksheet.Cell(lastRow, 5).Value = string.Format("{0:#,0}", TongND_KHDT_Huy);
                worksheet.Cell(lastRow, 6).Value = string.Format("{0:#,0}", TongND_KHDT_DaX);


                var style = worksheet.Range($"A{lastRow}:F{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 15;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeTomTatSoLanDieuTriCuaBSĐT" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> ExcelThongKeChiTietKHDT(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();

            var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).OrderByDescending(X=>X.IdndkhNavigation.IdkhdtNavigation.Idkhdt).ToList();



            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ThongKeKeHoachDieuTri");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:L1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH KẾ HOẠCH ĐIỀU TRỊ " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bác sĩ KH";
                worksheet.Cell("B2").Value = "Tên bác sĩ KH";
                worksheet.Cell("C2").Value = "Mã kế hoạch";
                worksheet.Cell("D2").Value = "Ngày lập KH";
                worksheet.Cell("E2").Value = "Mã bệnh nhân";
                worksheet.Cell("F2").Value = "Tên bệnh nhân";
                worksheet.Cell("G2").Value = "Mã bác sĩ điều trị";
                worksheet.Cell("H2").Value = "Tên bác sĩ điều trị";
                worksheet.Cell("I2").Value = "Tên CVDT";
                worksheet.Cell("J2").Value = "Đơn giá gốc";
                worksheet.Cell("K2").Value = "Đơn giá TT";
                worksheet.Cell("L2").Value = "Ghi chú";
                worksheet.Cell("M2").Value = "Tình trạng";
               

                worksheet.Range("A2:M2").Style.Font.Bold = true;
                worksheet.Range("A2:M2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
               
                for (var i = 0; i < data2.Count; i++)
                {
                    var bs = data2[i];
                    var currentRow = i + 3;
                    
                    worksheet.Cell(currentRow, 1).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 2).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs;
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.MaKeHoacDieuTri;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.NgayLap.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 7).Value = bs.IdndkhNavigation.IdbsdtNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 8).Value = bs.IdndkhNavigation.IdbsdtNavigation.TenBs;
                    worksheet.Cell(currentRow, 9).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 10).Value = bs.IdndkhNavigation.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 11).Value = bs.IdndkhNavigation.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 12).Value = bs.IdndkhNavigation.GhiChu;
                    worksheet.Cell(currentRow, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    if(bs.ThanhToan == "1" && (bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3"))
                     {

                        worksheet.Cell(currentRow, 13).Value = "Đã điều trị";

                    }
                   else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3")
                    {
                        worksheet.Cell(currentRow, 13).Value = "Bị huỷ";
                    }else if ( bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2")
                    {
                        worksheet.Cell(currentRow, 13).Value = "Bị huỷ";
                    }
                    else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1")
                    {
                        worksheet.Cell(currentRow, 13).Value = "Đang điều trị";
                    }
                }
                
                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeKeHoachDieuTri_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> ExcelHuyKHDT(string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var HuyKHDT = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdbsdtNavigation)
                .Where(x => x.IdkhdtNavigation.DieuTri == null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToListAsync();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ThongKeKeHoachDieuTriBiHuy");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:J1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH KẾ HOẠCH ĐIỀU TRỊ KHÔNG ĐỒNG Ý ĐIỀU TRỊ " + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bác sĩ KH";
                worksheet.Cell("B2").Value = "Tên bác sĩ KH";
                worksheet.Cell("C2").Value = "Mã kế hoạch";
                worksheet.Cell("D2").Value = "Ngày lập KH";
                worksheet.Cell("E2").Value = "Mã bệnh nhân";
                worksheet.Cell("F2").Value = "Tên bệnh nhân";
                worksheet.Cell("G2").Value = "Mã bác sĩ điều trị";
                worksheet.Cell("H2").Value = "Tên bác sĩ điều trị";
                worksheet.Cell("I2").Value = "Tên CVDT";
                worksheet.Cell("J2").Value = "Đơn giá gốc";
                worksheet.Cell("K2").Value = "Đơn giá TT";
                worksheet.Cell("L2").Value = "Ghi chú";
              


                worksheet.Range("A2:L2").Style.Font.Bold = true;
                worksheet.Range("A2:L2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác

                for (var i = 0; i < HuyKHDT.Count; i++)
                {
                    var bs = HuyKHDT[i];
                    var currentRow = i + 3;

                    worksheet.Cell(currentRow, 1).Value = bs.IdkhdtNavigation.IdbsNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 2).Value = bs.IdkhdtNavigation.IdbsNavigation.TenBs;
                    worksheet.Cell(currentRow, 3).Value = bs.IdkhdtNavigation.MaKeHoacDieuTri;
                    worksheet.Cell(currentRow, 4).Value = bs.IdkhdtNavigation.NgayLap.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(currentRow, 5).Value = bs.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 6).Value = bs.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 7).Value = bs.IdbsdtNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 8).Value = bs.IdbsdtNavigation.TenBs;
                    worksheet.Cell(currentRow, 9).Value = bs.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 10).Value = bs.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 11).Value = bs.FormattedDonGia;
                    worksheet.Cell(currentRow, 12).Value = bs.GhiChu;
                    worksheet.Cell(currentRow, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                   
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKeKeHoachDieuTriBiHuy_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> PdfThongKeND_KHDT(  string fo, string to)
        {

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdttNavigation)
                .ToListAsync();
           
                var data2 = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();

            var TongND_KHDT = data2.GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.Idbs)
                           .Select(group => new TongTienNDTT
                           {
                               Idndkh = group.Key,
                               TenBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.TenBs,
                               MaBS = group.Where(x => x.ThanhToan == "1").FirstOrDefault()?.IdndkhNavigation.IdbsdtNavigation.MaBacSi,
                               TotalSoCV = group.Where(x => x.ThanhToan == "1").Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// số cv đã điều trị
                               TotalTongKHDT = group.Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// Tổng số cv  điều trị
                               TotalSoKH = group.Where(x => (x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") || (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2"))
                               .Select(item => item.IdndkhNavigation.Idndkh).Distinct().Count().ToString(),// số ndkh bị huỷ

                               TotalTienBs = group.Where(x => x.ThanhToan == "1").Sum(item => item.SoTienThanhToan),
                               NgayTao = fromDate.ToString("dd/MM/yyyy"),
                               NgaySua = toDate.ToString("dd/MM/yyyy"),
                           })
                                 .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
                ViewBag.toDate = toDate.ToString("dd/MM/yyyy");

                ViewBag.TongND_KHDT = TongND_KHDT;

            //return View();
            return new ViewAsPdf("PdfThongKeND_KHDT", TongND_KHDT)

            {

                FileName = $"ThongKeNoiDungKHDT.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "TongND_KHDT", TongND_KHDT },

                    }

            };
        }



        public async Task<IActionResult>ChiTietKHDT( int id  , string fo, string to)
        {
            if (id == null)
            {
                return NotFound();
            }
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();
            
            var ChiTietBaoCaoKHDT = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs == id)
                .OrderByDescending(x=>x.IdndkhNavigation.IdkhdtNavigation.Idkhdt)              
                .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            return View(ChiTietBaoCaoKHDT);
        }
        public async Task<IActionResult>ChiTietND_KHDT( int id  , string fo, string to)
        {
            if (id == null)
            {
                return NotFound();
            }
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var ChiTietBaoCaoKHDT = data
            .Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdbsdtNavigation.Idbs == id)
 
            .ToList();



            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            return View(ChiTietBaoCaoKHDT);
        }
        public async Task<IActionResult>PdfChiTietKHDT( int id  , string fo, string to)
        {
            if (id == null)
            {
                return NotFound();
            }
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var ChiTietBaoCaoKHDT = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs == id)
                .OrderByDescending(x => x.IdndkhNavigation.IdkhdtNavigation.Idkhdt)
                .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            //return View(ChiTietBaoCaoKHDT);
            var firstItem = ChiTietBaoCaoKHDT.FirstOrDefault();
            string tenBsWithoutDiacritics = RemoveDiacritics(firstItem?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs ?? "");

            string fileName = firstItem != null ? $"ThongKeChiTietKeHoachDieuTri_{tenBsWithoutDiacritics}.pdf" : "ThongKeChiTietKeHoachDieuTri.pdf";


            return new ViewAsPdf("PdfChiTietKHDT", ChiTietBaoCaoKHDT)

            {

                FileName = fileName,
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") }
                    }

            };
        }
        public async Task<IActionResult> PdfChiTietND_KHDT(int id, string fo, string to)
        {
            if (id == null)
            {
                return NotFound();
            }
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var ChiTietBaoCaoKHDT = data
            .Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdbsdtNavigation.Idbs == id)

            .ToList();



            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");


            //return View(ChiTietBaoCaoKHDT);
            var firstItem = ChiTietBaoCaoKHDT.FirstOrDefault();
            string tenBsWithoutDiacritics = RemoveDiacritics(firstItem?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs ?? "");

            string fileName = firstItem != null ? $"ThongKeChiTietKeHoachDieuTri_{tenBsWithoutDiacritics}.pdf" : "ThongKeChiTietKHDT.pdf";


            return new ViewAsPdf("PdfChiTietND_KHDT", ChiTietBaoCaoKHDT)

            {

                FileName = fileName,
                PageOrientation = Orientation.Portrait,
                //PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") }
                    }

            };
        }
        public async Task<IActionResult> ExcelChiTietND_KHDT(int id, string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var ChiTietBaoCaoKHDT = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdbsdtNavigation.Idbs == id)
                .OrderByDescending(x => x.IdndkhNavigation.IdkhdtNavigation.Idkhdt)
                .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            var firstItem = ChiTietBaoCaoKHDT.FirstOrDefault();
            string tenBsWithoutDiacritics = RemoveDiacritics(firstItem?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs ?? "");

            string fileName = firstItem != null ? $"{firstItem.IdndkhNavigation.IdbsdtNavigation.MaBacSi} - {firstItem.IdndkhNavigation.IdbsdtNavigation.TenBs} " : "ThongKeChiTietNoiDungKHDT";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoChiTietKHDT");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:I1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH NỘI DUNG KẾ HOẠCH ĐIỀU TRỊ: " + fileName + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;
                titleRange.FirstCell().Value = titleRange.FirstCell().Value.ToString().ToUpper();

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã kế hoạch điều trị";
                worksheet.Cell("B2").Value = "Ngày lập kế hoạch";
                worksheet.Cell("C2").Value = "Mã bệnh nhân";
                worksheet.Cell("D2").Value = "Tên bệnh nhân";
                //worksheet.Cell("E2").Value = "Mã bác sĩ điều trị";
                //worksheet.Cell("F2").Value = "Tên bác sĩ điều trị";
                worksheet.Cell("E2").Value = "Tên công việc điều trị";
                worksheet.Cell("F2").Value = "Số lần điều trị";
                worksheet.Cell("G2").Value = "Đơn giá gốc";
                worksheet.Cell("H2").Value = "Đơn giá TT";
                worksheet.Cell("I2").Value = "Tình trạng";
               
                

                worksheet.Range("A2:I2").Style.Font.Bold = true;
                worksheet.Range("A2:I2").Style.Font.FontSize = 13;
                var TongTien = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < ChiTietBaoCaoKHDT.Count; i++)
                {
                    var bs = ChiTietBaoCaoKHDT[i];
                    var currentRow = i + 3; var tinhtrang ="";
                    if(bs.ThanhToan == "1" && (bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3"))
                     {
                        tinhtrang = "Đã điều trị xong";
                     }
                     else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3")
                     {
                        tinhtrang = "Huỷ bỏ";
                    }
                    else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1")
                    {
                        tinhtrang = "Đang điều trị";
                    }
                    worksheet.Cell(currentRow, 1).Value = bs.IdndkhNavigation.IdkhdtNavigation.MaKeHoacDieuTri;
                    worksheet.Cell(currentRow, 2).Value = bs.IdndkhNavigation.IdkhdtNavigation.NgayLap.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;                   
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.SoLan;
                    //worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 7).Value = bs.IdndkhNavigation.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 8).Value = bs.FormattedSoTienThanhToan;
                    worksheet.Cell(currentRow, 9).Value = tinhtrang;
                   
                }

                //var lastRow = ChiTietBaoCaoKHDT.Count + 3;
               

                //var tongDaDieuTri = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "1" && (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3"));
                //var tongHuyBo = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3");
                //var tongDangDieuTri = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1");

                //worksheet.Range($"A{lastRow}:J{lastRow}").Merge().Value = $"Tổng kế hoạch đã điều trị xong: {tongDaDieuTri}, kế hoạch bị huỷ bỏ: {tongHuyBo}, kế hoạch đang điều trị: {tongDangDieuTri}";


                //var style = worksheet.Range($"A{lastRow}:J{lastRow}").Style;
                //style.Font.Bold = true;
                //style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //style.Font.FontSize = 15;
                //style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachChiTietNoiDung_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }
        public async Task<IActionResult> ExcelChiTietKHDT(int id, string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var data = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdttNavigation).ToListAsync();

            var ChiTietBaoCaoKHDT = data.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.Idbs == id)
                .OrderByDescending(x => x.IdndkhNavigation.IdkhdtNavigation.Idkhdt)
                .ToList();
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            var firstItem = ChiTietBaoCaoKHDT.FirstOrDefault();
            string tenBsWithoutDiacritics = RemoveDiacritics(firstItem?.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs ?? "");

            string fileName = firstItem != null ? $"{firstItem.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.MaBacSi} - {firstItem.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation.TenBs} " : "ThongKeChiTietNoiDungKHDT";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoChiTietKHDT");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:L1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH KẾ HOẠCH ĐIỀU TRỊ: " + fileName + "(" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;
                titleRange.FirstCell().Value = titleRange.FirstCell().Value.ToString().ToUpper();

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã kế hoạch điều trị";
                worksheet.Cell("B2").Value = "Ngày lập kế hoạch";
                worksheet.Cell("C2").Value = "Mã bệnh nhân";
                worksheet.Cell("D2").Value = "Tên bệnh nhân";
                worksheet.Cell("E2").Value = "Mã bác sĩ điều trị";
                worksheet.Cell("F2").Value = "Tên bác sĩ điều trị";
                worksheet.Cell("G2").Value = "Tên công việc điều trị";
                worksheet.Cell("H2").Value = "Số lần điều trị";
                worksheet.Cell("I2").Value = "Đơn giá gốc";
                worksheet.Cell("J2").Value = "Đơn giá TT";
                worksheet.Cell("K2").Value = "Đơn giá TT";
                worksheet.Cell("L2").Value = "Tình trạng";
               
                

                worksheet.Range("A2:L2").Style.Font.Bold = true;
                worksheet.Range("A2:L2").Style.Font.FontSize = 13;
                var TongTien = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < ChiTietBaoCaoKHDT.Count; i++)
                {
                    var bs = ChiTietBaoCaoKHDT[i];
                    var currentRow = i + 3; var tinhtrang ="";
                    if(bs.ThanhToan == "1" && (bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3"))
                     {
                        tinhtrang = "Đã điều trị xong";
                     }
                     else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3")
                     {
                        tinhtrang = "Huỷ bỏ";
                    } else if ( bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "2")
                     {
                        tinhtrang = "Huỷ bỏ";
                    }
                    else if (bs.ThanhToan == "0" && bs.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1")
                    {
                        tinhtrang = "Đang điều trị";
                    }
                    worksheet.Cell(currentRow, 1).Value = bs.IdndkhNavigation.IdkhdtNavigation.MaKeHoacDieuTri;
                    worksheet.Cell(currentRow, 2).Value = bs.IdndkhNavigation.IdkhdtNavigation.NgayLap.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdbsdtNavigation.MaBacSi;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.IdbsdtNavigation.TenBs;
                    worksheet.Cell(currentRow, 7).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 8).Value = bs.IdndkhNavigation.SoLan;
                    //worksheet.Cell(currentRow, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    worksheet.Cell(currentRow, 9).Value = bs.IdndkhNavigation.IdcvdtNavigation.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 10).Value = bs.FormattedSoTienThanhToan;
                    worksheet.Cell(currentRow, 11).Value = bs.IdndkhNavigation.GhiChu;
                    worksheet.Cell(currentRow, 12).Value = tinhtrang;
                   
                }

                //var lastRow = ChiTietBaoCaoKHDT.Count + 3;
               

                //var tongDaDieuTri = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "1" && (x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3"));
                //var tongHuyBo = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3");
                //var tongDangDieuTri = ChiTietBaoCaoKHDT.Count(x => x.ThanhToan == "0" && x.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1");

                //worksheet.Range($"A{lastRow}:J{lastRow}").Merge().Value = $"Tổng kế hoạch đã điều trị xong: {tongDaDieuTri}, kế hoạch bị huỷ bỏ: {tongHuyBo}, kế hoạch đang điều trị: {tongDangDieuTri}";


                //var style = worksheet.Range($"A{lastRow}:J{lastRow}").Style;
                //style.Font.Bold = true;
                //style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //style.Font.FontSize = 15;
                //style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachChiTietKHDT_" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ".xlsx");
                }
            }
        }


        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public async Task<IActionResult> ThongKeBn(string? fo, string? to , int page = 1)
        {
            //var data = await _context.NoiDungThanhToans
            //    .Include(x => x.IdndkhNavigation.IdcvdtNavigation)
            //    .Include(x => x.IdndkhNavigation.IdbsdtNavigation)
            //    .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbsNavigation)
            //    .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
            //    .Include(x => x.IdttNavigation)
            //    .ToListAsync();

          

            DateTime fromDate, toDate;

            if (fo == null && to == null)
            {
                fromDate = toDate = DateTime.Now.Date;
            }
            else
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var dataBn = await _context.BenhNhans.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToListAsync();
            ViewBag.TongBn = dataBn.Count();

            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dataBn.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.NoOfRecordsPerPage = NoOfRecordPerPage; // Thêm số lượng bản ghi trên mỗi trang vào ViewBag
            dataBn = dataBn.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            ViewBag.dataBn = dataBn;
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
          
            return View();
        }


        public async Task<IActionResult> PdfThongKeBn(string fo, string to)
        {

           
            var dataBn = await _context.BenhNhans.ToListAsync();

            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                dataBn = dataBn.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
                ViewBag.dataBn = dataBn;
                ViewBag.toDate = toDate.ToString("dd/MM/yyyy"); ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            //return View();

            return new ViewAsPdf("PdfThongKeBn", dataBn)

            {

                FileName = $"DanhSachBenhNhan ({fromDate.ToString("dd/MM/yyyy")}-{toDate.ToString("dd/MM/yyyy")}).pdf",
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "dataBn", dataBn },
                    }

            };
        }

        public async Task<IActionResult> ExcelThongKeBn( string fo, string to)
        {
            DateTime fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dataBn = await _context.BenhNhans.ToListAsync();
            dataBn = dataBn.Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)).ToList();
            ViewBag.dataBn = dataBn;
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy"); ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachbenhNhanMoi");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:H1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH BỆNH NHÂN MỚI (" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;
                titleRange.FirstCell().Value = titleRange.FirstCell().Value.ToString().ToUpper();

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bệnh nhân";
                worksheet.Cell("B2").Value = "Tên bệnh nhân";
                worksheet.Cell("C2").Value = "Năm sinh";
                worksheet.Cell("D2").Value = "Giới tinh";
                worksheet.Cell("E2").Value = "Địa chỉ";
                worksheet.Cell("F2").Value = "Sđt";
                worksheet.Cell("G2").Value = "Email";
                worksheet.Cell("H2").Value = "Ghi chú";
               



                worksheet.Range("A2:H2").Style.Font.Bold = true;
                worksheet.Range("A2:H2").Style.Font.FontSize = 13;
                var TongTien = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < dataBn.Count; i++)
                {
                    var bs = dataBn[i];
                    var currentRow = i + 3; var tinhtrang = "";
         
                    worksheet.Cell(currentRow, 1).Value = bs.MaBenhNhan;
                    worksheet.Cell(currentRow, 2).Value = bs.TenBn;
                    worksheet.Cell(currentRow, 3).Value = bs.NgaySinh;
                    worksheet.Cell(currentRow, 4).Value = bs.FormatNgaySinh;
                    worksheet.Cell(currentRow, 5).Value = bs.DiaChi;
                    worksheet.Cell(currentRow, 6).Value = bs.Sdt;
                    worksheet.Cell(currentRow, 7).Value = bs.Email;
                    worksheet.Cell(currentRow, 8).Value = bs.GhiChu;
                   

                }

               

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachBenhNhan (" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ").xlsx");
                }
            }
        }


        public async Task<IActionResult> ThongKeHoaDon(string? fo, string? to)
        {

            DateTime fromDate, toDate;

            if (fo == null && to == null)
            {
                fromDate = toDate = DateTime.Now.Date;
            }
            else
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var data = await _context.NoiDungThanhToans
                .Include(x=>x.IdndkhNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation).Include(x=>x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x=>x.IdhtttNavigation).Include(x=>x.IdhtttNavigation.IdnhNavigation)
                .Include(x=>x.IdttNavigation)
                .Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate)&& x.ThanhToan=="1")
                .ToListAsync();
            var tongtien = data.Sum(x => x.SoTienThanhToan);
            ViewBag.TongTienHD = tongtien.ToString("N0");
            ViewBag.dataHD = data;
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");

            return View();
        }
        public async Task<IActionResult> PdfThongKeHoaDon(string fo, string to)
        {


            DateTime fromDate, toDate;

            if (fo == null && to == null)
            {
                fromDate = toDate = DateTime.Now.Date;
            }
            else
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var data = await _context.NoiDungThanhToans
                .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdhtttNavigation).Include(x => x.IdhtttNavigation.IdnhNavigation)
                .Include(x => x.IdttNavigation)
                .Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.ThanhToan == "1")
                .ToListAsync();
           
           

            return new ViewAsPdf("PdfThongKeHoaDon", data)

            {

                FileName = $"DanhSachHoaDonThanhToan ({fromDate.ToString("dd/MM/yyyy")}-{toDate.ToString("dd/MM/yyyy")}).pdf",
                //PageOrientation = Orientation.Portrait,
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        { "FromDate", fromDate.ToString("dd/MM/yyyy") },
                        { "ToDate", toDate.ToString("dd/MM/yyyy") },
                        { "dataHD", data },
                    }

            };
        }

        public async Task<IActionResult> ExcelThongKeHoaDon(string fo, string to)
        {
            DateTime fromDate, toDate;

            if (fo == null && to == null)
            {
                fromDate = toDate = DateTime.Now.Date;
            }
            else
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var data = await _context.NoiDungThanhToans
                .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdhtttNavigation).Include(x => x.IdhtttNavigation.IdnhNavigation)
                .Include(x => x.IdttNavigation)
                .Where(x => x.NgayTao != null && (x.NgayTao.Value.Date >= fromDate && x.NgayTao.Value.Date <= toDate) && x.ThanhToan == "1")
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoCaoDanhSachHoaDon");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:K1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH BỆNH NHÂN MỚI (" + fromDate.ToString("dd/MM/yyyy") + " - " + toDate.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;
                titleRange.FirstCell().Value = titleRange.FirstCell().Value.ToString().ToUpper();

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã phiếu thanh toán";
                worksheet.Cell("B2").Value = "Mã phiếu thanh toán";
                worksheet.Cell("C2").Value = "Tên bệnh nhân";
                worksheet.Cell("D2").Value = " GT";
                worksheet.Cell("E2").Value = "Ngày sinh";
                worksheet.Cell("F2").Value = "Sđt";
                worksheet.Cell("G2").Value = "Tên công việc";
                worksheet.Cell("H2").Value = "Hình thức thanh toán";
                worksheet.Cell("I2").Value = "Số tiền thanh toán";
                worksheet.Cell("J2").Value = "Ngày thanh toán";
                worksheet.Cell("K2").Value = "Người thực thu";




                worksheet.Range("A2:K2").Style.Font.Bold = true;
                worksheet.Range("A2:K2").Style.Font.FontSize = 13;
                var tongtienhd = 0;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < data.Count; i++)
                {
                    var bs = data[i];
                    var currentRow = i + 3; var tinhtrang = "";
                    
                    worksheet.Cell(currentRow, 1).Value = bs.IdttNavigation.MaThanhToan;
                    worksheet.Cell(currentRow, 2).Value = bs.IdttNavigation.MaThanhToan;
                    worksheet.Cell(currentRow, 3).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn;
                    worksheet.Cell(currentRow, 4).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.GioiTinh;
                    worksheet.Cell(currentRow, 5).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.FormatNgaySinh;
                    worksheet.Cell(currentRow, 6).Value = bs.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Sdt;
                    worksheet.Cell(currentRow, 7).Value = bs.IdndkhNavigation.IdcvdtNavigation.TenCongViec;
                    worksheet.Cell(currentRow, 8).Value = bs.IdhtttNavigation.TenHttt + bs.IdhtttNavigation.IdnhNavigation.TenNganHang + bs.IdhtttNavigation.IdnhNavigation.TenTk;
                    worksheet.Cell(currentRow, 9).Value = bs.FormattedSoTienThanhToan;
                    worksheet.Cell(currentRow, 10).Value = bs.FormatNgayTao;
                    worksheet.Cell(currentRow, 11).Value = bs.Ntao;
                    tongtienhd += Convert.ToInt32(bs.SoTienThanhToan);

                }
                var lastRow = data.Count + 3;
                worksheet.Range($"A{lastRow}:H{lastRow}").Merge().Value = "Tổng";
                worksheet.Cell(lastRow, 9).Value = string.Format("{0:#,0}", tongtienhd);

                var style = worksheet.Range($"A{lastRow}:K{lastRow}").Style;
                style.Font.Bold = true;
                style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                style.Font.FontSize = 20;
                style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHoaDon (" + fromDate.ToString("dd/MM/yyyy") + "_" + toDate.ToString("dd/MM/yyyy") + ").xlsx");
                }
            }
        }
        //end
    }
}
