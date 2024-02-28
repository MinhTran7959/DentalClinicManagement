using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using static System.Net.WebRequestMethods;

namespace QLRHM.Controllers
{
    [Authorize]
    public class HinhThucThanhToansController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public HinhThucThanhToansController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: HinhThucThanhToans
        public async Task<IActionResult> Index()
        {
           var httt = await _context.HinhThucThanhToans.Include(x=>x.IdnhNavigation).Where(x=>x.IdnhNavigation.Active==1).OrderByDescending(x=>x.Idhttt).ToListAsync();
            ViewBag.httt= httt;
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.HinhThucThanhToans.Any(x => x.MaHttt == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var hinhthuctt = new HinhThucThanhToan(); // Tạo một đối tượng NhomCongViec mới
            hinhthuctt.MaHttt = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.Generatedhttt = randomNumber;
            ViewBag.nh = httt;
            ViewData["NH"] = new SelectList(_context.NganHangs.Where(w => w.Active == 1), "Idnh", "TenNganHang");
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "HTTT" + number.ToString().PadLeft(9, '0');
        }
        // GET: HinhThucThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HinhThucThanhToans == null)
            {
                return NotFound();
            }

            var HinhThucThanhToan = await _context.HinhThucThanhToans
                .Include(h => h.IdnhNavigation)
                .FirstOrDefaultAsync(m => m.Idhttt == id);
            if (HinhThucThanhToan == null)
            {
                return NotFound();
            }

            return View(HinhThucThanhToan);
        }

        // GET: HinhThucThanhToans/Create
        public IActionResult Create()
        {
            ViewData["Idnh"] = new SelectList(_context.NganHangs, "Idnh", "Idnh");
            return View();
        }

        // POST: HinhThucThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idhttt,Idnh,MaHttt,TenHttt,Ntao,Nsua,NgayTao,NgaySua")] HinhThucThanhToan HinhThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(HinhThucThanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NH"] = new SelectList(_context.NganHangs.Where(w => w.Active == 1), "Idnh", "TenNganHang", HinhThucThanhToan.Idnh);
            //ViewData["Idnh"] = new SelectList(_context.NganHangs, "Idnh", "Idnh", HinhThucThanhToan.Idnh);
            return View(HinhThucThanhToan);
        }

        // GET: HinhThucThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HinhThucThanhToans == null)
            {
                return NotFound();
            }
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).OrderByDescending(x => x.Idhttt).ToListAsync();
            ViewBag.httt = httt;
            var HinhThucThanhToan = await _context.HinhThucThanhToans.FindAsync(id);
            if (HinhThucThanhToan == null)
            {
                return NotFound();
            }
            ViewData["NH"] = new SelectList(_context.NganHangs.Where(w => w.Active == 1), "Idnh", "TenNganHang", HinhThucThanhToan.Idnh);
            return View(HinhThucThanhToan);
        }

        // POST: HinhThucThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idhttt,Idnh,MaHttt,TenHttt,Ntao,Nsua,NgayTao,NgaySua")] HinhThucThanhToan HinhThucThanhToan)
        {
            if (id != HinhThucThanhToan.Idhttt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(HinhThucThanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HinhThucThanhToanExists(HinhThucThanhToan.Idhttt))
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
            ViewData["NH"] = new SelectList(_context.NganHangs.Where(w => w.Active == 1), "Idnh", "TenNganHang", HinhThucThanhToan.Idnh);
            TempData["NH"] = new SelectList(_context.NganHangs.Where(w => w.Active == 1), "Idnh", "TenNganHang", HinhThucThanhToan.Idnh);
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).OrderByDescending(x => x.Idhttt).ToListAsync();
            ViewBag.httt = httt;
            return View(HinhThucThanhToan);
        }

        // GET: HinhThucThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HinhThucThanhToans == null)
            {
                return NotFound();
            }

            var HinhThucThanhToan = await _context.HinhThucThanhToans
                .Include(h => h.IdnhNavigation)
                .FirstOrDefaultAsync(m => m.Idhttt == id);
            if (HinhThucThanhToan == null)
            {
                return NotFound();
            }

            return View(HinhThucThanhToan);
        }

        // POST: HinhThucThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HinhThucThanhToans == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.HinhThucThanhToans'  is null.");
            }
            var HinhThucThanhToan = await _context.HinhThucThanhToans.FindAsync(id);
            if (HinhThucThanhToan != null)
            {
                _context.HinhThucThanhToans.Remove(HinhThucThanhToan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HinhThucThanhToanExists(int id)
        {
          return (_context.HinhThucThanhToans?.Any(e => e.Idhttt == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.HinhThucThanhToans.Include(x=>x.IdnhNavigation).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("HinhThucThanhToan");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:G1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH HÌNH THỨC THANH TOÁN NHA KHOA RĂNG HÀM MẶT " + "(Ngày" + now.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã hình thức thanh toán";
                worksheet.Cell("B2").Value = "Tên hình thức thanh toán";
                worksheet.Cell("C2").Value = "Ngân hàng";
                worksheet.Cell("D2").Value = "Tên tài khoản";
                worksheet.Cell("E2").Value = "Số tài khoản";
                worksheet.Cell("F2").Value = "Ngày tạo";
                worksheet.Cell("G2").Value = "Trạng thái";
                worksheet.Range("A2:G2").Style.Font.Bold = true;
                worksheet.Range("A2:G2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < all.Count; i++)
                {
                    var bh = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)
                    string ngayTaoString = bh.IdnhNavigation.NgayTao.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 1).Value = bh.MaHttt;
                    worksheet.Cell(currentRow, 2).Value = bh.TenHttt;
                    worksheet.Cell(currentRow, 3).Value = bh.IdnhNavigation.TenNganHang;
                    worksheet.Cell(currentRow, 4).Value = bh.IdnhNavigation.SoTk;
                    worksheet.Cell(currentRow, 5).Value = bh.IdnhNavigation.TenTk;
                    worksheet.Cell(currentRow, 6).Value = ngayTaoString;
                    if (bh.IdnhNavigation.Active == 1)
                    {
                        worksheet.Cell(currentRow, 7).Value = "Còn hoạt động";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 7).Value = "Ngừng hoạt động";
                    }
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHinhThucThanhToan.xlsx");
                }
            }
        }

    }
}
