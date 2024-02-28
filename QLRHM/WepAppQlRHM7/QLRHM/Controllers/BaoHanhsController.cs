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
    public class BaoHanhsController : Controller
    {
        private readonly MasterNcvCvDbContext _context;
        private readonly MasterNcvCvDbContext _DbContext;
        public BaoHanhsController(MasterNcvCvDbContext context, MasterNcvCvDbContext DbContext)
        {
            _context = context;
            _DbContext = DbContext;
        }

        // GET: BaoHanhs
        public async Task<IActionResult> Index( BaoHanh baoHanh, int page = 1)
        {
            var bh = await _DbContext.BaoHanh.OrderByDescending(x=>x.Idbh).Where(v =>v.Active == 1).ToListAsync();
            var bhhide = await _DbContext.BaoHanh.OrderByDescending(x=>x.Idbh).Where(v => v.Active == 0).ToListAsync();
            ViewBag.bh = bh;
            ViewBag.bhhide = bhhide;
            int NoOfRecordPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(bh.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            bh = bh.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.BaoHanh.Any(x => x.MaBaoHanh == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var nhombac = new NhomCongViec(); // Tạo một đối tượng NhomCongViec mới
            baoHanh.MaBaoHanh = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.GeneratedMabh = randomNumber;
            
              return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "BH" + number.ToString().PadLeft(9, '0');
        }
        private string GenerateRandomNumber2()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "BH" + number.ToString().PadLeft(9, '0');
        }
        // GET: BaoHanhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BaoHanh == null)
            {
                return NotFound();
            }

            var baoHanh = await _context.BaoHanh
                .FirstOrDefaultAsync(m => m.Idbh == id);
            if (baoHanh == null)
            {
                return NotFound();
            }

            return View(baoHanh);
        }

        // GET: BaoHanhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BaoHanhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idbh,MaBaoHanh,TenBaoHanh,SoNgay,Ntao,Nsua,NgayTao,NgaySua,Active")] BaoHanh baoHanh)
        {
            if (ModelState.IsValid)
            {
                baoHanh.Active = 1;
                _context.Add(baoHanh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baoHanh);
        }

        // GET: BaoHanhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BaoHanh == null)
            {
                return NotFound();
            }

            var baoHanh = await _context.BaoHanh.FindAsync(id);
            if (baoHanh == null)
            {
                return NotFound();
            }
            var bh = await _DbContext.BaoHanh.OrderByDescending(x => x.Idbh).Where(v => v.Active == 1).ToListAsync();
            var bhhide = await _DbContext.BaoHanh.OrderByDescending(x => x.Idbh).Where(v => v.Active == 0).ToListAsync();
            ViewBag.bh = bh;
            ViewBag.bhhide = bhhide;
            return View(baoHanh);
        }

        // POST: BaoHanhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idbh,MaBaoHanh,TenBaoHanh,SoNgay,Ntao,Nsua,NgayTao,NgaySua,Active")] BaoHanh baoHanh)
        {
            if (id != baoHanh.Idbh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(baoHanh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaoHanhExists(baoHanh.Idbh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var bh = await _DbContext.BaoHanh.OrderByDescending(x => x.Idbh).Where(v => v.Active == 1).ToListAsync();
                var bhhide = await _DbContext.BaoHanh.OrderByDescending(x => x.Idbh).Where(v => v.Active == 0).ToListAsync();
                ViewBag.bh = bh;
                ViewBag.bhhide = bhhide;
                return RedirectToAction(nameof(Index));
            }
            return View(baoHanh);
        }

        // GET: BaoHanhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BaoHanh == null)
            {
                return NotFound();
            }

            var baoHanh = await _context.BaoHanh
                .FirstOrDefaultAsync(m => m.Idbh == id);
            if (baoHanh == null)
            {
                return NotFound();
            }

            return View(baoHanh);
        }

        // POST: BaoHanhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BaoHanh == null)
            {
                return Problem("Entity set 'MasterNcvCvDbContext.BaoHanh'  is null.");
            }
            var baoHanh = await _context.BaoHanh.FindAsync(id);
            if (baoHanh != null)
            {
                _context.BaoHanh.Remove(baoHanh);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaoHanhExists(int id)
        {
          return (_context.BaoHanh?.Any(e => e.Idbh == id)).GetValueOrDefault();
        }


        public IActionResult Hide(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            BaoHanh baoHanh = _context.BaoHanh.FirstOrDefault(a => a.Idbh == id);

            if (baoHanh == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            baoHanh.Active = 0;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            BaoHanh baoHanh = _context.BaoHanh.FirstOrDefault(a => a.Idbh == id);

            if (baoHanh == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            baoHanh.Active = 1  ;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.BaoHanh.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BaoHanh");
                var now = DateTime.Now;
                
              
                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:E1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH BẢO HÀNH NHA KHOA RĂNG HÀM MẶT " + "(Ngày" + now.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã bảo hành";
                worksheet.Cell("B2").Value = "Tên bảo hành";
                worksheet.Cell("C2").Value = "Số ngày bảo hành";
                worksheet.Cell("D2").Value = "Ngày tạo";              
                worksheet.Cell("E2").Value = "Trạng thái";              
                worksheet.Range("A2:E2").Style.Font.Bold = true;
                worksheet.Range("A2:E2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < all.Count; i++)
                {
                    var bh = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)
                    string ngayTaoString = bh.NgayTao.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 1).Value = bh.MaBaoHanh;
                    worksheet.Cell(currentRow, 2).Value = bh.TenBaoHanh;
                    worksheet.Cell(currentRow, 3).Value = bh.SoNgay;
                    worksheet.Cell(currentRow, 4).Value = ngayTaoString;
                    if (bh.Active == 1)
                    {
                        worksheet.Cell(currentRow, 5).Value = "Còn hoạt động";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 5).Value = "Ngừng hoạt động";
                    }
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachBaoHanh.xlsx");
                }
            }
        }

    }
}
