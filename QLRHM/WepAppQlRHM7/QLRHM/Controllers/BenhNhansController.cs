using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using QLRHM7.Interfaces;
using QLRHM7.DTOs;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class BenhNhansController : Controller
    {


        private readonly DatnqlrhmContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public BenhNhansController(DatnqlrhmContext context, IWebHostEnvironment webHost , IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
            _context = context;
            webHostEnvironment = webHost;
        }
        [Authorize(Roles = "Admin,QuanLy,TiepTan")]
        // GET: BenhNhans
        public async Task<IActionResult> Index(string searchString,int page = 1 )
        {
           
            var p = await _context.PhongKhams.Where(x=>x.Id!=1).ToListAsync();
            //var bn = await _context.BenhNhans.Where(x => x.Active == 1).Take(100).OrderByDescending(x => x.Idbn).ToListAsync();
            var _bn = await uow.benhNhanRepository.GetListBenhNhan();
            var _bnhide = await uow.benhNhanRepository.GetListBenhNhanAn();

            var bn = mapper.Map<IEnumerable<BenhNhanDto>>(_bn);
            var bnhide = mapper.Map<IEnumerable<BenhNhanDto>>(_bnhide);
            var TongBN = await _context.BenhNhans.CountAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchResult = await uow.benhNhanRepository.Search(searchString);
                bn = mapper.Map<IEnumerable<BenhNhanDto>>(searchResult);

            }
            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(bn.Count()) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.NoOfRecordsPerPage = NoOfRecordPerPage; // Thêm số lượng bản ghi trên mỗi trang vào ViewBag
            bn = bn.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            ViewBag.bn = bn;
            ViewBag.bnhide = bnhide;
            ViewData["Phong"] = new SelectList(p, "Id", "TenPhong")
             .Select(x => new SelectListItem
              {
                  Value = x.Value,
                 Text = $"{x.Text} ({SumBenhNhan(int.Parse(x.Value))})"
             });
            ViewBag.searchString = searchString;
            ViewBag.MaBenhNhan = MaBenhNhan();
            ViewBag.TongBenhNhan = TongBN;
            return View();
        }

        private int SumBenhNhan(int phongId)
        {
            return _context.BenhNhans.Count(bn => bn.Phong == phongId);
        }

        [Authorize(Roles = "Admin,QuanLy,BacSi")]
        public async Task<IActionResult> KHDT(string CheckEmail,int id)
        {
            if (CheckEmail == "1" && CheckEmail != null)
            {
                TempData["EmailSuccess"] = "";
            }
            else if (CheckEmail == "2")
            {
                TempData["EmailFails"] = "";
            }
            var now = DateTime.Now.Date;
            var tienSuBenhNhan = await _context.TienSuBenhNhans
                .Include(t => t.IdbnNavigation)
                .FirstOrDefaultAsync(x=>x.IdbnNavigation.Idbn == id);
            var khdt = await _context.KeHoachDieuTris.Include(a => a.IdbnNavigation).Include(a => a.IdbsNavigation).Where(a => a.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idkhdt).ToListAsync();
            var benhNhan = await _context.BenhNhans.Where(x=>x.Active==1).FirstOrDefaultAsync(m => m.Idbn == id);
            var pttGroups1 = await _context.NoiDungThanhToans.Include(a => a.IdttNavigation).Include(a => a.IdttNavigation.IdbsNavigation) .Include(a => a.IdndkhNavigation).Include(a => a.IdndkhNavigation.IdkhdtNavigation)
                .Include(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation) .Include(a => a.IdndkhNavigation.IdcvdtNavigation)
                .Where(a => (a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") && a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id)
                .ToListAsync();
            var pttGroups = pttGroups1.GroupBy(a => a.IdttNavigation.Idtt).ToList();
            var lh = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x=>x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idlh).ToListAsync();
            var Ndkh = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Where(x => x.IdkhdtNavigation.IdbnNavigation.Idbn==id && x.IdkhdtNavigation.DieuTri=="1").ToListAsync();

            var tt = await _context.ToaThuocChiTiets.Include(x=>x.IdthuocNavigation).Include(x=>x.IdtoaNavigation).Include(x=>x.IdtoaNavigation.IdbnNavigation).Include(x=>x.IdtoaNavigation.IdbsNavigation).Where(x=>x.IdtoaNavigation.IdbnNavigation.Idbn== id).GroupBy(x => x.IdtoaNavigation.Id)
                .Select(group => group.First()).ToListAsync();

            // Duyệt qua từng nhóm và lấy một dòng từ mỗi nhóm
            var ptt = pttGroups.Select(group => group.FirstOrDefault(item => item.ThanhToan != "1") ?? group.First(item => item.ThanhToan == "1"));
            ptt = ptt.OrderByDescending(item => item.IdttNavigation.Idtt).ToList();
            //xác nhận phiếu tt
            var Xntt = pttGroups1.Where(x=> (x.IdndkhNavigation.IdkhdtNavigation.DieuTri !=null )).ToList();
            ViewBag.khdt = khdt;ViewBag.bn = benhNhan;ViewBag.ptt = ptt;ViewBag.lh = lh;ViewBag.Ndkh = Ndkh;ViewBag.Xntt = Xntt;

            int TongKhDangDT = khdt.Count(a => a.DieuTri == "1");int TongKDaXong = khdt.Count(a => a.DieuTri == "3");
            int pttGroupsDTT = pttGroups.Count(a => a.Any(item => item.ThanhToan == "1"));int pttGroupsCTT = pttGroups.Count(a => a.Any(item => item.ThanhToan !="1"));

            int TongLH = lh.Count(a => a.NgayHen.Date >= now.Date);

            ViewBag.TongKhDangDT = TongKhDangDT;ViewBag.TongKDaXong = TongKDaXong;ViewBag.TongPhiDaTT = pttGroupsDTT;ViewBag.TongPhiChuaTT = pttGroupsCTT;ViewBag.TongLH = TongLH;;ViewBag.tienSuBenhNhan = tienSuBenhNhan;
            ViewBag.MaLichHen = MaLichHen();ViewBag.tt = tt;
            return View(khdt);
        }


        // GET: BenhNhans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BenhNhans == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var benhNhan = await _context.BenhNhans
                .FirstOrDefaultAsync(m => m.Idbn == id);
            if (benhNhan == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            //return View(benhNhan);
            return PartialView("Details", benhNhan);
        }

        // GET: BenhNhans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BenhNhans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idbn,MaBenhNhan,TenBn,NgaySinh,AnhBn,Cccd,GioiTinh,DiaChi,Sdt,Email,Zalo,FaceBook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active,Phong")] BenhNhan benhNhan, IFormFile FrontImage)
        {
            if (ModelState.IsValid)
            {
                if (FrontImage != null && FrontImage.Length > 0)
                {

                    string? uniqueFileName = null;
                    // Lưu hình ảnh vào thư mục wwwroot/images/NV
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "image", "BN");

                    uniqueFileName = Guid.NewGuid().ToString() + "-" + FrontImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        FrontImage.CopyTo(fileStream);
                    }
                    // Cập nhật tên hình ảnh vào đối tượng nhanvien
                    benhNhan.AnhBn = uniqueFileName;
                }

                else
                {
                    benhNhan.AnhBn = "profile.png";
                }

                _context.Add(benhNhan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(benhNhan);
        }

        // GET: BenhNhans/Edit/5
        public async Task<IActionResult> Edit2(int? id)
        {
            if (id == null || _context.BenhNhans == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            
           
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            
            return PartialView("Edit2",benhNhan);
        }

        // POST: BenhNhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, [Bind("Idbn,MaBenhNhan,TenBn,NgaySinh,AnhBn,Cccd,GioiTinh,DiaChi,Sdt,Email,Zalo,FaceBook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active,Phong")] BenhNhan benhNhan, IFormFile FrontImage)
        {
            if (id != benhNhan.Idbn)
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
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "image", "BN");

                        uniqueFileName = Guid.NewGuid().ToString() + "-" + FrontImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            FrontImage.CopyTo(fileStream);
                        }

                        benhNhan.AnhBn = uniqueFileName;
                    }
                    else
                    {
                        // FrontImage là null, giữ nguyên giá trị cũ của bacSi.Anhbs
                        benhNhan.AnhBn = _context.BenhNhans.AsNoTracking().FirstOrDefault(bs => bs.Idbn == benhNhan.Idbn)?.AnhBn;
                    }
                    benhNhan.NgaySua = _context.BenhNhans.AsNoTracking().FirstOrDefault(bs => bs.Idbn == benhNhan.Idbn)?.NgaySua;
                    _context.Update(benhNhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenhNhanExists(benhNhan.Idbn))
                    {
                        return RedirectToAction("Bug", "_404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("KHDT", "BenhNhans", new { id = benhNhan.Idbn });
            }
            return PartialView("Edit2", benhNhan);
        }
        // GET: BenhNhans/Edit/5
        public async Task<IActionResult> Edit(int? id, int page = 1)
        {
            if (id == null || _context.BenhNhans == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            var p = await _context.PhongKhams.Where(x => x.Id != 1).ToListAsync();
            var bn = await _context.BenhNhans.Where(x => x.Active == 1).Take(1000).OrderByDescending(x => x.Idbn).ToListAsync();
            var bnhide = await _context.BenhNhans.Where(x => x.Active == 0).Take(1000).OrderByDescending(x => x.Idbn).ToListAsync();

            int NoOfRecordPerPage = 10;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(bn.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            ViewBag.NoOfRecordsPerPage = NoOfRecordPerPage; // Thêm số lượng bản ghi trên mỗi trang vào ViewBag
            bn = bn.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
           
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            ViewData["Phong"] = new SelectList(p, "Id", "TenPhong")
                 .Select(x => new SelectListItem
                 {
                     Value = x.Value,
                     Text = $"{x.Text} ({SumBenhNhan(int.Parse(x.Value))})"
                 });
            
            ViewBag.bn = bn;
            ViewBag.bnhide = bnhide;
            return View(benhNhan);
        }
        
        // POST: BenhNhans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idbn,MaBenhNhan,TenBn,NgaySinh,AnhBn,Cccd,GioiTinh,DiaChi,Sdt,Email,Zalo,FaceBook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active,Phong")] BenhNhan benhNhan, IFormFile FrontImage)
        {
            if (id != benhNhan.Idbn)
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
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "image", "BN");

                        uniqueFileName = Guid.NewGuid().ToString() + "-" + FrontImage.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            FrontImage.CopyTo(fileStream);
                        }

                        benhNhan.AnhBn = uniqueFileName;
                    }
                    else
                    {
                        // FrontImage là null, giữ nguyên giá trị cũ của bacSi.Anhbs
                        benhNhan.AnhBn = _context.BenhNhans.AsNoTracking().FirstOrDefault(bs => bs.Idbn == benhNhan.Idbn)?.AnhBn;
                    }
                    _context.Update(benhNhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenhNhanExists(benhNhan.Idbn))
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
            return View(benhNhan);
        }

        // GET: BenhNhans/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.BenhNhans == null)
        //    {
        //        return RedirectToAction("Bug", "_404");
        //    }

        //    var benhNhan = await _context.BenhNhans
        //        .FirstOrDefaultAsync(m => m.Idbn == id);
        //    if (benhNhan == null)
        //    {
        //        return RedirectToAction("Bug", "_404");
        //    }

        //    return View(benhNhan);
        //}

        //// POST: BenhNhans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.BenhNhans == null)
        //    {
        //        return Problem("Entity set 'DatnqlrhmContext.BenhNhans'  is null.");
        //    }
        //    var benhNhan = await _context.BenhNhans.FindAsync(id);
        //    if (benhNhan != null)
        //    {
        //        _context.BenhNhans.Remove(benhNhan);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool BenhNhanExists(int id)
        {
            return (_context.BenhNhans?.Any(e => e.Idbn == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var bn = await _context.BenhNhans.FirstOrDefaultAsync(a => a.Idbn == id);

            if (bn == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            bn.Active = 0;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Active(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var bn = await _context.BenhNhans.FirstOrDefaultAsync(a => a.Idbn == id);

            if (bn == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            bn.Active = 1;
            bn.NgayTao = DateTime.Now.Date;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DieuTri(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var bn = await _context.KeHoachDieuTris.Include(x => x.IdbsNavigation).Include(x => x.IdbnNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);

            if (bn == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            bn.DieuTri = "1";


            _context.SaveChanges();

            return RedirectToAction("TaoPhieuThanhToan", "BenhNhans", new { id = bn.Idbn });
            //return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> HoanTatDieuTri(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var bn = await _context.KeHoachDieuTris.Include(x => x.IdbsNavigation).Include(x => x.IdbnNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);

            if (bn == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            bn.DieuTri = "3";


            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
            //return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> HuyDieuTri(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var bn = await _context.KeHoachDieuTris.Include(x => x.IdbsNavigation).Include(x => x.IdbnNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);

            if (bn == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            bn.DieuTri = "0";


            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
        }
        private string MaBenhNhan()
        {
            // Lấy ngày tháng năm hiện tại
            string ngayThangNam = DateTime.Now.ToString("ddMMyy");

            // Kiểm tra xem có phiếu cùng ngày không
            var Bn = _context.BenhNhans.OrderByDescending(x => x.MaBenhNhan).FirstOrDefault(p => p.MaBenhNhan.StartsWith($"BN{ngayThangNam}"));

            if (Bn != null)
            {
                // Tách lấy số phiếu hiện tại
                string MaBN = Bn.MaBenhNhan.Substring(14);
                int NewMaBn = int.Parse(MaBN) + 1;
                return $"BN{ngayThangNam}{NewMaBn:D8}";
            }
            else
            {
                // Nếu không có phiếu cùng ngày, bắt đầu từ 1
                return $"BN{ngayThangNam}00000001";
            }
        }
        private string MaKeHoach()
        {
            // Lấy ngày tháng năm hiện tại
            string ngayThangNam = DateTime.Now.ToString("ddMMyy");

            // Kiểm tra xem có phiếu cùng ngày không
            var Khdt = _context.KeHoachDieuTris.OrderByDescending(x => x.MaKeHoacDieuTri).FirstOrDefault(p => p.MaKeHoacDieuTri.StartsWith($"KHDT{ngayThangNam}"));

            if (Khdt != null)
            {
                // Tách lấy số phiếu hiện tại
                string Makhdt = Khdt.MaKeHoacDieuTri.Substring(14);
                int NewKhdt = int.Parse(Makhdt) + 1;
                return $"KHDT{ngayThangNam}{NewKhdt:D8}";
            }
            else
            {
                // Nếu không có phiếu cùng ngày, bắt đầu từ 1
                return $"KHDT{ngayThangNam}00000001";
            }
        }
        private string MaPhieuThanhToan()
        {
            // Lấy ngày tháng năm hiện tại
            string ngayThangNam = DateTime.Now.ToString("ddMMyy");

            // Kiểm tra xem có phiếu cùng ngày không
            var tt = _context.ThanhToans.OrderByDescending(x => x.MaThanhToan).FirstOrDefault(p => p.MaThanhToan.StartsWith($"PTT{ngayThangNam}"));

            if (tt != null)
            {
                // Tách lấy số phiếu hiện tại
                string Matt = tt.MaThanhToan.Substring(14);
                int NewMatt = int.Parse(Matt) + 1;
                return $"PTT{ngayThangNam}{NewMatt:D8}";
            }
            else
            {
                // Nếu không có phiếu cùng ngày, bắt đầu từ 1
                return $"PTT{ngayThangNam}00000001";
            }
        }

        private string MaLichHen()
        {
            // Lấy ngày tháng năm hiện tại
            string ngayThangNam = DateTime.Now.ToString("ddMMyy");

            // Kiểm tra xem có phiếu cùng ngày không
            var Bn = _context.LichHens.OrderByDescending(x => x.MaLichHen).FirstOrDefault(p => p.MaLichHen.StartsWith($"LH{ngayThangNam}"));

            if (Bn != null)
            {
                // Tách lấy số phiếu hiện tại
                string MaBN = Bn.MaLichHen.Substring(8);
                int NewMaBn = int.Parse(MaBN) + 1;
                return $"LH{ngayThangNam}{NewMaBn:D4}";
            }
            else
            {
                // Nếu không có phiếu cùng ngày, bắt đầu từ 1
                return $"LH{ngayThangNam}0001";
            }
        }



        public async Task<IActionResult> TaoKhdt(int id)
        {
            KeHoachDieuTri KHDT = new KeHoachDieuTri();

            KHDT.MasterNd.Add(new NoiDungKeHoach());
            ViewBag.MasterNdkhList = KHDT.MasterNd; // Đặt danh sách vào ViewBag

            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id);
            var NhomCongViec = await _context.NhomCongViecs.Where(d => d.Active == 1).ToListAsync();

            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi")
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.BacSis.Find(int.Parse(x.Value)).TenBs}"
                });

            ViewBag.BenhNhan = BenhNhan;
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}-" + $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });
            ViewData["NhomCongViec"] = new SelectList(NhomCongViec, "Idncv", "TenNcv")

               .Select(x => new SelectListItem
               {
                   Value = x.Value,
                   Text = $"{x.Text}"
               });


            ViewBag.MaKeHoach = MaKeHoach();
            return View(KHDT);
        }


        [HttpPost]
        public  IActionResult TaoKhdt(KeHoachDieuTri keHoachDieuTri, BenhNhan bn)
        {
            try
            {
                keHoachDieuTri.MasterNd.RemoveAll(x => x.IsDelete == true);
                _context.Add(keHoachDieuTri);
                _context.SaveChanges();

                return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
            }
            catch (Exception)
            {
                // Xử lý lỗi ở đây, ví dụ: ghi log, thông báo người dùng, v.v.
                
                return RedirectToAction("Bug", "_404");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SuaKhdt(int? id , int id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            KeHoachDieuTri kehoachdieutri = await _context.KeHoachDieuTris.Include(e => e.MasterNd).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);


            if (kehoachdieutri == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x=>x.Idbn== id2);
            var NhomCongViec = await _context.NhomCongViecs.Where(d => d.Active == 1).ToListAsync();
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", kehoachdieutri.Idbs)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });

            ViewBag.Idbn = BenhNhan;
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text =  $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });
            ViewData["NhomCongViec"] = new SelectList(NhomCongViec, "Idncv", "TenNcv")

                   .Select(x => new SelectListItem
                   {
                       Value = x.Value,
                       Text = $"{x.Text}"
                   });
            return View(kehoachdieutri);
        }
        [HttpPost]
        public async Task<IActionResult> SuaKhdt(int id, int id2 , KeHoachDieuTri keHoachDieuTri, BenhNhan bn)
        {
            if (id != keHoachDieuTri.Idkhdt)
            {
                return BadRequest(); // Xử lý trường hợp id không khớp
            }
            //var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id);
            //ViewBag.BenhNhan = BenhNhan;
            List<NoiDungKeHoach> ND = await _context.NoiDungKeHoaches.Where(x => x.Idkhdt == keHoachDieuTri.Idkhdt).ToListAsync();
            if (ND == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            if (!ModelState.IsValid)
            {
                // Dữ liệu không hợp lệ, quay lại view và hiển thị lỗi
                var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
                var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
                var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);

                ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", keHoachDieuTri.Idbs)
                  .Select(x => new SelectListItem
                  {
                      Value = x.Value,
                      Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                  });

                ViewBag.Idbn = BenhNhan;
                ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                                $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                                $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                    });
                return View(keHoachDieuTri);
            }


            _context.NoiDungKeHoaches.RemoveRange(ND);
            _context.SaveChanges();

            keHoachDieuTri.MasterNd.RemoveAll(x => x.IsDelete == true);
            _context.Update(keHoachDieuTri);
            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });

        }
        [HttpGet]
        public async Task<IActionResult> ChiTietKhdt(int? id , int id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            KeHoachDieuTri kehoachdieutri = await _context.KeHoachDieuTris.Include(e => e.MasterNd).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);


            if (kehoachdieutri == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);

            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", kehoachdieutri.Idbs)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });

            ViewBag.Idbn = BenhNhan;
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });
            return View(kehoachdieutri);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMaster(int? id , int id2)
        {


            KeHoachDieuTri kehoachdieutri = _context.KeHoachDieuTris.Include(e => e.MasterNd).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).Where(e => e.Idkhdt == id).FirstOrDefault();


            if (kehoachdieutri == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).Where(d => d.Active == 1 && d.NhomCongViec.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);

            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", kehoachdieutri.Idbs)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });

            ViewBag.Idbn = BenhNhan;
            ViewData["Idcvdt"] = new SelectList(CongViec, "Idcvdt", "MaCongViec")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.CongViecs.Find(int.Parse(x.Value)).NhomCongViec.TenNcv} - " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).TenCongViec}- " +
                            $"{_context.CongViecs.Find(int.Parse(x.Value)).FormattedDonGiaSuDung}"
                });
            return View(kehoachdieutri);
        }
        [HttpPost]

        public async Task<IActionResult> DeleteMaster(int id, KeHoachDieuTri keHoachDieuTri, BenhNhan bn)
        {
            var kehoachdieutriToDelete = await _context.KeHoachDieuTris
                .Include(e => e.MasterNd)
                .Include(e => e.IdbnNavigation)
                .Include(e => e.IdbsNavigation)
                .FirstOrDefaultAsync(a => a.Idkhdt == id);

            if (kehoachdieutriToDelete == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            _context.NoiDungKeHoaches.RemoveRange(kehoachdieutriToDelete.MasterNd);
            _context.KeHoachDieuTris.Remove(kehoachdieutriToDelete);
            _context.SaveChanges();

            return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
        }


        public async Task<IActionResult> HuyKHDT(int? id , int id2)
        {
            if (id == null || _context.KeHoachDieuTris == null)
            {
                return NotFound();
            }

            var keHoachDieuTri = await _context.KeHoachDieuTris.FindAsync(id);
            if (keHoachDieuTri == null)
            {
                return NotFound();
            }
          ViewBag.id2= id2;
            //return View(keHoachDieuTri);
            return PartialView("HuyKHDT", keHoachDieuTri);
        }

        // POST: KeHoachDieuTris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyKHDT(int id, int id2, [Bind("Idkhdt,Idbn,Idbs,MaKeHoacDieuTri,NgayLap,DieuTri,Nsua,NgaySua")] KeHoachDieuTri keHoachDieuTri)
        {
            if (id != keHoachDieuTri.Idkhdt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    keHoachDieuTri.NgaySua = DateTime.Now;
                    _context.Update(keHoachDieuTri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return RedirectToAction("Bug", "_404");
                }
                return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
            }

            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
        }

        public async Task<IActionResult> TaoPhieuThanhToan(int id)
        {
            ThanhToan tt = new ThanhToan();

            tt.MasterNdtt.Add(new NoiDungThanhToan());
            //ViewBag.MasterNdkhList = tt.MasterNd; // Đặt danh sách vào ViewBag

            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id
                && !(_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                          $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi")
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt")
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            ViewBag.BenhNhan = BenhNhan;
            ViewBag.MaPhieuThanhToan = MaPhieuThanhToan();
            return View(tt);
        }


        [HttpPost]
        public async Task<IActionResult> TaoPhieuThanhToan(int id, ThanhToan thanhToan, BenhNhan bn)
        {
            
            if (ModelState.IsValid)
            {
                thanhToan.MasterNdtt.RemoveAll(x => x.IsDelete == true);
                _context.Add(thanhToan);
                _context.SaveChanges();
                return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id
                && !(_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh")

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi")
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "TenHttt")
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            ViewBag.BenhNhan = BenhNhan;
            ViewBag.MaPhieuThanhToan = MaPhieuThanhToan();
            return View(thanhToan);
        }


        [HttpGet]
        public async Task<IActionResult> SuaPhieuThanhToan(int? id, int id2, NoiDungThanhToan noiDungThanhToan)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var thanhToan = await _context.ThanhToans.Include(e => e.MasterNdtt).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idtt == id);


            if (thanhToan == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id2
                && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            
            ViewBag.BenhNhan = BenhNhan;
            return View(thanhToan);
        }
        [HttpPost]
        public async Task<IActionResult> SuaPhieuThanhToan(int id, ThanhToan thanhToan, BenhNhan bn,int id2, NoiDungThanhToan noiDungThanhToan)
        {
            if (id != thanhToan.Idtt)
            {
                return BadRequest(); // Xử lý trường hợp id không khớp
            }

            List<NoiDungThanhToan> NDTT = await _context.NoiDungThanhToans.Include(x => x.IdndkhNavigation).Where(x => x.Idtt == thanhToan.Idtt).ToListAsync();

            if (NDTT == null || !NDTT.Any())
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy hoặc không có dữ liệu thỏa mãn điều kiện
            }
            if (!ModelState.IsValid)
            {
                var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
                var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                    .Where(d => d.IdkhdtNavigation.DieuTri == "1" &&
                    d.IdkhdtNavigation.IdbnNavigation.Idbn == id2
                    && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                    )
                    .ToListAsync();
                var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
                var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
                ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Value,
                        Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                                $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                    });
                ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", thanhToan.Idtt)
                  .Select(x => new SelectListItem
                  {
                      Value = x.Value,
                      Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                  });
                ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", thanhToan.Idtt)
                  .Select(x => new SelectListItem
                  {
                      Value = x.Value,
                      Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
                  });
                ViewBag.BenhNhan = BenhNhan;
            }
            _context.NoiDungThanhToans.RemoveRange(NDTT);
            _context.SaveChanges();
            thanhToan.MasterNdtt.RemoveAll(x => x.IsDelete == true);
            _context.Update(thanhToan);
            _context.SaveChanges();
            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
        }


        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToan(int id, int id2,NoiDungThanhToan noiDungThanhToan )
        {

            if (id != noiDungThanhToan.Idndtt)
            {
                return RedirectToAction("Bug", "_404");
            }

            // Lấy thông tin bệnh nhân từ cơ sở dữ liệu
            var ndtt = await _context.NoiDungThanhToans.FindAsync(id);
            var bn = await _context.BenhNhans.FindAsync(id2);

            if (ndtt == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            // Cập nhật phòng của bệnh nhân
            ndtt.ThanhToan ="1";
            //ndtt.NgaySua = DateTime.Now.Date;
            ndtt.ThanhToan = noiDungThanhToan.ThanhToan;
            ndtt.NgaySua = noiDungThanhToan.NgaySua;
            ndtt.NgayTao = noiDungThanhToan.NgayTao;

          
                _context.Update(ndtt);
                await _context.SaveChangesAsync();
         

            return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
          
        }


        [HttpGet]
        public async Task<IActionResult> PdfKeHoachDieuTri(int? id, int? id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            KeHoachDieuTri kehoachdieutri = await _context.KeHoachDieuTris.Include(e => e.MasterNd).Include(e => e.IdbnNavigation).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idkhdt == id);

            if (kehoachdieutri == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var BacSi = await _context.BacSis.ToListAsync();
            var CongViec = await _context.CongViecs.Include(x => x.NhomCongViec).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(x => x.Idbn == id2).FirstOrDefaultAsync();

            var pdfModel = new PdfKeHoach
            {
                KeHoachDieuTri = kehoachdieutri,
                BacSiList = BacSi,
                CongViecList = CongViec,
                BenhNhan = BenhNhan
            };
            //return View(pdfModel);
            return new ViewAsPdf("PdfKeHoachDieuTri", pdfModel)
            {
                FileName = $"{kehoachdieutri.MaKeHoacDieuTri}_{BenhNhan.MaBenhNhan}.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
        }
        [HttpGet]
        public async Task<IActionResult> PdfPhieuThanhToan(int? id, int? id2, NoiDungThanhToan noiDungThanhToan2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var noiDungThanhToan = await _context.NoiDungThanhToans.Include(e => e.IdhtttNavigation)
                .Include(e => e.IdttNavigation).Include(e => e.IdndkhNavigation).Include(e => e.IdndkhNavigation.IdcvdtNavigation)
                .Include(e => e.IdttNavigation.IdbsNavigation).Where(a => a.IdttNavigation.Idtt == id)                
                .ToListAsync();
            var noiDungThanhToan1 = await _context.NoiDungThanhToans.Include(e => e.IdhtttNavigation)
               .Include(e => e.IdttNavigation).Include(e => e.IdndkhNavigation).Include(e => e.IdndkhNavigation.IdcvdtNavigation)
               .Include(e => e.IdttNavigation.IdbsNavigation).FirstOrDefaultAsync(a => a.IdttNavigation.Idtt == id);
            var thanhtoan = await _context.ThanhToans.Include(x => x.MasterNdtt).Include(x => x.IdbsNavigation).FirstOrDefaultAsync(x => x.Idtt == id);

            if (noiDungThanhToan1 == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" || d.IdkhdtNavigation.DieuTri == "3" && d.IdkhdtNavigation.IdbnNavigation.Idbn == id2 && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))).ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan2.Idtt)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.FormattedDonGiaSuDung}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", noiDungThanhToan2.Idtt)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", noiDungThanhToan2.Idtt)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
                });
            ViewBag.BenhNhan = BenhNhan;

            var pdfModel = new PdfThanhToan
            {
                //thanhToan = thanhToan,
                bacSis = BacSi,
                noiDungKeHoaches = noidungkhdt,
               thanhToan=thanhtoan,
                noiDungThanhToan = noiDungThanhToan,
                noiDungThanhToan1 = noiDungThanhToan1,
                benhNhan = BenhNhan
            };

            //return View(pdfModel);
            return new ViewAsPdf("PdfPhieuThanhToan", pdfModel)
            {
                FileName = $"{thanhtoan.MaThanhToan}_{BenhNhan.MaBenhNhan}.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
        }
         public async Task<IActionResult> PdfXacNhanTT(int? id, int? id2,int? id3, NoiDungThanhToan noiDungThanhToan2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var noiDungThanhToan = await _context.NoiDungThanhToans.Include(e => e.IdhtttNavigation)
                .Include(e => e.IdttNavigation).Include(e => e.IdndkhNavigation).Include(e => e.IdndkhNavigation.IdcvdtNavigation)
                .Include(e => e.IdttNavigation.IdbsNavigation).Where(a => a.Idndtt == id)                
                .ToListAsync();
            var noiDungThanhToan1 = await _context.NoiDungThanhToans.Include(e => e.IdhtttNavigation)
               .Include(e => e.IdttNavigation).Include(e => e.IdndkhNavigation).Include(e => e.IdndkhNavigation.IdcvdtNavigation)
               .Include(e => e.IdttNavigation.IdbsNavigation).FirstOrDefaultAsync(a => a.Idndtt == id);
            var thanhtoan = await _context.ThanhToans.Include(x => x.MasterNdtt).Include(x => x.IdbsNavigation).FirstOrDefaultAsync(x => x.Idtt == id3);

            if (noiDungThanhToan1 == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1" || d.IdkhdtNavigation.DieuTri == "3" && d.IdkhdtNavigation.IdbnNavigation.Idbn == id2 && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))).ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan2.Idtt)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.FormattedDonGiaSuDung}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", noiDungThanhToan2.Idtt)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{x.Text} - {_context.BacSis.Find(int.Parse(x.Value)).TenBs} - {_context.BacSis.Find(int.Parse(x.Value)).NgaySinh.ToString("yyyy")}"
                });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", noiDungThanhToan2.Idtt)
                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
                });
            ViewBag.BenhNhan = BenhNhan;

            var pdfModel = new PdfThanhToan
            {
                //thanhToan = thanhToan,
                bacSis = BacSi,
                noiDungKeHoaches = noidungkhdt,
             
                noiDungThanhToan = noiDungThanhToan,
                noiDungThanhToan1 = noiDungThanhToan1,
                benhNhan = BenhNhan
            };

            //return View(pdfModel);
            return new ViewAsPdf("PdfXacNhanTT", pdfModel)
            {
                FileName = $"{thanhtoan.MaThanhToan}_{BenhNhan.MaBenhNhan}.pdf",
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4
            };
        }

        [HttpGet]
        public async Task<IActionResult> ChiTietPhieuThanhToan(int? id, int? id2, NoiDungThanhToan noiDungThanhToan)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp id là null
            }

            var thanhToan = await _context.ThanhToans.Include(e => e.MasterNdtt).Include(e => e.IdbsNavigation).FirstOrDefaultAsync(a => a.Idtt == id);


            if (thanhToan == null)
            {
                return RedirectToAction("Bug", "_404"); // Xử lý trường hợp không tìm thấy ứng viên
            }
            var BacSi = await _context.BacSis.Where(d => d.Active == 1).ToListAsync();
            var noidungkhdt = await _context.NoiDungKeHoaches.Include(x => x.IdcvdtNavigation).Include(x => x.IdcvdtNavigation.NhomCongViec).Include(x => x.IdkhdtNavigation)
                .Where(d => d.IdkhdtNavigation.DieuTri == "1"  || d.IdkhdtNavigation.DieuTri == "3"&&
                d.IdkhdtNavigation.IdbnNavigation.Idbn == id2
                && (_context.NoiDungThanhToans.Any(ntt => ntt.IdndkhNavigation.Idndkh == d.Idndkh))
                )
                .ToListAsync();
            var httt = await _context.HinhThucThanhToans.Include(x => x.IdnhNavigation).Where(d => d.IdnhNavigation.Active == 1).ToListAsync();
            var BenhNhan = await _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefaultAsync(x => x.Idbn == id2);
            ViewData["noidungkhdt"] = new SelectList(noidungkhdt, "Idndkh", "Idndkh", noiDungThanhToan.Idndkh)

                .Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).IdcvdtNavigation.TenCongViec} - " +
                            $"{_context.NoiDungKeHoaches.Find(int.Parse(x.Value)).FormattedDonGia}"
                });
            ViewData["Idbs"] = new SelectList(BacSi, "Idbs", "MaBacSi", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $"{_context.BacSis.Find(int.Parse(x.Value)).TenBs}"
              });
            ViewData["tt"] = new SelectList(httt, "Idhttt", "MaHttt", thanhToan.Idtt)
              .Select(x => new SelectListItem
              {
                  Value = x.Value,
                  Text = $" {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).TenHttt} - {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.TenNganHang}- {_context.HinhThucThanhToans.Find(int.Parse(x.Value)).IdnhNavigation.SoTk}"
              });
            ViewBag.BenhNhan = BenhNhan;
            //return View(thanhToan);
            return PartialView("ChiTietPhieuThanhToan", thanhToan);
            
           
        }


        //chọn theo nhóm
        [HttpGet]
        public JsonResult CvTheoNcv(int id)
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách các màu ứng với Idxe
            var cv = _context.CongViecs
                .Where(c => _context.NhomCongViecs.Any(x => x.Idncv == id && x.Idncv == c.Idncv && x.Active== 1 && c.Active==1)).ToList();          
            var TenCV = cv.Select(c => new { Idcvdt = c.Idcvdt, TenCv = c.TenCongViec ,Gia =c.FormattedDonGiaSuDung});

            return Json(TenCV);
        }
        public async Task<IActionResult> ChonPhong(int id, BenhNhan benhNhan)
        {
          
            if (id != benhNhan.Idbn)
            {
                return RedirectToAction("Bug", "_404");
            }

            // Lấy thông tin bệnh nhân từ cơ sở dữ liệu
            var existingBenhNhan = await _context.BenhNhans.FindAsync(id);

            if (existingBenhNhan == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            // Cập nhật phòng của bệnh nhân
            existingBenhNhan.Phong = benhNhan.Phong;
            existingBenhNhan.NgaySua = benhNhan.NgaySua;

            try
            {
                _context.Update(existingBenhNhan);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenhNhanExists(benhNhan.Idbn))
                {
                    return RedirectToAction("Bug", "_404");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "BenhNhans");
            //return View(existingBenhNhan);
        }
    }
}
