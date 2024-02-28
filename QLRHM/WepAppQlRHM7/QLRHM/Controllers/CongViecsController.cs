using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class CongViecsController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public CongViecsController(DatnqlrhmContext context)
        {
            _context = context;
        }

        // GET: CongViecs
        public async Task<IActionResult> Index(CongViec congViec, int page = 1)
        {
            List<CongViec> cv,cvhide;
            cv = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 1 && h.NhomCongViec.Active==1).OrderByDescending(x => x.Idcvdt).ToListAsync();
             cvhide = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 0).OrderByDescending(x => x.Idcvdt).ToListAsync();
            ViewBag.cv = cv;
            ViewBag.cvhide = cvhide;

            int NoOfRecordPerPage = 100;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cv.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            cv = cv.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();


            ViewData["Idbh"] = new SelectList(_context.BaoHanhs.Where(x => x.Active == 1), "Idbh", "SoNgay");
            ViewData["Idncv"] = new SelectList(_context.NhomCongViecs.Where(x=>x.Active==1), "Idncv", "TenNcv");
            ViewData["Idncv2"] = new SelectList(_context.NhomCongViecs.Where(x => x.Active == 1), "TenNcv", "TenNcv");
            //TempData["Idncv"] = _context.NhomCongViecs.ToList();


            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.CongViecs.Any(x => x.MaCongViec == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var nhombac = new NhomCongViec(); // Tạo một đối tượng NhomCongViec mới
            congViec.MaCongViec = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.GeneratedCvdt = randomNumber;
            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "CVDT" + number.ToString().PadLeft(9, '0');
        }
        // GET: CongViecs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CongViecs == null)
            {
                return NotFound();
            }

            var congViec = await _context.CongViecs
                .Include(c => c.BaoHanh)
                .Include(c => c.NhomCongViec)
                .FirstOrDefaultAsync(m => m.Idcvdt == id);
            if (congViec == null)
            {
                return NotFound();
            }

            return View(congViec);
        }

        // GET: CongViecs/Create
        public IActionResult Create()
        {
            ViewData["Idbh"] = new SelectList(_context.BaoHanhs, "Idbh", "Idbh");
            ViewData["Idncv"] = new SelectList(_context.NhomCongViecs, "Idncv", "MaNcv");
            return View();
        }

        // POST: CongViecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idcvdt,Idncv,Idbh,MaCongViec,TenCongViec,MoTa,DonGiaSuDung,Ntao,Nsua,NgayTao,NgaySua,Active")] CongViec congViec)
        {
            if (ModelState.IsValid)
            {
                congViec.Active = 1;
                _context.Add(congViec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbh"] = new SelectList(_context.BaoHanhs, "Idbh", "Idbh", congViec.Idbh);
            ViewData["Idncv"] = new SelectList(_context.NhomCongViecs, "Idncv", "MaNcv", congViec.Idncv);
            return View(congViec);
        }

        // GET: CongViecs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CongViecs == null)
            {
                return NotFound();
            }

            var congViec = await _context.CongViecs.FindAsync(id);
            if (congViec == null)
            {
                return NotFound();
            }
            ViewData["Idbh"] = new SelectList(_context.BaoHanhs, "Idbh", "SoNgay", congViec.Idbh);
            ViewData["Idncv"] = new SelectList(_context.NhomCongViecs, "Idncv", "TenNcv", congViec.Idncv);
           
            ViewData["Idncv2"] = new SelectList(_context.NhomCongViecs.Where(x => x.Active == 1), "TenNcv", "TenNcv");
            List<CongViec> cv, cvhide;
            cv = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 1).OrderByDescending(x => x.Idcvdt).ToListAsync();
            cvhide = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 0).OrderByDescending(x => x.Idcvdt).ToListAsync();
            ViewBag.cv = cv;
            ViewBag.cvhide = cvhide;
            return View(congViec);
        }

        // POST: CongViecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idcvdt,Idncv,Idbh,MaCongViec,TenCongViec,MoTa,DonGiaSuDung,Ntao,Nsua,NgaySua,Active")] CongViec congViec)
        {
            if (id != congViec.Idcvdt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    congViec.NgayTao = _context.CongViecs.AsNoTracking().FirstOrDefault(bs => bs.Idcvdt == congViec.Idcvdt)?.NgayTao;
                    _context.Update(congViec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongViecExists(congViec.Idcvdt))
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
            ViewData["Idbh"] = new SelectList(_context.BaoHanhs, "Idbh", "Idbh", congViec.Idbh);
            ViewData["Idncv"] = new SelectList(_context.NhomCongViecs, "Idncv", "MaNcv", congViec.Idncv);
            List<CongViec> cv, cvhide;
            cv = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 1).OrderByDescending(x => x.Idcvdt).ToListAsync();
            cvhide = await _context.CongViecs.Include(c => c.BaoHanh).Include(c => c.NhomCongViec).Where(h => h.Active == 0).OrderByDescending(x => x.Idcvdt).ToListAsync();
            ViewBag.cv = cv;
            ViewBag.cvhide = cvhide;
            return View(congViec);
        }

        // GET: CongViecs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CongViecs == null)
            {
                return NotFound();
            }

            var congViec = await _context.CongViecs
                .Include(c => c.BaoHanh)
                .Include(c => c.NhomCongViec)
                .FirstOrDefaultAsync(m => m.Idcvdt == id);
            if (congViec == null)
            {
                return NotFound();
            }

            return View(congViec);
        }

        // POST: CongViecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CongViecs == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.CongViecs'  is null.");
            }
            var congViec = await _context.CongViecs.FindAsync(id);
            if (congViec != null)
            {
                _context.CongViecs.Remove(congViec);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CongViecExists(int id)
        {
          return (_context.CongViecs?.Any(e => e.Idcvdt == id)).GetValueOrDefault();
        }



        public async Task<IActionResult> Hide(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            var congViec = await _context.CongViecs.FirstOrDefaultAsync(a => a.Idcvdt == id);

            if (congViec == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            congViec.Active = 0;

            _context.SaveChanges();
           
            return RedirectToAction("Index");
        }
        public IActionResult Active(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            var congViec = _context.CongViecs.FirstOrDefault(a => a.Idcvdt == id);

            if (congViec == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            congViec.Active = 1;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.CongViecs.Include(x =>x.NhomCongViec).Include(x=>x.BaoHanh).OrderByDescending(X=>X.NhomCongViec.TenNcv).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("CongViecDieuTri");
                var now = DateTime.Now;
                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:H1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH CÔNG VIỆC ĐIỀU TRỊ NHA KHOA RĂNG HÀM MẶT " + "(Ngày" + now.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã công việc điều trị";
                worksheet.Cell("B2").Value = "Nhóm công việc điều trị";
                worksheet.Cell("C2").Value = "Tên công việc điều trị";             
                worksheet.Cell("D2").Value = "Mô tả";
                worksheet.Cell("E2").Value = "Đơn giá sử dụng";
                worksheet.Cell("F2").Value = "Bảo hành ";
                worksheet.Cell("G2").Value = "Ngày tạo";
                worksheet.Cell("H2").Value = "Trạng thái";
                worksheet.Range("A2:I2").Style.Font.Bold = true;
                worksheet.Range("A2:I2").Style.Font.FontSize = 13;
                
                for (var i = 0; i < all.Count; i++)
                {
                    var bh = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)
                    //string ngayTaoString = bh.NgayTao.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 1).Value = bh.MaCongViec;
                    worksheet.Cell(currentRow, 2).Value = bh.NhomCongViec.TenNcv;
                    worksheet.Cell(currentRow, 3).Value = bh.TenCongViec;
                    worksheet.Cell(currentRow, 4).Value = bh.MoTa;
                    worksheet.Cell(currentRow, 5).Value = bh.FormattedDonGiaSuDung;
                    worksheet.Cell(currentRow, 6).Value = bh.BaoHanh.SoNgay;
                    worksheet.Cell(currentRow, 7).Value = bh.NgayTao;
                    if (bh.Active == 1)
                    {
                        worksheet.Cell(currentRow, 8).Value = "Còn hoạt động";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 8).Value = "Ngừng hoạt động";
                    }
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachCongViecDieuTri.xlsx");
                }
            }
        }


    }
}
