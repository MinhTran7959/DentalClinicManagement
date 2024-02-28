using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class KeHoachDieuTrisController : Controller
    {
        private readonly DatnqlrhmContext _context;
        private readonly MasterNcvCvDbContext _DbContext;
        public KeHoachDieuTrisController(DatnqlrhmContext context, MasterNcvCvDbContext dbContext)
        {
            _context = context;
            _DbContext = dbContext;
        }

        // GET: KeHoachDieuTris
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            //var datnqlrhmContext = _context.KeHoachDieuTris.Include(k => k.IdbnNavigation).Include(k => k.IdbsNavigation);
            //return View(await datnqlrhmContext.ToListAsync());

            var khdt = await _context.NoiDungKeHoaches.Include(a=>a.IdbsdtNavigation)
                .Include(a=>a.IdcvdtNavigation)
                .Include(a=>a.IdkhdtNavigation)
                .Include(a=>a.IdkhdtNavigation.IdbnNavigation)
                .Include(a=>a.IdkhdtNavigation.IdbsNavigation)
                .OrderByDescending(a=>a.IdkhdtNavigation).ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                khdt = await _context.NoiDungKeHoaches.Where(x => (x.IdkhdtNavigation.MaKeHoacDieuTri.Contains(searchString))
                       || (x.IdkhdtNavigation.IdbsNavigation.TenBs.Contains(searchString))|| (x.IdkhdtNavigation.IdbsNavigation.MaBacSi.Contains(searchString))|| (x.IdkhdtNavigation.IdbsNavigation.Sdt.Contains(searchString))
                       || (x.IdkhdtNavigation.IdbnNavigation.TenBn.Contains(searchString))|| (x.IdkhdtNavigation.IdbnNavigation.Sdt.Contains(searchString))|| (x.IdkhdtNavigation.IdbnNavigation.MaBenhNhan.Contains(searchString)) 
                       || (x.IdbsdtNavigation.TenBs.Contains(searchString)) || (x.IdbsdtNavigation.MaBacSi.Contains(searchString)) || (x.IdbsdtNavigation.Sdt.Contains(searchString))
                       || (x.IdcvdtNavigation.TenCongViec.Contains(searchString)) || (x.IdcvdtNavigation.MaCongViec.Contains(searchString))
                       ).ToListAsync();

            }
            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(khdt.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.NoOfRecordsPerPage = NoOfRecordPerPage; // Thêm số lượng bản ghi trên mỗi trang vào ViewBag
            khdt = khdt.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
           
            ViewBag.khdt = khdt;
            return View();
        }

        // GET: KeHoachDieuTris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KeHoachDieuTris == null)
            {
                return NotFound();
            }

            var keHoachDieuTri = await _context.KeHoachDieuTris
                .Include(k => k.IdbnNavigation)
                .Include(k => k.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Idkhdt == id);
            if (keHoachDieuTri == null)
            {
                return NotFound();
            }

            return View(keHoachDieuTri);
        }

        // GET: KeHoachDieuTris/Create
        public IActionResult Create()
        {
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn");
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs");
            return View();
        }

        // POST: KeHoachDieuTris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idkhdt,Idbn,Idbs,MaKeHoacDieuTri,NgayLap,DieuTri,Nsua,NgaySua")] KeHoachDieuTri keHoachDieuTri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keHoachDieuTri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keHoachDieuTri.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keHoachDieuTri.Idbs);
            return View(keHoachDieuTri);
        }

        // GET: KeHoachDieuTris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KeHoachDieuTris == null)
            {
                return NotFound();
            }

            var keHoachDieuTri = await _context.KeHoachDieuTris.FindAsync(id);
            if (keHoachDieuTri == null)
            {
                return NotFound();
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keHoachDieuTri.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keHoachDieuTri.Idbs);
            return View(keHoachDieuTri);
        }

        // POST: KeHoachDieuTris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idkhdt,Idbn,Idbs,MaKeHoacDieuTri,NgayLap,DieuTri,Nsua,NgaySua")] KeHoachDieuTri keHoachDieuTri)
        {
            if (id != keHoachDieuTri.Idkhdt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keHoachDieuTri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeHoachDieuTriExists(keHoachDieuTri.Idkhdt))
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
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", keHoachDieuTri.Idbn);
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", keHoachDieuTri.Idbs);
            return View(keHoachDieuTri);
        }










        // GET: KeHoachDieuTris/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KeHoachDieuTris == null)
            {
                return NotFound();
            }

            var keHoachDieuTri = await _context.KeHoachDieuTris
                .Include(k => k.IdbnNavigation)
                .Include(k => k.IdbsNavigation)
                .FirstOrDefaultAsync(m => m.Idkhdt == id);
            if (keHoachDieuTri == null)
            {
                return NotFound();
            }

            return View(keHoachDieuTri);
        }

        // POST: KeHoachDieuTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KeHoachDieuTris == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.KeHoachDieuTris'  is null.");
            }
            var keHoachDieuTri = await _context.KeHoachDieuTris.FindAsync(id);
            if (keHoachDieuTri != null)
            {
                _context.KeHoachDieuTris.Remove(keHoachDieuTri);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeHoachDieuTriExists(int id)
        {
          return (_context.KeHoachDieuTris?.Any(e => e.Idkhdt == id)).GetValueOrDefault();
        }

        private string GenerateRandomNumber()
        {
            Random random = new Random();
            long number = (long)(random.NextDouble() * (999999999999 - 1)) + 1; 
            return "KHDT" + number.ToString().PadLeft(12, '0');
        }
        public async Task<IActionResult> CreateMaster()
        {
            KeHoachDieuTri KHDT = new KeHoachDieuTri();

            KHDT.MasterNd.Add(new NoiDungKeHoach());
            ViewBag.MasterNdkhList = KHDT.MasterNd; // Đặt danh sách vào ViewBag

            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).ToListAsync();

            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi")
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                });
       
            ViewData["Idbn"] = new SelectList(BenhNhan, "Idbn", "MaBenhNhan")
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BenhNhans.Find(int.Parse(x.Value)).TenBn} - " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}- " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).Sdt}"
                });
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });

            string randomNumber = GenerateRandomNumber();
            while (_context.KeHoachDieuTris.Any(x => x.MaKeHoacDieuTri == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            TempData["GeneratedMaKhđt"] = randomNumber;
            return View(KHDT);
        }


        [HttpPost]
        public IActionResult CreateMaster(KeHoachDieuTri keHoachDieuTri )
        {
            ViewData["Idbs"] = new SelectList(_context.BacSis.Where(d => d.Active == 1), "Idbs", "MaBacSi")
               .Select(x => new SelectListItem
               {
                   Value = x.Value,
                   Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
               });

            ViewData["Idbn"] = new SelectList(_context.BenhNhans.Where(d => d.Active == 1), "Idbn", "MaBenhNhan")
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BenhNhans.Find(int.Parse(x.Value)).TenBn} - " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}- " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).Sdt}"
                });
            ViewData["Idcvdt"] = new SelectList(_context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1), "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- "+
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).DonGiaSuDung}"
                });
          



           
            _context.Add(keHoachDieuTri);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> EditMaster(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            KeHoachDieuTri kehoachdieutri = await _context.KeHoachDieuTris.Include(e => e.MasterNd).Include(e=>e.IdbnNavigation).Include(e=>e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);
          

            if (kehoachdieutri == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).ToListAsync();

            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi",kehoachdieutri.Idbs)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });

            ViewData["Idbn"] = new SelectList(BenhNhan, "Idbn", "MaBenhNhan",kehoachdieutri.Idbn)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BenhNhans.Find(int.Parse(x.Value)).TenBn} - " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}- " +
                            $"{_context.BenhNhans.Find(int.Parse(x.Value)).Sdt}"
                });
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });
            return View(kehoachdieutri);
        }

        [HttpPost]
        public async Task<IActionResult> EditMaster(int id, KeHoachDieuTri keHoachDieuTri)
        {
            if (id != keHoachDieuTri.Idkhdt)
            {
                return BadRequest(); // Xử lý trường hợp id không khớp
            }

            List<NoiDungKeHoach> ND = await _context.NoiDungKeHoaches.Where(x=>x.Idkhdt==keHoachDieuTri.Idkhdt).ToListAsync();
            if (ND == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            _context.NoiDungKeHoaches.RemoveRange(ND);
            _context.SaveChanges();
            _context.Update(keHoachDieuTri);
            _context.SaveChanges();
            
           
            // Thực hiện kiểm tra hợp lệ cho dữ liệu và thêm lỗi vào ModelState nếu cần
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, quay lại view và hiển thị lỗi
                var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
                var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
                var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).ToListAsync();

                ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", keHoachDieuTri.Idbs)
                  .Select(x => new SelectListItem
                  {
                      Value = x.Value,
                      Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                  });

                ViewData["Idbn"] = new SelectList(BenhNhan, "Idbn", "MaBenhNhan", keHoachDieuTri.Idbn)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = $"{x.Text} - {_context.BenhNhans.Find(int.Parse(x.Value)).TenBn} - " +
                                $"{_context.BenhNhans.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}- " +
                                $"{_context.BenhNhans.Find(int.Parse(x.Value)).Sdt}"
                    });
                ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                                $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                                $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                    });
                return View(keHoachDieuTri);
            }

            // Cập nhật thông tin của ứng viên
            //NewKHDT.MaKeHoacDieuTri = keHoachDieuTri.MaKeHoacDieuTri;
            //NewKHDT.Idbs = keHoachDieuTri.Idbs;
            //NewKHDT.Idbn = keHoachDieuTri.Idbn;
            //NewKHDT.NgaySua = keHoachDieuTri.NgaySua;

            //NewKHDT.MasterNd.Clear();

            //foreach (var noiDungKeHoach in keHoachDieuTri.MasterNd)
            //{
            //    if (string.IsNullOrEmpty(noiDungKeHoach.NgayTao))
            //    {
            //        noiDungKeHoach.NgayTao = DateTime.Now.ToString(); // Chuyển đổi ngày hiện tại thành chuỗi và gán
            //    }

            //    NewKHDT.MasterNd.Add(noiDungKeHoach);
            //}

            // Không cần đính kèm keHoachDieuTri mới vào DbContext
            // Lưu thay đổi
            //await _context.SaveChangesAsync();
             //_context.ChangeTracker.DetectChanges();
            return RedirectToAction("Index");
        }

    }
}
