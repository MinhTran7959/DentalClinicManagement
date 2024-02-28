using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class ThanhToansController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public ThanhToansController(DatnqlrhmContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,QuanLy,ThuNgan")]
        // GET: ThanhToans
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {

            var pttGroups1 = await _context.NoiDungThanhToans.Include(a => a.IdttNavigation).Include(a => a.IdttNavigation.IdbsNavigation).Include(a => a.IdndkhNavigation).Include(a => a.IdndkhNavigation.IdkhdtNavigation)
               .Include(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(a => a.IdndkhNavigation.IdcvdtNavigation)
               .Where(a => (a.IdndkhNavigation.IdkhdtNavigation.DieuTri != null))
               .ToListAsync();
            var pttGroups = pttGroups1.GroupBy(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn).ToList();
            var ptt = pttGroups.Select(group => group.FirstOrDefault() ?? group.First());

            ptt = ptt.OrderByDescending(item => item.NgayTao).ToList();

            ViewBag.bn = ptt;
           
            
            return View();
        }
        public async Task<IActionResult> PhieuThanhToan(int id)
        {
            var now = DateTime.Now.Date;

            
            var benhNhan = await _context.BenhNhans.Where(x => x.Active == 1).FirstOrDefaultAsync(m => m.Idbn == id);
            var pttGroups1 = await _context.NoiDungThanhToans.Include(a => a.IdttNavigation).Include(a => a.IdttNavigation.IdbsNavigation).Include(a => a.IdndkhNavigation).Include(a => a.IdndkhNavigation.IdkhdtNavigation)
                .Include(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(a => a.IdndkhNavigation.IdcvdtNavigation)
                .Where(a => (a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") && a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id)
                .ToListAsync();
            var pttGroups = pttGroups1.GroupBy(a => a.IdttNavigation.Idtt).ToList();
           
           
            // Duyệt qua từng nhóm và lấy một dòng từ mỗi nhóm
            var ptt = pttGroups.Select(group => group.FirstOrDefault(item => item.ThanhToan != "1") ?? group.First());
            ptt = ptt.OrderByDescending(item => item.IdttNavigation.Idtt).ToList();
            //xác nhận phiếu tt
            var Xntt = pttGroups1.Where(x => (x.IdndkhNavigation.IdkhdtNavigation.DieuTri != null)).ToList();
           ; ViewBag.bn = benhNhan; ViewBag.ptt = ptt; 

           
            
            return View(PhieuThanhToan);
        }
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(int id, int id2, NoiDungThanhToan noiDungThanhToan)
        {

            if (id != noiDungThanhToan.Idndtt)
            {
                 return RedirectToAction("Bug", "_404");
            }

            // Lấy thông tin bệnh nhân từ cơ sở dữ liệu
            var ndtt = await _context.NoiDungThanhToans.FindAsync(id);
            var bn = await _context.BenhNhans.FindAsync(id2);

            if (ndtt == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            // Cập nhật phòng của bệnh nhân
            ndtt.ThanhToan = "1";
            //ndtt.NgaySua = DateTime.Now.Date;
            ndtt.ThanhToan = noiDungThanhToan.ThanhToan;
            ndtt.NgaySua = noiDungThanhToan.NgaySua;
            ndtt.NgayTao = noiDungThanhToan.NgayTao;


            _context.Update(ndtt);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "ThanhToans");

        }



        [HttpGet]
        public async Task<IActionResult> SuaPhieuThanhToan(int? id, int id2, NoiDungThanhToan noiDungThanhToan)
        {
            if (id == null)
            {
                 return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var thanhToan = await _context.ThanhToans.Include(e => e.MasterNdtt).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idtt == id);


            if (thanhToan == null)
            {
                 return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id2
                && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            ViewBag.BenhNhan = BenhNhan;
            return View(thanhToan);
        }
        [HttpPost]
        public async Task<IActionResult> SuaPhieuThanhToan(int id, ThanhToan thanhToan, BenhNhan bn, int id2, NoiDungThanhToan noiDungThanhToan)
        {
            if (id != thanhToan.Idtt)
            {
                return BadRequest(); // Xử lý trường hợp id không khớp
            }

            List<NoiDungThanhToan> NDTT = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation).Where(x => x.Idtt == thanhToan.Idtt).ToListAsync();

            if (NDTT == null || !NDTT.Any())
            {
                 return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy hoặc không có dữ liệu thỏa mãn điều kiện
            }
            if (ModelState.IsValid)
            {
                try
                {

                    

                    _context.NoiDungThanhToans.RemoveRange(NDTT);
                    _context.SaveChanges();
                    thanhToan.MasterNdtt.RemoveAll(x => x.IsDelete == true);
                   
                  
                    _context.Update(thanhToan);
                    _context.SaveChanges();

                    return RedirectToAction("PhieuThanhToan", "ThanhToans", new { id = bn.Idbn });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.Idtt))
                    {
                         return RedirectToAction("Bug", "_404");
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id2
                && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            ViewBag.BenhNhan = BenhNhan;
            return View(thanhToan);
        }

        // GET: ThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Idtt == id);
            if (thanhToan == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public IActionResult Create()
        {
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs");
            return View();
        }

        // POST: ThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idtt,Idbs,MaThanhToan,NgayThanhToan,Nsua,NgaySua")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", thanhToan.Idbs);
            return View(thanhToan);
        }

        // GET: ThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan == null)
            {
                 return RedirectToAction("Bug", "_404");
            }
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", thanhToan.Idbs);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idtt,Idbs,MaThanhToan,NgayThanhToan,Nsua,NgaySua")] ThanhToan thanhToan)
        {
            if (id != thanhToan.Idtt)
            {
                 return RedirectToAction("Bug", "_404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.Idtt))
                    {
                         return RedirectToAction("Bug", "_404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", thanhToan.Idbs);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ThanhToans == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Idtt == id);
            if (thanhToan == null)
            {
                 return RedirectToAction("Bug", "_404");
            }

            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ThanhToans == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.ThanhToans'  is null.");
            }
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(int id)
        {
          return (_context.ThanhToans?.Any(e => e.Idtt == id)).GetValueOrDefault();
        }
    }
}
