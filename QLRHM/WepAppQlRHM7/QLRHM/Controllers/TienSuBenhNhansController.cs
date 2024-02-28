using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TienSuBenhNhansController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public TienSuBenhNhansController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: TienSuBenhNhans
        public async Task<IActionResult> Index()
        {
            var datnqlrhmContext = _context.TienSuBenhNhans.Include(t => t.IdbnNavigation);
            return View(await datnqlrhmContext.ToListAsync());
        }

        // GET: TienSuBenhNhans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TienSuBenhNhans == null)
            {
                return NotFound();
            }

            var tienSuBenhNhan = await _context.TienSuBenhNhans
                .Include(t => t.IdbnNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tienSuBenhNhan == null)
            {
                return NotFound();
            }

            return View(tienSuBenhNhan);
        }

        // GET: TienSuBenhNhans/Create
        public IActionResult Create( int id2)
        {
            ViewData["Idbn"] = new SelectList(_context.BenhNhans.Where(x=>x.Idbn == id2), "Idbn", "Idbn");
            return View();
        }

        // POST: TienSuBenhNhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idbn,TimMach,TieuDuong,CaoHuyetAp,TruyenNhiem,DiUngThuoc,Khac")] int id2, TienSuBenhNhan tienSuBenhNhan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tienSuBenhNhan);
                await _context.SaveChangesAsync();
                return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans.Where(x => x.Idbn == id2), "Idbn", "Idbn");
            return View(tienSuBenhNhan);
        }

        // GET: TienSuBenhNhans/Edit/5
        public async Task<IActionResult> Edit(int? id ,int id2)
        {
            if (id == null || _context.TienSuBenhNhans == null)
            {
                return NotFound();
            }
            ViewBag.Idbn2 = id2;
            var tienSuBenhNhan = await _context.TienSuBenhNhans.FindAsync(id);
            if (tienSuBenhNhan == null)
            {
                return NotFound();
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", tienSuBenhNhan.Idbn);
            //return View(tienSuBenhNhan);
            return PartialView("Edit", tienSuBenhNhan);
        }

        // POST: TienSuBenhNhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id2, [Bind("Id,Idbn,TimMach,TieuDuong,CaoHuyetAp,TruyenNhiem,DiUngThuoc,Khac")] TienSuBenhNhan tienSuBenhNhan)
        {
            if (id != tienSuBenhNhan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tienSuBenhNhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TienSuBenhNhanExists(tienSuBenhNhan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
            }
            ViewData["Idbn"] = new SelectList(_context.BenhNhans, "Idbn", "Idbn", tienSuBenhNhan.Idbn);
            return View(tienSuBenhNhan);
        }

        // GET: TienSuBenhNhans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TienSuBenhNhans == null)
            {
                return NotFound();
            }

            var tienSuBenhNhan = await _context.TienSuBenhNhans
                .Include(t => t.IdbnNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tienSuBenhNhan == null)
            {
                return NotFound();
            }

            return View(tienSuBenhNhan);
        }

        // POST: TienSuBenhNhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TienSuBenhNhans == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.TienSuBenhNhans'  is null.");
            }
            var tienSuBenhNhan = await _context.TienSuBenhNhans.FindAsync(id);
            if (tienSuBenhNhan != null)
            {
                _context.TienSuBenhNhans.Remove(tienSuBenhNhan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TienSuBenhNhanExists(int id)
        {
          return (_context.TienSuBenhNhans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
