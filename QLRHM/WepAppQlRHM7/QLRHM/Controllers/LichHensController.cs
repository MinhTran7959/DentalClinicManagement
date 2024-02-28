using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using MimeKit;
using MailKit.Net.Smtp;
using DocumentFormat.OpenXml.Office2010.Excel;
using QLRHM7.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QLRHM.Controllers
{
    [Authorize]
    public class LichHensController : Controller
    {
        private readonly DatnqlrhmContext _context;

        public LichHensController(DatnqlrhmContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,QuanLy,TiepTan,BacSi")]
        // GET: LichHens
        public async Task<IActionResult> ThongKeLichHen(string searchString, string fromTime, string toTime, string? fo, string? to,string? fo2, string? to2, int page = 1)
        {
            var data = await _context.LichHens
               
                .Include(x => x.IdndkhNavigation) .Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)               
                .Include(x => x.IdndkhNavigation.IdcvdtNavigation)
                .Include(x => x.IdndkhNavigation.IdbsdtNavigation)
                .OrderByDescending(x => x.NgayHen)
                .ThenByDescending(x => x.GioHen)
                .ToListAsync();
            var Data2 = data ;
            DateTime fromDate;
            DateTime toDate;

            if (fo == null && to == null)
            {
                fromDate = toDate = DateTime.Now.Date;
                toTime = "22:30";
                fromTime = "07:30";
            }
            else
            {
                fromDate = DateTime.ParseExact(fo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               
            }

            if (!string.IsNullOrEmpty(searchString) || !string.IsNullOrEmpty(fromTime) || !string.IsNullOrEmpty(toTime) || fo != null || to != null)
            {
                if (fromTime== null && toTime == null)
                {
                    toTime = "22:30";
                    fromTime = "07:30";
                }
                else if (toTime== null)
                {
                    toTime = "22:30";
                }
                else if (fromTime == null)
                {
                    fromTime = "07:30";
                }

                TimeSpan fromTimeValue = TimeSpan.ParseExact(fromTime, "hh\\:mm", CultureInfo.InvariantCulture);
                TimeSpan toTimeValue = TimeSpan.ParseExact(toTime, "hh\\:mm", CultureInfo.InvariantCulture);
               
                data = data.Where(x =>
                    (string.IsNullOrEmpty(searchString) || x.MaLichHen.Contains(searchString) ||
                     x.IdndkhNavigation.IdbsdtNavigation.TenBs.Contains(searchString) ||
                     x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.TenBn.Contains(searchString) ||
                     x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.MaBenhNhan.Contains(searchString) ||
                     x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Sdt.Contains(searchString) ||
                     x.IdndkhNavigation.IdcvdtNavigation.TenCongViec.Contains(searchString))
                    && (x.NgayHen >= fromDate && x.NgayHen <= toDate)
                    && (x.GioHen >= fromTimeValue && x.GioHen <= toTimeValue))
                    .ToList();
                 Data2 = Data2
                  
                 .Where(x => (x.GioHen >= fromTimeValue && x.GioHen <= toTimeValue) &&
                     x.NgayHen != null && (x.NgayHen >= fromDate && x.NgayHen <= toDate))
                 .ToList();



                ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
                ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
                ViewBag.fromTime = fromTimeValue.ToString("hh\\:mm");
                ViewBag.toTime = toTimeValue.ToString("hh\\:mm");
                ViewBag.searchString = searchString;
            }
            else
            {
                data = data.Where(x => x.NgayHen != null && (x.NgayHen >= fromDate && x.NgayHen <= toDate)).ToList();
                Data2 = Data2.Where(x => x.NgayHen != null && (x.NgayHen >= fromDate && x.NgayHen <= toDate)).ToList();

                ViewBag.searchString = searchString;
            }

            var tenBnList = Data2
                .GroupBy(x => x.IdndkhNavigation.IdbsdtNavigation.TenBs)
                .Select(group => $"{group.Key}")
                .ToList();
            //var tenBnList = await _context.BacSis.Where(x => x.Active == 1).ToListAsync();



            ViewBag.fromTimeValue = fromTime;
            ViewBag.toTimeValue = toTime;
            int NoOfRecordPerPage = 20;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Count) /
                                                          Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            var data2 = data.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            ViewBag.toDate = toDate.ToString("dd/MM/yyyy");
            ViewBag.fromDate = fromDate.ToString("dd/MM/yyyy");
            


            // Gán danh sách TenBn cho ViewBag
            ViewBag.Bs = tenBnList;
            return View(data2);
        }

        public async Task<IActionResult> Index()
        {
            var datnqlrhmContext = _context.LichHens.Include(l => l.IdndkhNavigation);
            return View(await datnqlrhmContext.ToListAsync());
        }
        [Authorize(Roles = "Admin,QuanLy,BacSi")]
        public async Task<IActionResult> KHDT(int id , string fo , string to )
        {
            var now = DateTime.Now.Date;

            var khdt = await _context.KeHoachDieuTris.Include(a => a.IdbnNavigation).Include(a => a.IdbsNavigation).Where(a => a.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idkhdt).ToListAsync();
            var benhNhan = await _context.BenhNhans.Where(x => x.Active == 1).FirstOrDefaultAsync(m => m.Idbn == id);
            var pttGroups1 = await _context.NoiDungThanhToans.Include(a => a.IdttNavigation).Include(a => a.IdttNavigation.IdbsNavigation).Include(a => a.IdndkhNavigation).Include(a => a.IdndkhNavigation.IdkhdtNavigation)
                .Include(a => a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation).Include(a => a.IdndkhNavigation.IdcvdtNavigation)
                .Where(a => (a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "1" || a.IdndkhNavigation.IdkhdtNavigation.DieuTri == "3") && a.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id)
                .ToListAsync();
            var pttGroups = pttGroups1.GroupBy(a => a.IdttNavigation.Idtt).ToList();
            var lh = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                .Where(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Idbn == id).OrderByDescending(x => x.Idlh).ToListAsync();
            var Ndkh = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Where(x => x.IdkhdtNavigation.IdbnNavigation.Idbn == id && x.IdkhdtNavigation.DieuTri == "1").ToListAsync();

            // Duyệt qua từng nhóm và lấy một dòng từ mỗi nhóm
            var ptt = pttGroups.Select(group => group.FirstOrDefault(item => item.ThanhToan != "1") ?? group.First(item => item.ThanhToan == "1"));
            ptt = ptt.OrderByDescending(item => item.IdttNavigation.Idtt).ToList();
            //xác nhận phiếu tt
            var Xntt = pttGroups1.Where(x => (x.IdndkhNavigation.IdkhdtNavigation.DieuTri != null)).ToList();
            ViewBag.khdt = khdt; ViewBag.bn = benhNhan; ViewBag.ptt = ptt; ViewBag.lh = lh; ViewBag.Ndkh = Ndkh; ViewBag.Xntt = Xntt;

            int TongKhDangDT = khdt.Count(a => a.DieuTri == "1"); int TongKDaXong = khdt.Count(a => a.DieuTri == "3");
            int pttGroupsDTT = pttGroups.Count(a => a.Any(item => item.ThanhToan == "1")); int pttGroupsCTT = pttGroups.Count(a => a.Any(item => item.ThanhToan != "1"));

            int TongLH = lh.Count(a => a.NgayHen.Date >= now.Date);

            ViewBag.TongKhDangDT = TongKhDangDT; ViewBag.TongKDaXong = TongKDaXong; ViewBag.TongPhiDaTT = pttGroupsDTT; ViewBag.TongPhiChuaTT = pttGroupsCTT; ; ViewBag.TongLH = TongLH;
            ViewBag.MaLichHen = MaLichHen();
           ViewBag.toDate = to ;  ViewBag.fromDate= fo;
            return View(khdt);
        }
        // GET: LichHens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LichHens == null)
            {
                return NotFound();
            }

            var lichHen = await _context.LichHens
                .Include(l => l.IdndkhNavigation)
                .FirstOrDefaultAsync(m => m.Idlh == id);
            if (lichHen == null)
            {
                return NotFound();
            }

            return View(lichHen);
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



        public async Task<IActionResult> DenHen(int? id , int id2)
        {
            if (id == null)
            {
                return RedirectToAction("Bug", "_404");
            }

            var lh = await _context.LichHens.FirstOrDefaultAsync(a => a.Idlh == id);

            if (lh == null)
            {
                return RedirectToAction("Bug", "_404");
            }
            lh.TrangThai = 1;
            lh.NgayDen = DateTime.Now;

            _context.SaveChanges();

            var BenhNhan = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
               .FirstOrDefaultAsync(x => x.Idlh == id);
            if (!string.IsNullOrEmpty(BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email))
            {
                string userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.ValueType;
                await SendEmailNotification(
                    BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email,
                    "Xác nhận đã đến hẹn",
                     "Quý khách đã đến vào lúc: " +DateTime.Now +" theo lịch hẹn"
                    + "\n----------------------------------------------------------"
                    + "\n Bạn có lịch hẹn bác sĩ: " + BenhNhan.IdndkhNavigation.IdbsdtNavigation.TenBs
                    + "\n Ngày: " + BenhNhan.NgayHen.ToString("dd/MM/yyyy")
                    + "\n Công việc điều trị: " + BenhNhan.IdndkhNavigation.IdcvdtNavigation.TenCongViec
                    + "\n----------------------------------------------------------" 
                    + "\n ~~ Cảm ơn quý khách đã đến theo lịch hẹn ~~"
                    + "\n Người xác nhận: " + userName
                );
            }
                return RedirectToAction("KHDT", "BenhNhans", new { id = id2, CheckEmail = "1" });
            //return Json(new { success = true });
        }

        // GET: LichHens/Create
        public IActionResult Create(int id2)
        {
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh");
            ViewBag.MaLichHen = MaLichHen();
            var BenhNhan =  _context.BenhNhans.Where(d => d.Active == 1).FirstOrDefault(x => x.Idbn == id2);
            ViewBag.BenhNhan = BenhNhan;
            return View();
        }

        // POST: LichHens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idlh,Idndkh,MaLichHen,LyDo,NgayHen,GioHen,Ntao,Nsua,NgayTao,NgaySua,Active")] LichHen lichHen, BenhNhan bn)
        {
            if (ModelState.IsValid)
            {
              
                _context.Add(lichHen);
               
                await _context.SaveChangesAsync();
                int createdRecordId = lichHen.Idlh;// lấy email vừa tạo
                // Gửi email thông báo
                
                var BenhNhan = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                 .FirstOrDefaultAsync(x => x.Idlh == createdRecordId);
                if (!string.IsNullOrEmpty(BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email))
                {
                    string userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.ValueType;
                    await SendEmailNotification(
                        BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email,
                        "Lịch hẹn khám bệnh",
                        "Bạn có lịch hẹn bác sĩ: " + BenhNhan.IdndkhNavigation.IdbsdtNavigation.TenBs
                        + "\n Ngày: " + BenhNhan.NgayHen.ToString("dd/MM/yyyy") 
                        + "\n Công việc điều trị: " + BenhNhan.IdndkhNavigation.IdcvdtNavigation.TenCongViec
                        + "\n Ghi chú: " + BenhNhan.LyDo
                         + "\n Người gửi: " + userName
                    );
                    return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn, CheckEmail = "1" });
                }
                else
                {
                    return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn, CheckEmail = "2" });
                }



               
            }
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", lichHen.Idndkh);
            return RedirectToAction("KHDT", "BenhNhans", new { id = bn.Idbn });
        }



        private async Task SendEmailNotification(string toEmail, string subject, string body)
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
    

    // GET: LichHens/Edit/5
    public async Task<IActionResult> Edit(int? id ,int id2)
        {
            if (id == null || _context.LichHens == null)
            {
                return NotFound();
            }

            var lichHen = await _context.LichHens.FindAsync(id);
            if (lichHen == null)
            {
                return NotFound();
            }
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", lichHen.Idndkh);
            var Ndkh = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Where(x => x.IdkhdtNavigation.IdbnNavigation.Idbn == id2 && x.IdkhdtNavigation.DieuTri == "1").ToListAsync();
            ViewBag.id2 = id2; ViewBag.Ndkh2 = Ndkh;
            return View(lichHen);
            //return PartialView("Edit", lichHen);
        }

        // POST: LichHens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id2, [Bind("Idlh,Idndkh,MaLichHen,LyDo,NgayHen,GioHen,Ntao,Nsua,NgayTao,NgaySua,Active")] LichHen lichHen, BenhNhan bn)
        {
            if (id != lichHen.Idlh)
            {
                return NotFound();
            }
            var Ndkh = await _context.NoiDungKeHoaches.Include(x => x.IdkhdtNavigation).Include(x => x.IdcvdtNavigation).Include(x => x.IdkhdtNavigation.IdbnNavigation).Where(x => x.IdkhdtNavigation.IdbnNavigation.Idbn == id2 && x.IdkhdtNavigation.DieuTri == "1").ToListAsync();
          
            if (ModelState.IsValid)
            {
                try
                {
                    lichHen.NgaySua= DateTime.Now;
                    _context.Update(lichHen);
                    await _context.SaveChangesAsync();

                    var BenhNhan = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
              .FirstOrDefaultAsync(x => x.Idlh == id);
                    if (BenhNhan != null && !string.IsNullOrEmpty(BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email)  )
                    {
                        string userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.ValueType;
                        await SendEmailNotification(
                            BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email,
                            "Cập nhật lịch hẹn khám bệnh",
                            "Bạn có lịch hẹn bác sĩ: " + BenhNhan.IdndkhNavigation.IdbsdtNavigation.TenBs
                            + "\n Ngày: " + BenhNhan.NgayHen.ToString("dd/MM/yyyy") 
                            + "\n Công việc điều trị: " + BenhNhan.IdndkhNavigation.IdcvdtNavigation.TenCongViec
                            + "\n Ghi chú: " + BenhNhan.LyDo
                             + "\n Người gửi: " + userName
                        );
                       
                    }
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichHenExists(lichHen.Idlh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

               
                return RedirectToAction("KHDT", "BenhNhans", new { id = id2,CheckEmail = "1" });

            }
            ViewData["Idndkh"] = new SelectList(_context.NoiDungKeHoaches, "Idndkh", "Idndkh", lichHen.Idndkh);
            return RedirectToAction("KHDT", "BenhNhans", new { id = id2 });
        }

        // GET: LichHens/Delete/5
       

        // POST: LichHens/Delete/5

        [HttpPost]
        public async Task<IActionResult> Delete(int id , int id2 )
        {
            if (id <= 0 || id2 <= 0)
            {
                return NotFound();
            }
            var lichHen = await _context.LichHens.FirstOrDefaultAsync(x=>x.Idlh == id);
            if (lichHen != null)
            {
                var BenhNhan = await _context.LichHens.Include(x => x.IdndkhNavigation).Include(x => x.IdndkhNavigation.IdcvdtNavigation).Include(x => x.IdndkhNavigation.IdbsdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation).Include(x => x.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation)
                 .FirstOrDefaultAsync(x => x.Idlh == id);
                if (BenhNhan != null && !string.IsNullOrEmpty(BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email))
                {
                    if(User != null)
                    {
                        string userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.ValueType;
                        await SendEmailNotification(
                            BenhNhan.IdndkhNavigation.IdkhdtNavigation.IdbnNavigation.Email,
                            "Huỷ lịch hẹn khám bệnh",

                                "Huỷ lịch hẹn với bác sĩ: " + BenhNhan.IdndkhNavigation.IdbsdtNavigation.TenBs
                                + "\n Ngày: " + BenhNhan.NgayHen.ToString("dd/MM/yyyy")
                                + "\n Công việc điều trị: " + BenhNhan.IdndkhNavigation.IdcvdtNavigation.TenCongViec
                                + "\n Ghi chú: " + BenhNhan.LyDo
                                + "\n Người gửi: " + userName
                        );
                    }
                }
                _context.LichHens.Remove(lichHen);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("KHDT", "BenhNhans", new { id = id2, CheckEmail = "1" });
        }

        private bool LichHenExists(int id)
        {
          return (_context.LichHens?.Any(e => e.Idlh == id)).GetValueOrDefault();
        }
    }
}
