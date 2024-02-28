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
    public class ThuocsController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public ThuocsController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: Thuocs
        public async Task<IActionResult> Index()
        {
            var thuoc = await _context.Thuocs.Where(x => x.active == 1).OrderByDescending(x => x.Id).ToListAsync();
            var thuoc2 = await _context.Thuocs.Where(x => x.active == 0).OrderByDescending(x => x.Id).ToListAsync();
            
            string randomNumber = GenerateRandomNumber();
          
            while (_context.Thuocs.Any(x => x.MaThuoc == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); 
            }
            var T = new Thuoc(); 
            T.MaThuoc = randomNumber; 
            ViewBag.GeneratedT = randomNumber;
            ViewBag.T = thuoc;
            ViewBag.T2 = thuoc2;
           
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "T" + number.ToString().PadLeft(9, '0');
        }
        // GET: Thuocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Thuocs == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // GET: Thuocs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Thuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaThuoc,TenThuoc,DonViTinh,active")] Thuoc thuoc)
        {
            if (ModelState.IsValid)
            {
                thuoc.active = 1;
                _context.Add(thuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thuoc);
        }

        // GET: Thuocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Thuocs == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null)
            {
                return NotFound();
            }
            return PartialView("Edit",thuoc);
        }
        public IActionResult hide(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            Thuoc thuoc = _context.Thuocs.FirstOrDefault(a => a.Id == id);

            if (thuoc == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            thuoc.active = 0;

            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
        public IActionResult active(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            Thuoc thuoc = _context.Thuocs.FirstOrDefault(a => a.Id == id);

            if (thuoc == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            thuoc.active = 1;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Thuocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MaThuoc,TenThuoc,DonViTinh,active")] Thuoc thuoc)
        {
            if (id != thuoc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThuocExists(thuoc.Id))
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
            return View(thuoc);
        }

        // GET: Thuocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Thuocs == null)
            {
                return NotFound();
            }

            var thuoc = await _context.Thuocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thuoc == null)
            {
                return NotFound();
            }

            return View(thuoc);
        }

        // POST: Thuocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thuocs == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.Thuocs'  is null.");
            }
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc != null)
            {
                _context.Thuocs.Remove(thuoc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThuocExists(int id)
        {
          return (_context.Thuocs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
