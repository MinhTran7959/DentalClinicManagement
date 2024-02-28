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
    public class NoiDungKeHoachesController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public NoiDungKeHoachesController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: NoiDungKeHoaches
        public async Task<IActionResult> Index()
        {
            var datnqlrhmContext = _context.NoiDungKeHoaches.Include(n => n.IdbsdtNavigation).Include(n => n.IdcvdtNavigation).Include(n => n.IdkhdtNavigation);
            return View(await datnqlrhmContext.ToListAsync());
        }

        // GET: NoiDungKeHoaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NoiDungKeHoaches == null)
            {
                return NotFound();
            }

            var noiDungKeHoach = await _context.NoiDungKeHoaches
                .Include(n => n.IdbsdtNavigation)
                .Include(n => n.IdcvdtNavigation)
                .Include(n => n.IdkhdtNavigation)
                .FirstOrDefaultAsync(m => m.Idndkh == id);
            if (noiDungKeHoach == null)
            {
                return NotFound();
            }

            return View(noiDungKeHoach);
        }

        // GET: NoiDungKeHoaches/Create
        public IActionResult Create()
        {
            ViewData["Idbsdt"] = new SelectList(_context.BacSis, "Idbs", "Idbs");
            ViewData["Idcvdt"] = new SelectList(_context.CongViecs, "Idcvdt", "MoTa");
            ViewData["Idkhdt"] = new SelectList(_context.KeHoachDieuTris, "Idkhdt", "Idkhdt");
            return View();
        }

        // POST: NoiDungKeHoaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idndkh,Idkhdt,Idcvdt,Idbsdt,SoLan,DonGia,GhiChu,Ntao,Nsua,NgayTao,NgaySua")] NoiDungKeHoach noiDungKeHoach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noiDungKeHoach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbsdt"] = new SelectList(_context.BacSis, "Idbs", "Idbs", noiDungKeHoach.Idbsdt);
            ViewData["Idcvdt"] = new SelectList(_context.CongViecs, "Idcvdt", "MoTa", noiDungKeHoach.Idcvdt);
            ViewData["Idkhdt"] = new SelectList(_context.KeHoachDieuTris, "Idkhdt", "Idkhdt", noiDungKeHoach.Idkhdt);
            return View(noiDungKeHoach);
        }

        // GET: NoiDungKeHoaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NoiDungKeHoaches == null)
            {
                return NotFound();
            }

            var noiDungKeHoach = await _context.NoiDungKeHoaches.FindAsync(id);
            if (noiDungKeHoach == null)
            {
                return NotFound();
            }
            ViewData["Idbsdt"] = new SelectList(_context.BacSis, "Idbs", "Idbs", noiDungKeHoach.Idbsdt);
            ViewData["Idcvdt"] = new SelectList(_context.CongViecs, "Idcvdt", "MoTa", noiDungKeHoach.Idcvdt);
            ViewData["Idkhdt"] = new SelectList(_context.KeHoachDieuTris, "Idkhdt", "Idkhdt", noiDungKeHoach.Idkhdt);
            return View(noiDungKeHoach);
        }

        // POST: NoiDungKeHoaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idndkh,Idkhdt,Idcvdt,Idbsdt,SoLan,DonGia,GhiChu,Ntao,Nsua,NgayTao,NgaySua")] NoiDungKeHoach noiDungKeHoach)
        {
            if (id != noiDungKeHoach.Idndkh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noiDungKeHoach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoiDungKeHoachExists(noiDungKeHoach.Idndkh))
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
            ViewData["Idbsdt"] = new SelectList(_context.BacSis, "Idbs", "Idbs", noiDungKeHoach.Idbsdt);
            ViewData["Idcvdt"] = new SelectList(_context.CongViecs, "Idcvdt", "MoTa", noiDungKeHoach.Idcvdt);
            ViewData["Idkhdt"] = new SelectList(_context.KeHoachDieuTris, "Idkhdt", "Idkhdt", noiDungKeHoach.Idkhdt);
            return View(noiDungKeHoach);
        }

        // GET: NoiDungKeHoaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NoiDungKeHoaches == null)
            {
                return NotFound();
            }

            var noiDungKeHoach = await _context.NoiDungKeHoaches
                .Include(n => n.IdbsdtNavigation)
                .Include(n => n.IdcvdtNavigation)
                .Include(n => n.IdkhdtNavigation)
                .FirstOrDefaultAsync(m => m.Idndkh == id);
            if (noiDungKeHoach == null)
            {
                return NotFound();
            }

            return View(noiDungKeHoach);
        }

        // POST: NoiDungKeHoaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NoiDungKeHoaches == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.NoiDungKeHoaches'  is null.");
            }
            var noiDungKeHoach = await _context.NoiDungKeHoaches.FindAsync(id);
            if (noiDungKeHoach != null)
            {
                _context.NoiDungKeHoaches.Remove(noiDungKeHoach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoiDungKeHoachExists(int id)
        {
          return (_context.NoiDungKeHoaches?.Any(e => e.Idndkh == id)).GetValueOrDefault();
        }
    }
}
