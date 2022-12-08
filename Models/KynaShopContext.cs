using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KynaShop.Models
{
    public partial class KynaShopContext : DbContext
    {
        public KynaShopContext()
        {
        }

        public KynaShopContext(DbContextOptions<KynaShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Hinh> Hinhs { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<KhachHang> KhachHangs { get; set; } = null!;
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; } = null!;
        public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; } = null!;
        public virtual DbSet<NhanVienBanHang> NhanVienBanHangs { get; set; } = null!;
        public virtual DbSet<SanPham> SanPhams { get; set; } = null!;

        //Custom
        public virtual DbSet<SanPhamApi> SanPhamApis { get; set; } = null!;
        public virtual DbSet<KhachHangApi> KhachHangApis { get; set; } = null!;
        public virtual DbSet<CommentApi> CommentApis { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDon_GioHang> ChiTietHoaDon_GioHangs { get; set; } = null!;
        public virtual DbSet<Top10SanPham> Top10SanPhams { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("khongnomon");

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => e.Stt)
                    .HasName("PK__ChiTietH__CA1EB6908C556D26");

                entity.ToTable("ChiTietHoaDon");

                entity.Property(e => e.Stt).HasColumnName("STT");

                entity.HasOne(d => d.MaHoaDonNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHoaDon)
                    .HasConstraintName("FK__ChiTietHo__MaHoa__753864A1");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK__ChiTietHo__MaSan__74444068");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Stt)
                    .HasName("PK__Comment__CA3208781488E605");

                entity.ToTable("Comment");

                entity.Property(e => e.NoiDung).HasMaxLength(255);

                entity.HasOne(d => d.MaKhachHangNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.MaKhachHang)
                    .HasConstraintName("FK__Comment__MaKhach__7167D3BD");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK__Comment__MaSanPh__7073AF84");
            });

            modelBuilder.Entity<Hinh>(entity =>
            {
                entity.HasKey(e => e.Mahinh)
                    .HasName("PK__Hinh__0DCA86A17DEF0395");

                entity.ToTable("Hinh");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasColumnName("URL");

                entity.HasOne(d => d.MaSanPhamNavigation)
                    .WithMany(p => p.Hinhs)
                    .HasForeignKey(d => d.MaSanPham)
                    .HasConstraintName("FK__Hinh__MaSanPham__6D9742D9");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHoaDon)
                    .HasName("PK__HoaDon__835ED13B6DBBC9A7");

                entity.ToTable("HoaDon");

                entity.Property(e => e.NgayXuatHoaDon).HasColumnType("date");

                entity.HasOne(d => d.MaKhachHangNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaKhachHang)
                    .HasConstraintName("FK__HoaDon__MaKhachH__6319B466");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__HoaDon__MaNhanVi__6225902D");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKhachHang)
                    .HasName("PK__KhachHan__88D2F0E5CD4BC130");

                entity.ToTable("KhachHang");

                entity.HasIndex(e => e.SoDienThoai, "UQ__KhachHan__0389B7BD2C11E8AA")
                    .IsUnique();

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(60);

                entity.Property(e => e.Facebook).HasMaxLength(255);

                entity.Property(e => e.Google).HasMaxLength(255);

                entity.Property(e => e.SoDienThoai).HasMaxLength(12);

                entity.Property(e => e.TenKhachHang).HasMaxLength(40);
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.MaKhuyenMai)
                    .HasName("PK__KhuyenMa__6F56B3BD48F68DCA");

                entity.ToTable("KhuyenMai");

                entity.Property(e => e.NgayBatDau).HasColumnType("date");

                entity.Property(e => e.NgayKetThuc).HasColumnType("date");
            });

            modelBuilder.Entity<NhaSanXuat>(entity =>
            {
                entity.HasKey(e => e.MaNhaSanXuat)
                    .HasName("PK__NhaSanXu__838C17A1C3406947");

                entity.ToTable("NhaSanXuat");

                entity.Property(e => e.Hinh).HasMaxLength(255);

                entity.Property(e => e.TenNhaSanXuat).HasMaxLength(40);
            });

            modelBuilder.Entity<NhanVienBanHang>(entity =>
            {
                entity.HasKey(e => e.MaNhanVien)
                    .HasName("PK__NhanVien__77B2CA47E8125146");

                entity.ToTable("NhanVienBanHang");

                entity.HasIndex(e => e.Username, "UQ__NhanVien__536C85E471178855")
                    .IsUnique();

                entity.Property(e => e.Avatar).HasMaxLength(255);

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.Password).HasMaxLength(60);

                entity.Property(e => e.SoDienThoai).HasMaxLength(255);

                entity.Property(e => e.Username).HasMaxLength(40);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSanPham)
                    .HasName("PK__SanPham__FAC7442D152379F4");

                entity.ToTable("SanPham");

                entity.Property(e => e.Mota).HasMaxLength(255);

                entity.Property(e => e.TenSanPham).HasMaxLength(50);

                entity.HasOne(d => d.MaKhuyenMaiNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaKhuyenMai)
                    .HasConstraintName("FK__SanPham__MaKhuye__6ABAD62E");

                entity.HasOne(d => d.MaNhaSanXuatNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaNhaSanXuat)
                    .HasConstraintName("FK__SanPham__MaNhaSa__69C6B1F5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
