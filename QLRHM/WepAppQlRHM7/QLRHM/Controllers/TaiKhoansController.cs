using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using System.Security.Cryptography;
using QLRHM7.ViewModel;
using Microsoft.AspNetCore.Hosting;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.InkML;
using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;

namespace QLRHM7.Controllers
{
    [Authorize]
    public class TaiKhoansController : Controller
    {
        private readonly DatnqlrhmContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public TaiKhoansController(DatnqlrhmContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: TaiKhoans
        [Authorize(Roles = "Admin,QuanLy")]
        public async Task<IActionResult> Index()
        {
            var taiKhoans = await _context.TaiKhoans.Include(t => t.IdbsNavigation).Include(t => t.IdbsNavigation.IdnbsNavigation).Where(x=>x.Quyen != "Admin").ToListAsync();
            var ListNhanVien = await _context.BacSis.Include(x=>x.IdnbsNavigation)
                .Where(x=> x.Active == 1 &&  x.IdnbsNavigation != null && x.IdnbsNavigation.Active ==1 )
                .Where(nv => !_context.TaiKhoans.Any( tk => tk.Idbs == nv.Idbs))
                .OrderByDescending(x=>x.IdnbsNavigation.TenNbs).ToListAsync();

            ViewData["ListNhanVien"] = new SelectList(ListNhanVien, "Idbs", "Idbs")

                           .Select(x => new SelectListItem
                           {
                               Value = x.Value,
                               Text = $"{ _context.BacSis.Find(int.Parse(x.Value)).MaBacSi}-" + $"{_context.BacSis.Find(int.Parse(x.Value)).TenBs}-" + $"{_context.BacSis.Find(int.Parse(x.Value)).IdnbsNavigation.TenNbs}"
                           });
            ViewBag.taiKhoans = taiKhoans;
            return View();
        }

        // GET: TaiKhoans/Details/5
        public async Task<IActionResult> CaNhan(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return NotFound();
            }

            var CaNhan = await _context.BacSis
               
                .FirstOrDefaultAsync(m => m.Idbs == id);
            if (CaNhan == null)
            {
                return NotFound();
            }

            return View(CaNhan);
        }
        public async Task<IActionResult> EditCaNhan(int? id)
        {
            if (id == null || _context.BacSis == null)
            {
                return NotFound();
            }
           
            
            var bacSi = await _context.BacSis.FindAsync(id);
            if (bacSi == null)
            {
                return NotFound();
            }
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis, "Idnbs", "TenNbs", bacSi.Idnbs);
           /* return View(bacSi);*/ return PartialView("EditCaNhan", bacSi);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCaNhan(int id, [Bind("Idbs,Idnbs,MaBacSi,TenBs,AnhBs,NgaySinh,Cccd,GioiTinh,QueQuan,DiaChi,Sdt,Email,Zalo,Facebook,GhiChu,Ntao,Nsua,NgayTao,NgaySua,Active")] BacSi bacSi, IFormFile FrontImage)
        {
            if (id != bacSi.Idbs)
            {
                return NotFound();
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
                    TempData["CapNhat"] = "";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(bacSi.Idbs))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CaNhan", "TaiKhoans", new { id = id});

            }

            return RedirectToAction("CaNhan", "TaiKhoans", new { id = id });
        }


        public async Task<IActionResult> DoiMatKhau(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }


            var taikhoan = await _context.TaiKhoans.Include(x=>x.IdbsNavigation).Include(x=>x.IdbsNavigation.IdnbsNavigation).FirstOrDefaultAsync(x=>x.IdbsNavigation.Idbs == id);
            if (taikhoan == null)
            {
                return NotFound();
            }
            ViewData["Idnbs"] = new SelectList(_context.NhomBacSis, "Idnbs", "TenNbs", taikhoan.IdbsNavigation.Idnbs);
            ViewBag.id = id;
            /* return View(bacSi);*/
            return PartialView("DoiMatKhau", taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(int idtk,int id, [Bind("Idtk,Idbs,TenTaiKhoan,MatKhau,Quyen,Ntao,Nsua,NgayTao,NgaySua")] TaiKhoan taiKhoan) 
        {
            if (idtk != taiKhoan.Idtk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (taiKhoan.MatKhau != null)
                    {
                        SHA512Encryption sha = new SHA512Encryption();
                        taiKhoan.MatKhau = sha.Encrypt(taiKhoan.MatKhau);

                    }
                    taiKhoan.Quyen = _context.TaiKhoans.AsNoTracking().FirstOrDefault(bs => bs.Idtk == taiKhoan.Idtk)?.Quyen;
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();

                    var BacSiTK = await _context.BacSis
                        .FirstOrDefaultAsync(x => x.Idbs == id);
                    if (BacSiTK != null)
                    {
                        if (!string.IsNullOrEmpty(BacSiTK.Email))
                        {
                            string? userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.ValueType;
                            await SendEmailNotification2(
                                BacSiTK.Email,
                                "Cập nhật mật khẩu tài khoản",
                                 "Tài khoản của bạn đã cập nhật vào lúc " + DateTime.Now

                                + "\n Người cập nhật: " + userName
                            );
                        }
                    }
                   

                    TempData["CapNhat"] = "";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.IdbsNavigation.Idbs))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
                return RedirectToAction("CaNhan", "TaiKhoans", new { id = id });

            }

            return RedirectToAction("CaNhan", "TaiKhoans", new { id = id });
        }



        private async Task SendEmailNotification2(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                // Log hoặc xử lý lỗi ở đây nếu cần
                return;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Phòng khám nha khoa răng hàm mặt", "phongkhamnhakhoarhm159@gmail.com")); // Thay thế bằng địa chỉ email của bạn
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = body;

            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false); // Thay thế bằng thông tin SMTP của bạn
                await client.AuthenticateAsync("phongkhamnhakhoarhm159@gmail.com", "rjrq fdfx jmqm sncd"); // Thay thế bằng thông tin xác thực của bạn
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        // GET: TaiKhoans/Create
        public IActionResult Create()
        {
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs");
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idtk,Idbs,TenTaiKhoan,MatKhau,Quyen,Ntao,Nsua,NgayTao,NgaySua")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var tk =await _context.BacSis.FirstOrDefaultAsync(x=>x.Idbs == taiKhoan.Idbs);
                if( taiKhoan.MatKhau != null && tk!= null)
                {
                    SHA512Encryption sha = new SHA512Encryption();
                    taiKhoan.MatKhau = sha.Encrypt(taiKhoan.MatKhau);
                    taiKhoan.TenTaiKhoan = tk.Email;
                }    
                _context.Add(taiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", taiKhoan.Idbs);
            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TaiKhoans == null)
            {
                return NotFound();
            }

            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", taiKhoan.Idbs);
            return PartialView("Edit", taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idtk,Idbs,TenTaiKhoan,MatKhau,Quyen,Ntao,Nsua,NgayTao,NgaySua")] TaiKhoan taiKhoan)
        {
            if (id != taiKhoan.Idtk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (taiKhoan.MatKhau != null)
                    {
                        SHA512Encryption sha = new SHA512Encryption();
                        taiKhoan.MatKhau = sha.Encrypt(taiKhoan.MatKhau);

                    }
                    else
                    {
                        taiKhoan.MatKhau = _context.TaiKhoans.AsNoTracking().FirstOrDefault(bs => bs.Idtk == taiKhoan.Idtk)?.MatKhau;
                    }
                  
                    _context.Update(taiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaiKhoanExists(taiKhoan.Idtk))
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
            ViewData["Idbs"] = new SelectList(_context.BacSis, "Idbs", "Idbs", taiKhoan.Idbs);
            return View(taiKhoan);
        }
        public bool VerifyPassword(string plainText, string hashedPassword)
        {
            SHA512Encryption sha = new SHA512Encryption();
            return sha.Verify(plainText, hashedPassword);
        }
        // GET: TaiKhoans/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.TaiKhoans == null)
        //    {
        //        return NotFound();
        //    }

        //    var taiKhoan = await _context.TaiKhoans
        //        .Include(t => t.IdbsNavigation)
        //        .FirstOrDefaultAsync(m => m.Idtk == id);
        //    if (taiKhoan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(taiKhoan);
        //}

        //// POST: TaiKhoans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.TaiKhoans == null)
            {
                return Problem("Entity set 'DatnqlrhmContext.TaiKhoans'  is null.");
            }
            var taiKhoan = await _context.TaiKhoans.FindAsync(id);
            if (taiKhoan != null)
            {
                _context.TaiKhoans.Remove(taiKhoan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaiKhoanExists(int id)
        {
          return (_context.TaiKhoans?.Any(e => e.Idtk == id)).GetValueOrDefault();
        }
    }
}
