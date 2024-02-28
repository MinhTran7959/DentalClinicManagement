using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;

namespace QLRHM.Controllers
{
    [Authorize]
    public class NganHangsController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public NganHangsController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: NganHangs
        public async Task<IActionResult> Index()
        {
             var nh = await _context.NganHangs.Where(x=>x.Active==1).OrderByDescending(x => x.Idnh).ToListAsync();
             var nhhide = await _context.NganHangs.Where(x=>x.Active==0).OrderByDescending(x => x.Idnh).ToListAsync();
        
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.CongViecs.Any(x => x.MaCongViec == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var nganhang = new NganHang(); // Tạo một đối tượng NhomCongViec mới
            nganhang.MaNganHang = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.GeneratedNh = randomNumber;
            ViewBag.nh = nh;
            ViewBag.nhhide = nhhide;
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "NH" + number.ToString().PadLeft(9, '0');
        }
        // GET: NganHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NganHangs == null)
            {
                return NotFound();
            }

            var nganHang = await _context.NganHangs
                .FirstOrDefaultAsync(m => m.Idnh == id);
            if (nganHang == null)
            {
                return NotFound();
            }

            return View(nganHang);
        }

        // GET: NganHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NganHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idnh,MaNganHang,TenNganHang,SoTk,TenTk,Ntao,Nsua,NgayTao,NgaySua,Active")] NganHang nganHang)
        {
            if (ModelState.IsValid)
            {
                nganHang.Active = 1;
                _context.Add(nganHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nganHang);
        }

        // GET: NganHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NganHangs == null)
            {
                return NotFound();
            }

            var nganHang = await _context.NganHangs.FindAsync(id);
            if (nganHang == null)
            {
                return NotFound();
            }
            var nh = await _context.NganHangs.Where(x => x.Active == 1).OrderByDescending(x => x.Idnh).ToListAsync();
            var nhhide = await _context.NganHangs.Where(x => x.Active == 0).OrderByDescending(x => x.Idnh).ToListAsync();
            ViewBag.nh = nh;
            ViewBag.nhhide = nhhide;
            return View(nganHang);
        }

        // POST: NganHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idnh,MaNganHang,TenNganHang,SoTk,TenTk,Ntao,Nsua,NgayTao,NgaySua,Active")] NganHang nganHang)
        {
            if (id != nganHang.Idnh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nganHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NganHangExists(nganHang.Idnh))
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
            var nh = await _context.NganHangs.Where(x => x.Active == 1).OrderByDescending(x => x.Idnh).ToListAsync();
            var nhhide = await _context.NganHangs.Where(x => x.Active == 0).OrderByDescending(x => x.Idnh).ToListAsync();
            ViewBag.nh = nh;
            ViewBag.nhhide = nhhide;
            return View(nganHang);
        }

        // GET: NganHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NganHangs == null)
            {
                return NotFound();
            }

            var nganHang = await _context.NganHangs
                .FirstOrDefaultAsync(m => m.Idnh == id);
            if (nganHang == null)
            {
                return NotFound();
            }

            return View(nganHang);
        }

        // POST: NganHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NganHangs == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.NganHangs'  is null.");
            }
            var nganHang = await _context.NganHangs.FindAsync(id);
            if (nganHang != null)
            {
                _context.NganHangs.Remove(nganHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NganHangExists(int id)
        {
          return (_context.NganHangs?.Any(e => e.Idnh == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            var nganHang = await _context.NganHangs.FirstOrDefaultAsync(a => a.Idnh == id);

            if (nganHang == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nganHang.Active = 0;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            var nganHang = await _context.NganHangs.FirstOrDefaultAsync(a => a.Idnh == id);

            if (nganHang == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nganHang.Active = 1;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
