using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QLRHM7.Models;

namespace QLRHM.Models;

public partial class DatnqlrhmContext : DbContext
{
    public DatnqlrhmContext()
    {
    }

    public DatnqlrhmContext(DbContextOptions<DatnqlrhmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BacSi> BacSis { get; set; }

    public virtual DbSet<BaoHanh> BaoHanhs { get; set; }

    public virtual DbSet<BenhNhan> BenhNhans { get; set; }

    public virtual DbSet<CongViec> CongViecs { get; set; }



    public virtual DbSet<HinhThucThanhToan> HinhThucThanhToans { get; set; }

    public virtual DbSet<KeHoachDieuTri> KeHoachDieuTris { get; set; }

    public virtual DbSet<LichHen> LichHens { get; set; }

    public virtual DbSet<LyDoHen> LyDoHens { get; set; }

    public virtual DbSet<NganHang> NganHangs { get; set; }

    public virtual DbSet<NhomBacSi> NhomBacSis { get; set; }

    public virtual DbSet<NhomCongViec> NhomCongViecs { get; set; }

    public virtual DbSet<NoiDungKeHoach> NoiDungKeHoaches { get; set; }

    public virtual DbSet<NoiDungThanhToan> NoiDungThanhToans { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
    public virtual DbSet<Thuoc> Thuocs { get; set; }

    public virtual DbSet<TienSuBenhNhan> TienSuBenhNhans { get; set; }

    public virtual DbSet<ToaThuocChiTiet> ToaThuocChiTiets { get; set; }
    public virtual DbSet<KeToa> KeToas { get; set; }
    public virtual DbSet<ThanhToan> ThanhToans { get; set; }
    public virtual DbSet<PhongKham> PhongKhams { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BacSi>(entity =>
        {
            entity.HasKey(e => e.Idbs);

            entity.ToTable("BacSi");

            entity.Property(e => e.Idbs).HasColumnName("IDBS");
            entity.Property(e => e.AnhBs)
                .HasMaxLength(250)
                .HasColumnName("AnhBS");
            entity.Property(e => e.Cccd)
                .HasMaxLength(50)
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Facebook).HasMaxLength(200);
            entity.Property(e => e.GhiChu).HasMaxLength(250);
            entity.Property(e => e.GioiTinh).HasMaxLength(20);
            entity.Property(e => e.Idnbs).HasColumnName("IDNBS");
            entity.Property(e => e.MaBacSi).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.QueQuan).HasMaxLength(250);
            entity.Property(e => e.Sdt).HasMaxLength(100);
            entity.Property(e => e.TenBs).HasMaxLength(200);
            entity.Property(e => e.Zalo).HasMaxLength(50);

            entity.HasOne(d => d.IdnbsNavigation).WithMany(p => p.BacSis)
                .HasForeignKey(d => d.Idnbs)
                .HasConstraintName("FK_BacSi_NhomBacSi");
        });

        modelBuilder.Entity<BaoHanh>(entity =>
        {
            entity.HasKey(e => e.Idbh);

            entity.ToTable("BaoHanh");

            entity.Property(e => e.Idbh).HasColumnName("IDBH");
            entity.Property(e => e.Active)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MaBaoHanh)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NTao");
            entity.Property(e => e.TenBaoHanh)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BenhNhan>(entity =>
        {
            entity.HasKey(e => e.Idbn);

            entity.ToTable("BenhNhan");

            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.AnhBn)
                .HasMaxLength(200)
                .HasColumnName("AnhBN");
            entity.Property(e => e.Cccd).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FaceBook).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.MaBenhNhan).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenBn)
                .HasMaxLength(50)
                .HasColumnName("TenBN");
            entity.Property(e => e.Zalo).HasMaxLength(50);

            entity.HasOne(d => d.PhongNavigation).WithMany(p => p.BenhNhans)
        .HasForeignKey(d => d.Phong)
        .HasConstraintName("FK_BenhNhan_PhongKham");
        });

        modelBuilder.Entity<CongViec>(entity =>
        {
            entity.HasKey(e => e.Idcvdt);

            entity.ToTable("CongViec");

            entity.Property(e => e.Idcvdt).HasColumnName("IDCVDT");
            entity.Property(e => e.Idbh).HasColumnName("IDBH");
            entity.Property(e => e.Idncv).HasColumnName("IDNCV");
            entity.Property(e => e.MaCongViec).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasMaxLength(100);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("NTao");
            entity.Property(e => e.TenCongViec).HasMaxLength(50);

            //entity.HasOne(d => d.IdbhNavigation).WithMany(p => p.CongViecs)
            //    .HasForeignKey(d => d.Idbh)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_CongViec_BaoHanh");

            entity.HasOne(d => d.NhomCongViec).WithMany(p => p.MasterCV)
                .HasForeignKey(d => d.Idncv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CongViec_NhomCongViec");
        });

        modelBuilder.Entity<HinhThucThanhToan>(entity =>
        {
            entity.HasKey(e => e.Idhttt);

            entity.ToTable("HinhThucThanhToan");

            entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");
            entity.Property(e => e.Idnh).HasColumnName("IDNH");
            entity.Property(e => e.MaHttt)
                .HasMaxLength(50)
                .HasColumnName("MaHTTT");
            entity.Property(e => e.NgaySua).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasMaxLength(50);
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.TenHttt)
                .HasMaxLength(50)
                .HasColumnName("TenHTTT");

            entity.HasOne(d => d.IdnhNavigation).WithMany(p => p.HinhThucThanhToans)
                .HasForeignKey(d => d.Idnh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HinhThucThanhToan_NganHang");
        });

        modelBuilder.Entity<KeHoachDieuTri>(entity =>
        {
            entity.HasKey(e => e.Idkhdt);

            entity.ToTable("KeHoachDieuTri");

            entity.Property(e => e.Idkhdt).HasColumnName("IDKHDT");
            entity.Property(e => e.DieuTri).HasMaxLength(10);
            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.Idbs).HasColumnName("IDBS");
            entity.Property(e => e.MaKeHoacDieuTri).HasMaxLength(50);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(150)
                .HasColumnName("NSua");

            entity.HasOne(d => d.IdbnNavigation).WithMany(p => p.KeHoachDieuTris)
                .HasForeignKey(d => d.Idbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KeHoachDieuTri_BenhNhan");

            entity.HasOne(d => d.IdbsNavigation).WithMany(p => p.KeHoachDieuTris)
                .HasForeignKey(d => d.Idbs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KeHoachDieuTri_BacSi");
        });

        modelBuilder.Entity<LichHen>(entity =>
        {
            entity.HasKey(e => e.Idlh);

            entity.ToTable("LichHen");

            entity.Property(e => e.Idlh).HasColumnName("IDLH");
            entity.Property(e => e.Idndkh).HasColumnName("IDNDKH");
            
            entity.Property(e => e.LyDo).HasMaxLength(100);
            entity.Property(e => e.MaLichHen).HasMaxLength(50);
            entity.Property(e => e.NgayHen).HasColumnType("date");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.NgayDen).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");

            entity.HasOne(d => d.IdndkhNavigation).WithMany(p => p.LichHens)
                .HasForeignKey(d => d.Idndkh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichHen_NoiDungKeHoach");
        });

        modelBuilder.Entity<LyDoHen>(entity =>
        {
            entity.ToTable("LyDoHen");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Lydo)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.MaLdh)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("MaLDH");
        });

        modelBuilder.Entity<NganHang>(entity =>
        {
            entity.HasKey(e => e.Idnh);

            entity.ToTable("NganHang");

            entity.Property(e => e.Idnh).HasColumnName("IDNH");
            entity.Property(e => e.MaNganHang).HasMaxLength(50);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.SoTk)
                .HasMaxLength(50)
                .HasColumnName("SoTK");
            entity.Property(e => e.TenNganHang).HasMaxLength(50);
            entity.Property(e => e.TenTk)
                .HasMaxLength(50)
                .HasColumnName("TenTK");
        });

        modelBuilder.Entity<NhomBacSi>(entity =>
        {
            entity.HasKey(e => e.Idnbs);

            entity.ToTable("NhomBacSi");

            entity.Property(e => e.Idnbs).HasColumnName("IDNBS");
            entity.Property(e => e.Active)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.MaNbs)
                .HasMaxLength(50)
                .HasColumnName("MaNBS");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.TenNbs)
                .HasMaxLength(50)
                .HasColumnName("TenNBS");
        });

        modelBuilder.Entity<NhomCongViec>(entity =>
        {
            entity.HasKey(e => e.Idncv);

            entity.ToTable("NhomCongViec");

            entity.Property(e => e.Idncv).HasColumnName("IDNCV");
            entity.Property(e => e.MaNcv)
                .HasMaxLength(50)
                .HasColumnName("MaNCV");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");
            entity.Property(e => e.TenNcv)
                .HasMaxLength(50)
                .HasColumnName("TenNCV");
        });

        modelBuilder.Entity<NoiDungKeHoach>(entity =>
        {
            entity.HasKey(e => e.Idndkh);

            entity.ToTable("NoiDungKeHoach");

            entity.Property(e => e.Idndkh).HasColumnName("IDNDKH");
            entity.Property(e => e.GhiChu).HasMaxLength(200);
            entity.Property(e => e.Idbsdt).HasColumnName("IDBSDT");
            entity.Property(e => e.Idcvdt).HasColumnName("IDCVDT");
            entity.Property(e => e.Idkhdt).HasColumnName("IDKHDT");
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");

            entity.HasOne(d => d.IdbsdtNavigation).WithMany(p => p.NoiDungKeHoaches)
                .HasForeignKey(d => d.Idbsdt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoiDungKeHoach_BacSi");

            entity.HasOne(d => d.IdcvdtNavigation).WithMany(p => p.NoiDungKeHoaches)
                .HasForeignKey(d => d.Idcvdt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoiDungKeHoach_CongViec");

            entity.HasOne(d => d.IdkhdtNavigation).WithMany(p => p.MasterNd)
                .HasForeignKey(d => d.Idkhdt)
                //.OnDelete(DeleteBehavior.ClientSetNull)
                 .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_NoiDungKeHoach_KeHoachDieuTri");
        });

        modelBuilder.Entity<NoiDungThanhToan>(entity =>
        {
            entity.HasKey(e => e.Idndtt);

            entity.ToTable("NoiDungThanhToan");

            entity.Property(e => e.Idndtt).HasColumnName("IDNDTT");
            entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");
            entity.Property(e => e.Idndkh).HasColumnName("IDNDKH");
            entity.Property(e => e.Idtt).HasColumnName("IDTT");
            entity.Property(e => e.ThanhToan).HasMaxLength(50);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");

            entity.HasOne(d => d.IdhtttNavigation).WithMany(p => p.NoiDungThanhToans)
                .HasForeignKey(d => d.Idhttt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoiDungThanhToan_HinhThucThanhToan");

            entity.HasOne(d => d.IdndkhNavigation).WithMany(p => p.NoiDungThanhToans)
                .HasForeignKey(d => d.Idndkh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoiDungThanhToan_NoiDungKeHoach");

            entity.HasOne(d => d.IdttNavigation).WithMany(p => p.MasterNdtt)
                .HasForeignKey(d => d.Idtt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NoiDungThanhToan_ThanhToan");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Idtk);

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.Idtk).HasColumnName("IDTK");
            entity.Property(e => e.Idbs).HasColumnName("IDBS");
            entity.Property(e => e.TenTaiKhoan).HasMaxLength(50);
            entity.Property(e => e.MatKhau).HasMaxLength(200);
            entity.Property(e => e.Quyen).HasMaxLength(50);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");
            entity.Property(e => e.Ntao)
                .HasMaxLength(50)
                .HasColumnName("NTao");

            entity.HasOne(d => d.IdbsNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.Idbs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_BacSi");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.Idtt);

            entity.ToTable("ThanhToan");

            entity.Property(e => e.Idtt).HasColumnName("IDTT");
            entity.Property(e => e.Idbs).HasColumnName("IDBS");
            entity.Property(e => e.MaThanhToan).HasMaxLength(50);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");
            entity.Property(e => e.Nsua)
                .HasMaxLength(50)
                .HasColumnName("NSua");

            entity.HasOne(d => d.IdbsNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.Idbs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThanhToan_BacSi1");
        });

        modelBuilder.Entity<Thuoc>(entity =>
        {
            entity.ToTable("Thuoc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonViTinh).HasMaxLength(50);
            entity.Property(e => e.MaThuoc).HasMaxLength(50);
            entity.Property(e => e.TenThuoc).HasMaxLength(100);
        });

        modelBuilder.Entity<TienSuBenhNhan>(entity =>
        {
            entity.ToTable("TienSuBenhNhan");

            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.Khac).HasMaxLength(500);
            entity.Property(e => e.DiUngThuoc).HasMaxLength(500);

            entity.HasOne(d => d.IdbnNavigation).WithMany(p => p.TienSuBenhNhans)
                .HasForeignKey(d => d.Idbn)
                .HasConstraintName("FK_TienSuBenhNhan_BenhNhan");
        });

        modelBuilder.Entity<ToaThuocChiTiet>(entity =>
        {
           
                entity.HasKey(e => e.Id);
            entity.ToTable("ToaThuocChiTiet");
                

            entity.Property(e => e.CachDung).HasMaxLength(500);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idthuoc).HasColumnName("IDThuoc");
            entity.Property(e => e.Idtoa).HasColumnName("IDToa");

            entity.HasOne(d => d.IdthuocNavigation).WithMany()
                .HasForeignKey(d => d.Idthuoc)
                .HasConstraintName("FK_ToaThuocChiTiet_Thuoc");

            entity.HasOne(d => d.IdtoaNavigation).WithMany(p => p.MasterTTCT)
                .HasForeignKey(d => d.Idtoa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ToaThuocChiTiet_KeToa");
        });
        modelBuilder.Entity<KeToa>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.ToTable("KeToa");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChuanDoan).HasMaxLength(500);
            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.Idbs).HasColumnName("IDBS");
            entity.Property(e => e.MaToaThuoc).HasMaxLength(50);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");

            entity.HasOne(d => d.IdbnNavigation).WithMany(p => p.KeToas)
                .HasForeignKey(d => d.Idbn)
                .HasConstraintName("FK_KeToa_BenhNhan");

            entity.HasOne(d => d.IdbsNavigation).WithMany(p => p.KeToas)
                .HasForeignKey(d => d.Idbs)
                .HasConstraintName("FK_KeToa_BacSi");
        });


        modelBuilder.Entity<PhongKham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LyDoHen");

            entity.ToTable("PhongKham");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idbn).HasColumnName("IDBn");
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.NgaySua).HasColumnType("datetime");
            entity.Property(e => e.NgayThem).HasColumnType("datetime");
            entity.Property(e => e.TenPhong).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
