


using Microsoft.EntityFrameworkCore;

namespace QLRHM.Models
{

    public class MasterNcvCvDbContext : DbContext
    {
        public MasterNcvCvDbContext(DbContextOptions<MasterNcvCvDbContext> options) : base(options)
        {

        }
        public virtual DbSet<BacSi> BacSi { get; set; }

        public virtual DbSet<BaoHanh> BaoHanh { get; set; }

        public virtual DbSet<BenhNhan> BenhNhan{ get; set; }

        public virtual DbSet<CongViec> CongViec { get; set; }



        public virtual DbSet<HinhThucThanhToan> HinhThucThanhToan { get; set; }

        public virtual DbSet<KeHoachDieuTri> KeHoachDieuTri { get; set; }

        public virtual DbSet<LichHen> LichHen { get; set; }

        public virtual DbSet<LyDoHen> LyDoHen { get; set; }

        public virtual DbSet<NganHang> NganHang { get; set; }

        public virtual DbSet<NhomBacSi> NhomBacSi { get; set; }

        public virtual DbSet<NhomCongViec> NhomCongViec { get; set; }

        public virtual DbSet<NoiDungKeHoach> NoiDungKeHoache { get; set; }

        public virtual DbSet<NoiDungThanhToan> NoiDungThanhToan { get; set; }

        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }

        public virtual DbSet<ThanhToan> ThanhToan { get; set; }
    }
}
