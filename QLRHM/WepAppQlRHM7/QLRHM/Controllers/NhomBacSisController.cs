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

namespace QLRHM.Controllers
{
    [Authorize]
    public class NhomBacSisController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public NhomBacSisController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: NhomBacSis
        public async Task<IActionResult> Index(NhomBacSi nhomBacSi,int id,int page = 1)
        {
            var nbs = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 1).Take(100).ToListAsync();
       
            var nbshide = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 0).Take(100).ToListAsync();
            ViewBag.nbs = nbs;
            ViewBag.nbshide = nbshide;
            int NoOfRecordPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nbs.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            nbs = nbs.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.NhomBacSis.Any(x => x.MaNbs == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var nhombac = new NhomCongViec(); // Tạo một đối tượng NhomCongViec mới
            nhomBacSi.MaNbs = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.GeneratedMaNbs = randomNumber;
          
          
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "NBS" + number.ToString().PadLeft(9, '0');
        }
        private string GenerateRandomNumber2()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "NBS" + number.ToString().PadLeft(9, '0');
        }
        // GET: NhomBacSis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhomBacSis == null)
            {
                return NotFound();
            }

            var nhomBacSi = await _context.NhomBacSis
                .FirstOrDefaultAsync(m => m.Idnbs == id);
            if (nhomBacSi == null)
            {
                return NotFound();
            }

            return View(nhomBacSi);
        }

        // GET: NhomBacSis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhomBacSis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idnbs,MaNbs,TenNbs,Ntao,Nsua,NgayTao,NgaySua,Active")] NhomBacSi nhomBacSi)
        {
            if (ModelState.IsValid)
            {
                nhomBacSi.Active = 1;
                _context.Add(nhomBacSi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhomBacSi);
        }

        // GET: NhomBacSis/Edit/5
        public async Task<IActionResult> Edit(int? id , int page = 1)
        {
            if (id == null || _context.NhomBacSis == null)
            {
                return NotFound();
            }
            var nbs = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 1).Take(100).ToListAsync();
            var nbshide = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 0).Take(100).ToListAsync();
            var nhomBacSi = await _context.NhomBacSis.FindAsync(id);
            if (nhomBacSi == null)
            {
                return NotFound();
            }
            int NoOfRecordPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nbs.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            nbs = nbs.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            ViewBag.nbs = nbs;
            ViewBag.nbshide = nbshide;

            return View(nhomBacSi);
        }

        // POST: NhomBacSis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idnbs,MaNbs,TenNbs,Ntao,Nsua,NgayTao,NgaySua,Active")] NhomBacSi nhomBacSi)
        {
            if (id != nhomBacSi.Idnbs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhomBacSi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhomBacSiExists(nhomBacSi.Idnbs))
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
            var nbs = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 1).Take(100).ToListAsync();

            var nbshide = await _context.NhomBacSis.OrderByDescending(x => x.Idnbs).Where(a => a.Active == 0).Take(100).ToListAsync();
            ViewBag.nbs = nbs;
            ViewBag.nbshide = nbshide;
            return View(nhomBacSi);
        }

        // GET: NhomBacSis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhomBacSis == null)
            {
                return NotFound();
            }

            var nhomBacSi = await _context.NhomBacSis
                .FirstOrDefaultAsync(m => m.Idnbs == id);
            if (nhomBacSi == null)
            {
                return NotFound();
            }

            return View(nhomBacSi);
        }

        // POST: NhomBacSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhomBacSis == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.NhomBacSis'  is null.");
            }
            var nhomBacSi = await _context.NhomBacSis.FindAsync(id);
            if (nhomBacSi != null)
            {
                _context.NhomBacSis.Remove(nhomBacSi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhomBacSiExists(int id)
        {
          return (_context.NhomBacSis?.Any(e => e.Idnbs == id)).GetValueOrDefault();
        }

        public IActionResult Hide(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            NhomBacSi nhomBacSi = _context.NhomBacSis.FirstOrDefault(a => a.Idnbs == id);

            if (nhomBacSi == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nhomBacSi.Active = 0;
         
            _context.SaveChanges();
            TempData["AlertMessage"] = nhomBacSi.MaNbs + " ẩn thành công";
            return RedirectToAction("Index");
        } public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            NhomBacSi nhomBacSi = _context.NhomBacSis.FirstOrDefault(a => a.Idnbs == id);

            if (nhomBacSi == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nhomBacSi.Active = 1;
         
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.NhomBacSis.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NhomNhanVien");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:D1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH NHÓM NHÂN VIÊN NHA KHOA RĂNG HÀM MẶT " + "(Ngày" + now.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã nhóm nhân viên";
                worksheet.Cell("B2").Value = "Tên nhóm nhân viên";          
                worksheet.Cell("C2").Value = "Ngày tạo";
                worksheet.Cell("D2").Value = "Trạng thái";
                worksheet.Range("A2:D2").Style.Font.Bold = true;
                worksheet.Range("A2:D2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < all.Count; i++)
                {
                    var bh = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)
                    string ngayTaoString = bh.NgayTao.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 1).Value = bh.MaNbs;
                    worksheet.Cell(currentRow, 2).Value = bh.TenNbs;                  
                    worksheet.Cell(currentRow, 3).Value = ngayTaoString;
                    if (bh.Active == 1)
                    {
                        worksheet.Cell(currentRow, 4).Value = "Còn hoạt động";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 4).Value = "Ngừng hoạt động";
                    }
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhomNV.xlsx");
                }
            }
        }

    }
}
