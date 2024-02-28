using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using System.Diagnostics;

namespace QLRHM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatnqlrhmContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(DatnqlrhmContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

      

        public async Task< IActionResult> Index()
    {
            using (var _context = new DatnqlrhmContext()) // Thay Your_context bằng _context của bạn
            {
                //var result = _context.NoiDungThanhToans
                //    .Where(ntt => ntt.NgayTao.Value.Year == DateTime.Now.Year)
                //    .GroupBy(ntt => ntt.NgayTao.Value.Month)
                //    .Select(group => new ThangTongTien
                //    {
                //        Thang = group.Key,
                //        TongTien = group.Sum(ntt => ntt.SoTienThanhToan)
                //    })
                //    .OrderBy(tt => tt.Thang)
                //    .ToList();

                var result = await _context.NoiDungThanhToans
                    .Join(_context.CongViecs,
                        ntt => ntt.IdndkhNavigation.IdcvdtNavigation.Idcvdt,
                        cv => cv.Idcvdt,
                        (ntt, cv) => new { ntt, cv })
                    .Join(_context.NoiDungKeHoaches,
                        joined => joined.ntt.Idndkh,
                        ndkh => ndkh.Idndkh,
                        (joined, ndkh) => new { joined.ntt, joined.cv, ndkh })
                    .Join(_context.KeHoachDieuTris,
                        joined => joined.ntt.IdndkhNavigation.IdkhdtNavigation.Idkhdt,
                        khdt => khdt.Idkhdt,
                        (joined, khdt) => new { joined.ntt, joined.cv, joined.ndkh, khdt })
                    .Join(_context.BenhNhans,
                        joined => joined.ntt.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn,
                        bn => bn.Idbn,
                        (joined, bn) => new { joined.ntt, joined.cv, joined.ndkh, joined.khdt, bn })
                    .Where(result => result.ntt.NgayTao != null && result.ntt.NgayTao.Value.Year == DateTime.Now.Year )
                    .GroupBy(result => result.ntt.NgayTao.Value.Month)
                    .Select(group => new QLRHM.Models.ThangTongTien
                    {
                        Thang = group.Key,
                        TongTien = group.Sum(result => result.ntt.SoTienThanhToan),
                        TongKeHoach = group.Select(result => result.ntt.IdndkhNavigation.IdkhdtNavigation.Idkhdt).Distinct().Count(),
                        TongCV = group.Select(result => result.ntt.IdndkhNavigation.IdcvdtNavigation.Idcvdt).Count(),
                    })
                    .OrderBy(tt => tt.Thang)
                    .ToListAsync();

                var resultLH = await _context.LichHens

                   .Where(result => result.NgayTao != null && result.NgayTao.Value.Year == DateTime.Now.Year)
                    .GroupBy(result => result.NgayTao.Value.Month)
                    .Select(group => new QLRHM.Models.ThangTongTien
                    {
                        Thang = group.Key,
                        TongLH = group.Select(result => result.Idlh).Count(),
                    })
                    .OrderBy(tt => tt.Thang)
                    .ToListAsync();
                var resultBN = await _context.BenhNhans
               .Where(result => result.NgayTao != null && result.NgayTao.Value.Year == DateTime.Now.Year)
               .GroupBy(result => result.NgayTao.Value.Month)
               .Select(group => new QLRHM.Models.ThangTongTien
               {
                   Thang = group.Key,
                   TongBN = group.Select(result => result.Idbn).Count(),
               })
               .OrderBy(tt => tt.Thang)
               .ToListAsync();

                var ThangTongTien = result.Concat(resultLH)
                    .Concat(resultBN)
                    .GroupBy(c => c.Thang)
                    .Select(group => new ThangTongTien
                    {
                        Thang = group.Key,
                        TongTien = group.Sum(c => c.TongTien),
                        TongKeHoach = group.Sum(c => c.TongKeHoach),
                        TongCV = group.Sum(c => c.TongCV),
                        TongLH = group.Sum(c => c.TongLH),
                        TongBN = group.Sum(c => c.TongBN),
                    })
                    
                    .OrderBy(tt => tt.Thang)
                    .ToList();


                return View(ThangTongTien); 
            }
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}