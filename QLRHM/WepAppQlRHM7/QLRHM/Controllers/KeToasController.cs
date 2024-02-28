using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace QLRHM7.Controllers
{
    public class KeToasController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public KeToasController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: KeToas
        public async Task<IActionResult> Index()
        {
            var datnqlrhmContext = _context.KeToas.Include(k => k.IdbnNavigation).Include(k => k.IdbsNavigation);
            return View(await datnqlrhmContext.ToListAsync());
        }

        public async Task<IActionResult> TaoToaThuoc( int id2)
        {
            KeToa KeToa = new KeToa();

            KeToa.MasterTTCT.Add(new ToaThuocChiTiet());
            ViewBag.MasterTTCTList = KeToa.MasterTTCT; // Đặt danh sách vào ViewBag

            var BenhNhan = await _context.BenhNhans.FirstOrDefaultAsync(x=>x.Idbn == id2);
            var Thuoc = await _context.Thuocs.Where(x=>x.active ==1).ToListAsync();
            ViewData["Thuoc"] = new SelectList(Thuoc, "Id", "TenThuoc")
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text}/{_context.Thuocs.Find(int.Parse(x.Value)).DonViTinh}"
                });
            ViewBag.BenhNhan = BenhNhan; ViewBag.MaToaThuoc = MaToaThuoc();
            return View(KeToa);
        }
        [HttpPost]
        public async Task<IActionResult> TaoToaThuoc(int Idbn, KeToa keToa)
        {
            try
            {

                keToa.MasterTTCT.RemoveAll(x => x.IsDelete == true);
                _context.Add(keToa);
                _context.SaveChanges();

                return RedirectToAction("KHDT", "BenhNhans", new { id = Idbn });

            }
            catch (Exception)
            {
                // Xử lý lỗi ở đây, ví dụ: ghi log, thông báo người dùng, v.v.
                // Sau đó có thể chuyển hướng đến trang "Bug" hoặc một trang thông báo lỗi khác.
                return RedirectToAction("Bug", "_404");
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> SuaToaThuoc(int? id, int id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            KeToa ketoa = await _context.KeToas.Include(e => e.MasterTTCT).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Id == id);


            if (ketoa == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
          
            var Thuoc = await _context.Thuocs.Where(d => d.active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewBag.BenhNhan= BenhNhan;
            ViewData["Thuoc"] = new SelectList(Thuoc, "Id", "TenThuoc")
               .Select(x => new SelectListItem
               {
                   Value = x.Value,
                   Text = $"{x.Text} / {_context.Thuocs.Find(int.Parse(x.Value)).DonViTinh}"
               });
            return View(ketoa);
        }
        [HttpPost]
        public async Task<IActionResult> SuaToaThuoc(int id, int id2, KeToa keToa, BenhNhan bn)
        {
            if (id != keToa.Id)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id không khớp
            }
            //var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id);
            //ViewBag.BenhNhan = BenhNhan;
            List<ToaThuocChiTiet> TTCT = await _context.ToaThuocChiTiets.Where(x => x.Idtoa == keToa.Id).ToListAsync();
            if (TTCT == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, quay lại view và hiển thị lỗi
                var Thuoc = await _context.Thuocs.Where(d => d.active == 1).ToListAsync();
                var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);

                ViewData["Thuoc"] = new SelectList(Thuoc, "Id", "TenThuoc")
               .Select(x => new SelectListItem
               {
                   Value = x.Value,
                   Text = $"{x.Text} / {_context.Thuocs.Find(int.Parse(x.Value)).DonViTinh}"
               });
                return View(keToa);
            }


            _context.ToaThuocChiTiets.RemoveRange(TTCT);
            _context.SaveChanges();

            keToa.MasterTTCT.RemoveAll(x => x.IsDelete == true);
            _context.Update(keToa);
            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });

        }

        [HttpGet]
        public async Task<IActionResult> ChiTietToaThuoc(int? id, int id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            KeToa ketoa = await _context.KeToas.Include(e => e.MasterTTCT).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Id == id);


            if (ketoa == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }

            var Thuoc = await _context.Thuocs.Where(d => d.active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewBag.BenhNhan = BenhNhan;
            ViewData["Thuoc"] = new SelectList(Thuoc, "Id", "TenThuoc")
               .Select(x => new SelectListItem
               {
                   Value = x.Value,
                   Text = $"{x.Text} / {_context.Thuocs.Find(int.Parse(x.Value)).DonViTinh}"
               });
            //return View(ketoa);

            return new ViewAsPdf("ChiTietToaThuoc", ketoa)

            {

                FileName = $"ToaThuoc.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {

                        { "BenhNhan", BenhNhan },
                         { "Thuoc", ViewData["Thuoc"] },
                    }

            };

        }
        public async Task<IActionResult> XoaToaThuoc(int? id, int id2 )
        {
            var xoaketoa = await _context.KeToas
                .Include(e => e.MasterTTCT)
                
                .FirstOrDefaultAsync(a => a.Id == id);

            if (xoaketoa == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            _context.ToaThuocChiTiets.RemoveRange(xoaketoa.MasterTTCT);
            _context.KeToas.Remove(xoaketoa);
            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
        }


        private string MaToaThuoc()
        {
            // Lấy ngày tháng năm hiện tại
            string ngayThangNam = DateTime.Now.ToString("ddMMyy");

            // Kiểm tra xem có phiếu cùng ngày không
            var Bn = _context.KeToas.OrderByDescending(x => x.MaToaThuoc).FirstOrDefault(p => p.MaToaThuoc.StartsWith($"BN{ngayThangNam}"));

            if (Bn != null)
            {
                // Tách lấy số phiếu hiện tại
                string MaBN = Bn.MaToaThuoc.Substring(8);
                int NewMaBn = int.Parse(MaBN) + 1;
                return $"TT{ngayThangNam}{NewMaBn:D4}";
            }
            else
            {
                // Nếu không có phiếu cùng ngày, bắt đầu từ 1
                return $"TT{ngayThangNam}0001";
            }
        }



        // GET: KeToas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KeToas == null)
            {
                return NotFound();
            }

            var keToa = await _context.KeToas
                .Include(k => k.IdbnNavigation)
                .Include(k => k.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keToa == null)
            {
                return NotFound();
            }

            return View(keToa);
        }

        // GET: KeToas/Create
        public IActionResult Create()
        {
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn");
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs");
            return View();
        }

        // POST: KeToas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idbs,Idbn,MaToaThuoc,NgayLap,ChuanDoan")] KeToa keToa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keToa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keToa.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keToa.Idbs);
            return View(keToa);
        }

        // GET: KeToas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KeToas == null)
            {
                return NotFound();
            }

            var keToa = await _context.KeToas.FindAsync(id);
            if (keToa == null)
            {
                return NotFound();
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keToa.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keToa.Idbs);
            return View(keToa);
        }

        // POST: KeToas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idbs,Idbn,MaToaThuoc,NgayLap,ChuanDoan")] KeToa keToa)
        {
            if (id != keToa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keToa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeToaExists(keToa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keToa.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keToa.Idbs);
            return View(keToa);
        }

        // GET: KeToas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KeToas == null)
            {
                return NotFound();
            }

            var keToa = await _context.KeToas
                .Include(k => k.IdbnNavigation)
                .Include(k => k.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keToa == null)
            {
                return NotFound();
            }

            return View(keToa);
        }

        // POST: KeToas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KeToas == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.KeToas'  is null.");
            }
            var keToa = await _context.KeToas.FindAsync(id);
            if (keToa != null)
            {
                _context.KeToas.Remove(keToa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeToaExists(int id)
        {
          return (_context.KeToas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
