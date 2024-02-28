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
    public class NoiDungThanhToansController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public NoiDungThanhToansController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: NoiDungThanhToans
        public async Task<IActionResult> Index()
        {
            var datnqlrhmContext = _context.NoiDungThanhToans.Include(n => n.IdhtttNavigation).Include(n => n.IdndkhNavigation).Include(n => n.IdttNavigation);
            return View(await datnqlrhmContext.ToListAsync());
        }

        // GET: NoiDungThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NoiDungThanhToans == null)
            {
                return NotFound();
            }

            var noiDungThanhToan = await _context.NoiDungThanhToans
                .Include(n => n.IdhtttNavigation)
                .Include(n => n.IdndkhNavigation)
                .Include(n => n.IdttNavigation)
                .FirstOrDefaultAsync(m => m.Idndtt == id);
            if (noiDungThanhToan == null)
            {
                return NotFound();
            }

            return View(noiDungThanhToan);
        }

        // GET: NoiDungThanhToans/Create
        public IActionResult Create()
        {
            ViewData["Idhttt"] = new SelectList(_context.HinhThucThanhToans, "Idhttt", "Idhttt");
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh");
            ViewData["Idtt"] = new SelectList(_context.ThanhToans, "Idtt", "Idtt");
            return View();
        }

        // POST: NoiDungThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idndtt,Idtt,Idndkh,Idhttt,MaPhieuThanhToan,SoTienThanhToan,Ntao,Nsua,NgayTao,NgaySua")] NoiDungThanhToan noiDungThanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noiDungThanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idhttt"] = new SelectList(_context.HinhThucThanhToans, "Idhttt", "Idhttt", noiDungThanhToan.Idhttt);
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh);
            ViewData["Idtt"] = new SelectList(_context.ThanhToans, "Idtt", "Idtt", noiDungThanhToan.Idtt);
            return View(noiDungThanhToan);
        }

        // GET: NoiDungThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NoiDungThanhToans == null)
            {
                return NotFound();
            }

            var noiDungThanhToan = await _context.NoiDungThanhToans.FindAsync(id);
            if (noiDungThanhToan == null)
            {
                return NotFound();
            }
            ViewData["Idhttt"] = new SelectList(_context.HinhThucThanhToans, "Idhttt", "Idhttt", noiDungThanhToan.Idhttt);
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh);
            ViewData["Idtt"] = new SelectList(_context.ThanhToans, "Idtt", "Idtt", noiDungThanhToan.Idtt);
            return View(noiDungThanhToan);
        }

        // POST: NoiDungThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idndtt,Idtt,Idndkh,Idhttt,MaPhieuThanhToan,SoTienThanhToan,Ntao,Nsua,NgayTao,NgaySua")] NoiDungThanhToan noiDungThanhToan)
        {
            if (id != noiDungThanhToan.Idndtt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noiDungThanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoiDungThanhToanExists(noiDungThanhToan.Idndtt))
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
            ViewData["Idhttt"] = new SelectList(_context.HinhThucThanhToans, "Idhttt", "Idhttt", noiDungThanhToan.Idhttt);
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh);
            ViewData["Idtt"] = new SelectList(_context.ThanhToans, "Idtt", "Idtt", noiDungThanhToan.Idtt);
            return View(noiDungThanhToan);
        }

        // GET: NoiDungThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NoiDungThanhToans == null)
            {
                return NotFound();
            }

            var noiDungThanhToan = await _context.NoiDungThanhToans
                .Include(n => n.IdhtttNavigation)
                .Include(n => n.IdndkhNavigation)
                .Include(n => n.IdttNavigation)
                .FirstOrDefaultAsync(m => m.Idndtt == id);
            if (noiDungThanhToan == null)
            {
                return NotFound();
            }

            return View(noiDungThanhToan);
        }

        // POST: NoiDungThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NoiDungThanhToans == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.NoiDungThanhToans'  is null.");
            }
            var noiDungThanhToan = await _context.NoiDungThanhToans.FindAsync(id);
            if (noiDungThanhToan != null)
            {
                _context.NoiDungThanhToans.Remove(noiDungThanhToan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoiDungThanhToanExists(int id)
        {
          return (_context.NoiDungThanhToans?.Any(e => e.Idndtt == id)).GetValueOrDefault();
        }
    }
}
