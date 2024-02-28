using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class BacSisController : Controller
    {
        private readonly DatnqlrhmContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BacSisController(DatnqlrhmContext context ,IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnvironment = webHost;
        }

        // GET: BacSis
        public async Task<IActionResult> Index()
        {
            var bs = await _context.BacSis.Include(x=>x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 1 && v.IdnbsNavigation.Active ==1).ToListAsync();
            var bshide = await _context.BacSis.Include(x => x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 0).ToListAsync();
            ViewBag.bs = bs;
            ViewBag.bshide = bshide;
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis.Where(x=>x.Active==1), "Idnbs", "TenNbs");
            string randomNumber = GenerateRandomNumber();
            // Kiểm tra tính duy nhất của mã
            while (_context.BacSis.Any(x => x.MaBacSi == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            var bacsi = new BacSi(); // Tạo một đối tượng NhomCongViec mới
            bacsi.MaBacSi = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            ViewBag.GeneratedMaBs = randomNumber;

            return View();
        }
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "NV" + number.ToString().PadLeft(9, '0');
        }
        // GET: BacSis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return RedirectToAction("Bug", "_404");
                
            }

            var bacSi = await _context.BacSis
                .Include(b => b.IdnbsNavigation).Where(x=>x.IdnbsNavigation.Active==1)
                .FirstOrDefaultAsync(m => m.Idbs == id);
            if (bacSi == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            return View(bacSi);
        }

        // GET: BacSis/Create
        public IActionResult Create()
        {
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis, "Idnbs", "Idnbs");
            return View();
        }

        // POST: BacSis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idbs,Idnbs,MaBacSi,TenBs,AnhBs,NgaySinh,Cccd,GioiTinh,QueQuan,DiaChi,Sdt,Email,Zalo,Facebook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active")] BacSi bacSi, IFormFile FrontImage)
        {
            if (ModelState.IsValid)
            {              
                if (FrontImage != null && FrontImage.Length > 0)
                {

                    string? uniqueFileName = null;
                    // Lưu hình ảnh vào thư mục wwwroot/images/NV
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "image", "NV");

                    uniqueFileName = Guid.NewGuid().ToString() + "-" + FrontImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        FrontImage.CopyTo(fileStream);
                    }
                    // Cập nhật tên hình ảnh vào đối tượng nhanvien
                    bacSi.AnhBs = uniqueFileName;
                }
                   
                else {
                    bacSi.AnhBs = "profile.png";

                }
                bacSi.Active = 1;
                _context.Attach(bacSi);
                _context.Entry(bacSi).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis, "Idnbs", "Idnbs", bacSi.Idnbs);
            return View(bacSi);
        }

        // GET: BacSis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            var bs = await _context.BacSis.Include(x => x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 1).ToListAsync();
            var bshide = await _context.BacSis.Include(x => x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 0).ToListAsync();
            ViewBag.bs = bs;
            ViewBag.bshide = bshide;
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis.Where(x=>x.Active ==1), "Idnbs", "TenNbs", bacSi.Idnbs);
            return View(bacSi);
        }

        // POST: BacSis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idbs,Idnbs,MaBacSi,TenBs,AnhBs,NgaySinh,Cccd,GioiTinh,QueQuan,DiaChi,Sdt,Email,Zalo,Facebook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active")] BacSi bacSi , IFormFile FrontImage)
        {
            if (id != bacSi.Idbs)
            {
                return RedirectToAction("Bug", "_404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (FrontImage != null && FrontImage.Length > 0)
                    {

                        string? uniqueFileName = null;
                        // Lưu hình ảnh vào thư mục wwwroot/images/NV
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "image", "NV");

                        uniqueFileName = Guid.NewGuid().ToString() + "-" + FrontImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            FrontImage.CopyTo(fileStream);
                        }
                        // Cập nhật tên hình ảnh vào đối tượng nhanvien
                        bacSi.AnhBs = uniqueFileName;
                    }
                    else
                    {
                        // FrontImage là null, giữ nguyên giá trị cũ của bacSi.Anhbs
                        bacSi.AnhBs = _context.BacSis.AsNoTracking().FirstOrDefault(bs => bs.Idbs == bacSi.Idbs)?.AnhBs;
                    }
                    _context.Update(bacSi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BacSiExists(bacSi.Idbs))
                    {
                        return RedirectToAction("Bug", "_404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var bs = await _context.BacSis.Include(x => x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 1).ToListAsync();
            var bshide = await _context.BacSis.Include(x => x.IdnbsNavigation).OrderByDescending(x => x.Idbs).Where(v => v.Active == 0).ToListAsync();
            ViewBag.bs = bs;
            ViewBag.bshide = bshide;
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis, "Idnbs", "TenNbs", bacSi.Idnbs);
            return View(bacSi);
        }

        // GET: BacSis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var bacSi = await _context.BacSis
                .Include(b => b.IdnbsNavigation)
                .FirstOrDefaultAsync(m => m.Idbs == id);
            if (bacSi == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            return View(bacSi);
        }

        // POST: BacSis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BacSis == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.BacSis'  is null.");
            }
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi != null)
            {
                _context.BacSis.Remove(bacSi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BacSiExists(int id)
        {
          return (_context.BacSis?.Any(e => e.Idbs == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> Hide(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var bacSi = await _context.BacSis.FirstOrDefaultAsync(a => a.Idbs == id);

            if (bacSi == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            bacSi.Active = 0;

            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = bacSi.MaBacSi + " ẩn thành công";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Active(int? id) 
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var bacSi = await _context.BacSis.FirstOrDefaultAsync(a => a.Idbs == id);

            if (bacSi == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            bacSi.Active = 1;

            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = bacSi.MaBacSi + " ẩn thành công";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.BacSis.Include(x => x.IdnbsNavigation).ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BacSi");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:M1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH NHÂN VIÊN NHA KHOA RĂNG HÀM MẶT " +"(Ngày"+ now.ToString("dd/MM/yyyy")+")";
                titleRange.FirstCell().Style.Font.Bold = true;
               
                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã nhân viên";
                worksheet.Cell("B2").Value = "Thuộc nhóm";
                worksheet.Cell("C2").Value = "Tên tên nhân viên";
                worksheet.Cell("D2").Value = "Ngày sinh";
                worksheet.Cell("E2").Value = "CCCD";
                worksheet.Cell("F2").Value = "Giới tính";
                worksheet.Cell("G2").Value = "Địa chỉ";
                worksheet.Cell("H2").Value = "Sđt";
                worksheet.Cell("I2").Value = "Email";
                worksheet.Cell("J2").Value = "Zalo";
                worksheet.Cell("K2").Value = "FaceBook";
                worksheet.Cell("L2").Value = "Ghi Chú";
                worksheet.Cell("M2").Value = "Trạng Thái";
                worksheet.Range("A2:M2").Style.Font.Bold = true;
                worksheet.Range("A2:M2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < all.Count; i++)
                {
                    var bs = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)

                    worksheet.Cell(currentRow, 1).Value = bs.MaBacSi;
                    worksheet.Cell(currentRow, 2).Value = bs.IdnbsNavigation.TenNbs;
                    worksheet.Cell(currentRow, 3).Value = bs.TenBs;
                    worksheet.Cell(currentRow, 4).Value = bs.NgaySinh.ToString("dd/M/yyyy");
                    worksheet.Cell(currentRow, 5).Value = bs.Cccd;
                    worksheet.Cell(currentRow, 6).Value = bs.GioiTinh;
                    worksheet.Cell(currentRow, 7).Value = bs.DiaChi;
                    worksheet.Cell(currentRow, 8).Value = bs.Sdt;
                    worksheet.Cell(currentRow, 9).Value = bs.Email;
                    worksheet.Cell(currentRow, 10).Value = bs.Zalo;
                    worksheet.Cell(currentRow, 11).Value = bs.Facebook;
                    worksheet.Cell(currentRow, 12).Value = bs.GhiChu;
                    if (bs.Active == 1)
                    {
                        worksheet.Cell(currentRow, 13).Value ="Còn hoạt động";
                    }
                    else
                    {
                        worksheet.Cell(currentRow, 13).Value = "Ngừng hoạt động";
                    }    
                }

                using (var stream = new MemoryStream())
                {
                    worksheet.Columns().AdjustToContents();
                    worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhanVien.xlsx");
                }
            }
        }


    }
}
