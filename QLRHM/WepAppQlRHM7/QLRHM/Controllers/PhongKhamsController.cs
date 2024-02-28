using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using QLRHM7.Models;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class PhongKhamsController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public PhongKhamsController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: PhongKhams
        [Authorize(Roles = "Admin,QuanLy,BacSi")]
        public async Task<IActionResult> Index(DateTime fromDate, DateTime toDate)
        {

            var fdate = fromDate.ToString("yyyyMMdd");
            var tdate = toDate.ToString("yyyyMMdd");
            var TimeNow = DateTime.Now.Date;
            if (fdate != "00010101" && tdate != "00010101" && int.Parse(fdate) <= int.Parse(tdate))
            {
                var PK = await _context.BenhNhans
                        .Where(bn => bn.Phong != 1 &&
                                     bn.NgaySua != null &&
                                     bn.NgaySua.Value.Date >= fromDate &&
                                     bn.NgaySua.Value.Date <= toDate &&
                                     !_context.LichHens.Any(lh => lh.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == bn.Idbn && lh.NgayHen >= TimeNow))
                        .OrderBy(x => x.NgaySua)
                        .ToListAsync();



                var PKLH = await _context.LichHens
                    .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.PhongNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                    .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Phong != 1
                    && x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua != null
                    && (x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua.Value.Date >= fromDate && x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua.Value.Date <= toDate)
                     && x.NgayHen >= TimeNow && x.TrangThai != 1
                    )
                    .OrderBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua)
                .GroupBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn) // Nhóm theo Idbn
                .Select(group => group.First()) // Chọn một đối tượng từ mỗi nhóm
                .ToListAsync();
                ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
                ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
                ViewBag.bn = PK;
                ViewBag.PKLH = PKLH;
            }
            else
            {
                var PK = await _context.BenhNhans
                .Where(bn => bn.Phong != 1 && bn.NgaySua != null && (bn.NgaySua.Value.Date >= TimeNow && bn.NgaySua.Value.Date <= TimeNow))
                .Where(bn => !_context.LichHens
                    .Any(lh =>
                        lh.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == bn.Idbn &&
                        lh.NgayHen >= TimeNow && lh.TrangThai != 1
                    )
                )
               .OrderBy(x => x.NgaySua)
                .ToListAsync();


                var PKLH = await _context.LichHens
                    .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation)
                    .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                    .Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.PhongNavigation)
                    .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Phong != 1 
                    && x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua != null
                    && (x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua.Value.Date >= TimeNow 
                    && x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua.Value.Date <= TimeNow)
                   && x.NgayHen >= TimeNow && x.TrangThai != 1
                    )
                     .OrderBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.NgaySua)
                .GroupBy(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn) // Nhóm theo Idbn
                .Select(group => group.First()) // Chọn một đối tượng từ mỗi nhóm
                .ToListAsync();


                ViewBag.fromDate = TimeNow.ToString("dd/MM/yyyy");
                ViewBag.toDate = TimeNow.ToString("dd/MM/yyyy");
                ViewBag.bn = PK;
                ViewBag.PKLH = PKLH;
            }

            var p = await _context.PhongKhams.Where(x => x.Id != 1).ToListAsync();
            var phongList = p.Select(x => new SelectListItem
            {
                Value = x.TenPhong, // Value là tên phòng
                Text = $"{x.TenPhong} (Sl: {SumBenhNhan(x.Id)})"
            }).ToList();

            ViewData["Phong"] = new SelectList(phongList, "Value", "Text");

            return View();
        }
        [Authorize(Roles = "Admin,QuanLy,TiepTan,BacSi")]
        public async Task<IActionResult> KHDT( int id)
        {
            
            var now = DateTime.Now.Date;

            var khdt = await _context.KeHoachDieuTris.Include(a => a.IdbnNavigation).Include(a => a.IdbsNavigation).Where(a => a.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idkhdt).ToListAsync();
            var benhNhan = await _context.BenhNhans.Where(x => x.Active == 1).FirstOrDefaultAsync(m => m.Idbn == id);
            var pttGroups1 = await _context.NoiDungThanhToans.Include(a => a.IdttNavigation).Include(a => a.IdttNavigation.IdbsNavigation).Include(a => a.IdndkhNavigation).Include(a => a.IdndkhNavigation.IdkhdtNavigation)
                .Include(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(a => a.IdndkhNavigation.IdcvdtNavigation)
                .Where(a => (a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") && a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id)
                .ToListAsync();
            var pttGroups = pttGroups1.GroupBy(a => a.IdttNavigation.Idtt).ToList();
            var lh = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idlh).ToListAsync();
            var Ndkh = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Where(x => x.IdkhdtNavigation.IdbnNavigation.Idbn == id && x.IdkhdtNavigation.DieuTri == "1").ToListAsync();

            // Duyệt qua từng nhóm và lấy một dòng từ mỗi nhóm
            var ptt = pttGroups.Select(group => group.FirstOrDefault(item => item.ThanhToan != "1") ?? group.First(item => item.ThanhToan == "1"));
            ptt = ptt.OrderByDescending(item => item.IdttNavigation.Idtt).ToList();
            //xác nhận phiếu tt
            var Xntt = pttGroups1.Where(x => (x.IdndkhNavigation.IdkhdtNavigation.DieuTri != null)).ToList();
            ViewBag.khdt = khdt; ViewBag.bn = benhNhan; ViewBag.ptt = ptt; ViewBag.lh = lh; ViewBag.Ndkh = Ndkh; ViewBag.Xntt = Xntt;

            int TongKhDangDT = khdt.Count(a => a.DieuTri == "1"); int TongKDaXong = khdt.Count(a => a.DieuTri == "3");
            int pttGroupsDTT = pttGroups.Count(a => a.Any(item => item.ThanhToan == "1")); int pttGroupsCTT = pttGroups.Count(a => a.Any(item => item.ThanhToan != "1"));

            int TongLH = lh.Count(a => a.NgayHen.Date >= now.Date);

            ViewBag.TongKhDangDT = TongKhDangDT; ViewBag.TongKDaXong = TongKDaXong; ViewBag.TongPhiDaTT = pttGroupsDTT; ViewBag.TongPhiChuaTT = pttGroupsCTT; ; ViewBag.TongLH = TongLH;
           
            return View(khdt);
        }


        //public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate)
        //{
        //    fromDate ??= DateTime.Today;
        //    toDate ??= DateTime.Today;

        //    if (fromDate <= toDate)
        //    {
        //        var PK = await _context.BenhNhans
        //            .Where(bn => bn.Phong != 1) // Điều kiện lọc phòng (tùy chọn)
        //            .Where(bn =>  !_context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
        //            .Any(lh => lh.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == bn.Idbn)) // Loại bỏ những bệnh nhân có lịch hẹn
        //            .OrderByDescending(x => x.NgaySua)
        //            .ToListAsync();

        //        var PKLH = await _context.LichHens
        //            .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.PhongNavigation)
        //            .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Phong != 1 && (x.NgaySua.Value.Date >= fromDate && x.NgaySua.Value.Date <= toDate))
        //            .OrderByDescending(x => x.NgaySua)
        //            .ToListAsync();
        //        ViewBag.fromDate = fromDate.Value.ToString("dd/MM/yyyy");
        //        ViewBag.toDate = toDate.Value.ToString("dd/MM/yyyy");
        //        ViewBag.bn = PK;
        //        ViewBag.PKLH = PKLH;
        //    }
        //    else
        //    {
        //        var PK = await _context.BenhNhans
        //            .Where(bn => bn.Phong != 1 && (bn.NgaySua.Value.Date >= DateTime.Now.Date && bn.NgaySua.Value.Date <= DateTime.Now.Date))
        //            .Where(bn =>  !_context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
        //            .Any(lh => lh.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == bn.Idbn)) // Loại bỏ những bệnh nhân có lịch hẹn
        //             .OrderByDescending(x => x.NgaySua)
        //            .ToListAsync();

        //        var PKLH = await _context.LichHens
        //            .Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.PhongNavigation)
        //            .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Phong != 1 && (x.NgaySua.Value.Date >= DateTime.Now.Date && x.NgaySua.Value.Date <= DateTime.Now.Date))
        //            .OrderByDescending(x => x.NgaySua)
        //            .ToListAsync();
        //        ViewBag.fromDate = fromDate.Value.ToString("dd/MM/yyyy");
        //        ViewBag.toDate = toDate.Value.ToString("dd/MM/yyyy");
        //        ViewBag.bn = PK;
        //        ViewBag.PKLH = PKLH;
        //    }

        //    var phongList = await _context.PhongKhams
        //        .Where(x => x.Id != 1)
        //        .Select(x => new SelectListItem
        //        {
        //            Value = x.TenPhong, // Value là tên phòng
        //            Text = $"{x.TenPhong} (Sl: {_context.BenhNhans.Count(b => b.Phong == x.Id)})"
        //        })
        //        .ToListAsync();

        //    ViewData["Phong"] = new SelectList(phongList, "Value", "Text");

        //    return View();
        //}

        private int SumBenhNhan(int phongId)
        {
            return _context.BenhNhans.Count(bn => bn.Phong == phongId);
        }
        public async Task<IActionResult> DanhSachPhong(PhongKham phong)
        {
          var PK = await _context.PhongKhams.Where(x=>x.Id != 1).ToListAsync();
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.PhongKhams.Any(x => x.MaPhong == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); 
            }
           
            ViewBag.GeneratedMaP = randomNumber;
            ViewBag.PK = PK;
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999);
            return "P0" + number.ToString().PadLeft(3, '0');
        }

        public async Task<IActionResult> ChonPhong(int id, BenhNhan benhNhan)
        {

            if (id != benhNhan.Idbn)
            {
                return NotFound();
            }

            // Lấy thông tin bệnh nhân từ cơ sở dữ liệu
            var existingBenhNhan = await _context.BenhNhans.FindAsync(id);

            if (existingBenhNhan == null)
            {
                return NotFound();
            }

            // Cập nhật phòng của bệnh nhân
            existingBenhNhan.Phong = benhNhan.Phong;
            existingBenhNhan.NgaySua = benhNhan.NgaySua;

            try
            {
                _context.Update(existingBenhNhan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhongKhamExists(benhNhan.Idbn))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "PhongKhams");
            //return View(existingBenhNhan);
        }
        // GET: PhongKhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PhongKhams == null)
            {
                return NotFound();
            }

            var phongKham = await _context.PhongKhams
               
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongKham == null)
            {
                return NotFound();
            }

            return PartialView("ViewTest",phongKham);
        }

        // GET: PhongKhams/Create
        public IActionResult Create()
        {
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn");
            return View();
        }

        // POST: PhongKhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idbn,MaPhong,TenPhong,NgayThem,NgaySua")] PhongKham phongKham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(phongKham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DanhSachPhong));
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", phongKham.Idbn);
            return RedirectToAction("DanhSachPhong");
        }

        // GET: PhongKhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PhongKhams == null)
            {
                return NotFound();
            }

            var phongKham = await _context.PhongKhams.FindAsync(id);
            if (phongKham == null)
            {
                return NotFound();
            }
            var PK = await _context.PhongKhams.Where(x => x.Id != 1).ToListAsync();
            ViewBag.PK = PK;
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", phongKham.Idbn);
            return View(phongKham);
        }

        // POST: PhongKhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idbn,MaPhong,TenPhong,NgayThem,NgaySua")] PhongKham phongKham)
        {
            if (id != phongKham.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phongKham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhongKhamExists(phongKham.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DanhSachPhong));
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", phongKham.Idbn);
            return View(phongKham);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongkham = await _context.PhongKhams
              
                .FirstOrDefaultAsync(m => m.Id == id);
            if (phongkham == null)
            {
                return NotFound();
            }

            _context.PhongKhams.Remove(phongkham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DanhSachPhong));
        }



        private bool PhongKhamExists(int id)
        {
          return (_context.PhongKhams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
