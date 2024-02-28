using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using QLRHM.Models;

namespace QLRHM.Controllers
{
    [Authorize]
    public class NhomCongViecsController : Controller
    {
        private readonly DatnqlrhmContext _context;
        private readonly MasterNcvCvDbContext _DbContext;

        public NhomCongViecsController(DatnqlrhmContext context, MasterNcvCvDbContext DbContext)
        {
            _context = context;
            _DbContext = DbContext;
        }

        // GET: NhomCongViecs
        public async Task<IActionResult> Index( int page = 1)
        {
            var nvc = await _DbContext.NhomCongViec.OrderByDescending(x => x.Idncv).Where(x=>x.Active==1).Take(100).ToListAsync();
           
            var nvchide = await _DbContext.NhomCongViec.OrderByDescending(x => x.Idncv).Where(x=>x.Active==0).Take(100).ToListAsync();
          
            int NoOfRecordPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nvc.Count) /Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            nvc = nvc.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            ViewBag.ncv = nvc;
            ViewBag.nvchide = nvchide;
            ViewBag.GeneratedMaNcv = GenerateRandomNumber();
            return View(nvc);
        }

        private string GenerateRandomNumber()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "NCV" + number.ToString().PadLeft(9, '0');
        }
        private string GenerateRandomNumber2()
        {
            Random random = new Random();
            int number = random.Next(1, 999999999); // Tạo số ngẫu nhiên từ 1 đến 999999999
            return "CV" + number.ToString().PadLeft(9, '0');
        }

       


        // GET: NhomCongViecs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhomCongViecs == null)
            {
                return NotFound();
            }

            var nhomCongViec = await _context.NhomCongViecs
                .FirstOrDefaultAsync(m => m.Idncv == id);
            if (nhomCongViec == null)
            {
                return NotFound();
            }

            return View(nhomCongViec);
        }

        // GET: NhomCongViecs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhomCongViecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idncv,MaNcv,TenNcv,Ntao,Nsua,NgayTao,NgaySua,Active")] NhomCongViec nhomCongViec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhomCongViec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhomCongViec);
        }

        // GET: NhomCongViecs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhomCongViecs == null)
            {
                return NotFound();
            }
            var nvc = await _DbContext.NhomCongViec.OrderByDescending(x => x.Idncv).Where(x => x.Active == 1).Take(100).ToListAsync();

            var nvchide = await _DbContext.NhomCongViec.OrderByDescending(x => x.Idncv).Where(x => x.Active == 0).Take(100).ToListAsync();
            var nhomCongViec = await _context.NhomCongViecs.FindAsync(id);
            if (nhomCongViec == null)
            {
                return NotFound();
            }
            ViewBag.ncv = nvc;
            ViewBag.nvchide = nvchide;
            return View(nhomCongViec);
        }

        // POST: NhomCongViecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idncv,MaNcv,TenNcv,Ntao,Nsua,NgayTao,NgaySua,Active")] NhomCongViec nhomCongViec)
        {
            if (id != nhomCongViec.Idncv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhomCongViec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhomCongViecExists(nhomCongViec.Idncv))
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
            return View(nhomCongViec);
        }

        // GET: NhomCongViecs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhomCongViecs == null)
            {
                return NotFound();
            }

            var nhomCongViec = await _context.NhomCongViecs
                .FirstOrDefaultAsync(m => m.Idncv == id);
            if (nhomCongViec == null)
            {
                return NotFound();
            }

            return View(nhomCongViec);
        }

        // POST: NhomCongViecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhomCongViecs == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.NhomCongViecs'  is null.");
            }
            var nhomCongViec = await _context.NhomCongViecs.FindAsync(id);
            if (nhomCongViec != null)
            {
                _context.NhomCongViecs.Remove(nhomCongViec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhomCongViecExists(int id)
        {
            return (_context.NhomCongViecs?.Any(e => e.Idncv == id)).GetValueOrDefault();
        }


        public async Task <IActionResult> CreateMaster()
        {
            NhomCongViec ncv = new NhomCongViec();

            ncv.MasterCV.Add(new CongViec());
            ViewBag.MasterCVList = ncv.MasterCV; // Đặt danh sách vào ViewBag

            var baoHanhs = await _context.BaoHanhs.ToListAsync();
            ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");

            string randomNumber = GenerateRandomNumber();
            while (_context.NhomCongViecs.Any(x => x.MaNcv == randomNumber))
            {
                randomNumber = GenerateRandomNumber(); // Tạo mã mới nếu trùng
            }
            string randomNumber2 = GenerateRandomNumber2();
            while (_context.CongViecs.Any(x => x.MaCongViec == randomNumber2))
            {
                randomNumber2 = GenerateRandomNumber2(); // Tạo mã mới nếu trùng
            }

            // Lưu giá trị randomNumber vào TempData để có thể sử dụng ở action POST
            TempData["GeneratedMaNcv"] = randomNumber;
            TempData["GeneratedMacv"] = randomNumber2;

            return View(ncv);
        }

        [HttpPost]
        public async  Task<IActionResult> CreateMaster(NhomCongViec NhomCongViec, CongViec congViec)

        {
            // Truyền giá trị từ TempData vào biến trong action POST
            if (TempData.TryGetValue("GeneratedMaNcv", out var randomNumberObj) && randomNumberObj is string randomNumber)
            {
                NhomCongViec.MaNcv = randomNumber; // Đặt mã vào đối tượng nhomCongViec
            }
            else if (TempData.TryGetValue("GeneratedMacv", out var randomNumberObj1) && randomNumberObj1 is string randomNumber2)
            {
                congViec.MaCongViec = randomNumber2; // Đặt mã vào đối tượng nhomCongViec
            }


            NhomCongViec.Active = 1;

            await _DbContext.AddAsync(NhomCongViec);
            await _DbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> EditMaster(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            NhomCongViec nhomCongViec = _DbContext.NhomCongViec.Include(e => e.MasterCV).FirstOrDefault(a => a.Idncv == id);

            if (nhomCongViec == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var baoHanhs = await _context.BaoHanhs.ToListAsync();
            ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");
            return View(nhomCongViec);
        }

        [HttpPost]
        public async Task<IActionResult> EditMaster(int id, NhomCongViec nhomCongViec)
        {
            if (id != nhomCongViec.Idncv)
            {
                return BadRequest(); // Xử lý trường hợp id không khớp
            }

            var NewNCV = await _DbContext.NhomCongViec.Include(e => e.MasterCV).FirstOrDefaultAsync(a => a.Idncv == nhomCongViec.Idncv);

            if (NewNCV == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }

            // Thực hiện kiểm tra hợp lệ cho dữ liệu và thêm lỗi vào ModelState nếu cần
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, quay lại view và hiển thị lỗi
                var baoHanhs = await _context.BaoHanhs.ToListAsync();
                ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");
                return View(nhomCongViec);
            }

            // Cập nhật thông tin của ứng viên
            NewNCV.MaNcv = nhomCongViec.MaNcv;
            NewNCV.TenNcv = nhomCongViec.TenNcv;
            NewNCV.NgaySua = nhomCongViec.NgaySua;

            NewNCV.MasterCV.Clear();

            foreach (var congViec in  nhomCongViec.MasterCV)
            {
                if (!string.IsNullOrEmpty(congViec.TenCongViec) &&
                    !string.IsNullOrEmpty(congViec.MoTa) &&
                    !string.IsNullOrEmpty(congViec.MaCongViec) 
                    //congViec.BaoHanh != null && congViec.BaoHanh.Idbh > 0 &&
                    //congViec.NgaySua != DateTime.MinValue &&
                    //congViec.NgayTao != DateTime.MinValue &&
                    //!double.IsNaN((double)congViec.DonGiaSuDung)
                    
                    )
                {
                    congViec.Active = 1;
                    NewNCV.MasterCV.Add(congViec);
                }
            }

            // Lưu thay đổi
            await _DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> HideMaster(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }
             
            var nhomCongViec = await _DbContext.NhomCongViec.Include(e => e.MasterCV).FirstOrDefaultAsync(a => a.Idncv == id);

            if (nhomCongViec == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nhomCongViec.Active = 0;
            var baoHanhs = await _context.BaoHanhs.ToListAsync();
            ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");
           await _DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        } public async Task<IActionResult> ActiveMaster(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Xử lý trường hợp id là null
            }

            var nhomCongViec = await _DbContext.NhomCongViec.Include(e => e.MasterCV).FirstOrDefaultAsync(a => a.Idncv == id);

            if (nhomCongViec == null)
            {
                return NotFound(); // Xử lý trường hợp không tìm thấy ứng viên
            }
            nhomCongViec.Active = 1;
            var baoHanhs = await _context.BaoHanhs.ToListAsync();
            ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");
          await  _DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailsMaster(int id)
        {
            var nhomCongViec = await _DbContext.NhomCongViec
                .Include(e => e.MasterCV)
                .Where(a => a.Idncv == id).FirstOrDefaultAsync();

            var baoHanhs = await _context.BaoHanhs.ToListAsync();
            ViewData["idbh"] = new SelectList(baoHanhs, "Idbh", "SoNgay");
            return View(nhomCongViec);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMaster(int id)
        {
            var applicant =  await _DbContext.NhomCongViec
                .Include(e => e.MasterCV)
                .Where(a => a.Idncv == id).FirstOrDefaultAsync();

            return View(applicant);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMaster(NhomCongViec nhomCongViec)
        {

             _DbContext.Attach(nhomCongViec);
             _DbContext.Entry(nhomCongViec).State = EntityState.Deleted;
            await _DbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> ExportExcel()
        {
            var all = await _context.CongViecs.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("NhomCongViecDieuTri");
                var now = DateTime.Now;

                // Gộp ô cho tiêu đề
                var titleRange = worksheet.Range("A1:D1");
                titleRange.Merge();
                titleRange.FirstCell().Value = "DANH SÁCH NHÓM CÔNG VIỆC ĐIỀU TRỊ NHA KHOA RĂNG HÀM MẶT " + "(Ngày" + now.ToString("dd/MM/yyyy") + ")";
                titleRange.FirstCell().Style.Font.Bold = true;

                titleRange.FirstCell().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.FirstCell().Style.Font.FontSize = 16; // Đặt kích thước font

                // Đặt border cho titleRange
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                // Thiết lập tiêu đề cho từng cột
                worksheet.Cell("A2").Value = "Mã nhóm công việc điều trị";
                worksheet.Cell("B2").Value = "Tên nhóm công việc điều trị";
                worksheet.Cell("C2").Value = "Ngày tạo";
                worksheet.Cell("D2").Value = "Trạng thái";
                worksheet.Range("A2:D2").Style.Font.Bold = true;
                worksheet.Range("A2:D2").Style.Font.FontSize = 13;
                // Fill dữ liệu bác sĩ khác
                for (var i = 0; i < all.Count; i++)
                {
                    var bh = all[i];
                    var currentRow = i + 3; // Bắt đầu từ hàng 3 (do hàng 1 và 2 đã có tiêu đề)
                    //string ngayTaoString = bh.NgayTao.Value.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 1).Value = bh.MaCongViec;
                    worksheet.Cell(currentRow, 2).Value = bh.TenCongViec;
                    worksheet.Cell(currentRow, 3).Value = bh.NgayTao;
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
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhomCVĐT.xlsx");
                }
            }
        }
    }
}

