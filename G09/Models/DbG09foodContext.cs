using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace G09.Models;

public partial class DbG09foodContext : DbContext
{
    public DbG09foodContext()
    {
    }

    public DbG09foodContext(DbContextOptions<DbG09foodContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<LoaiMonAn> LoaiMonAns { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<TheoDoi> TheoDois { get; set; }

    public virtual DbSet<Thich> Thiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("G09food"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__BaiViet__AEDD56473B060195");

            entity.ToTable("BaiViet");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaLoaiMonAnNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaLoaiMonAn)
                .HasConstraintName("FK__BaiViet__MaLoaiM__52593CB8");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__BaiViet__MaNguoi__5165187F");
        });

        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.MaBinhLuan).HasName("PK__BinhLuan__87CB66A0B5F49EF9");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaBaiVietNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaBaiViet)
                .HasConstraintName("FK__BinhLuan__MaBaiV__5629CD9C");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__BinhLuan__MaNguo__571DF1D5");
        });

        modelBuilder.Entity<LoaiMonAn>(entity =>
        {
            entity.HasKey(e => e.MaLoaiMonAn).HasName("PK__LoaiMonA__AF2559D340F86737");

            entity.ToTable("LoaiMonAn");

            entity.HasIndex(e => e.TenLoaiMonAn, "UQ__LoaiMonA__7AD8299C135BB907").IsUnique();

            entity.Property(e => e.TenLoaiMonAn).HasMaxLength(100);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762303B0343");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenNguoiDung, "UQ__NguoiDun__57E5A81DADD4AFC0").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__NguoiDun__A9D10534472D9BA7").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(256);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenNguoiDung).HasMaxLength(100);
            entity.Property(e => e.TieuSu).HasMaxLength(500);
        });

        modelBuilder.Entity<TheoDoi>(entity =>
        {
            entity.HasKey(e => e.MaTheoDoi).HasName("PK__TheoDoi__3156C079A2F63E05");

            entity.ToTable("TheoDoi");

            entity.HasIndex(e => new { e.MaNguoiTheoDoi, e.MaNguoiDuocTheoDoi }, "UQ__TheoDoi__1DBD570C78F5600C").IsUnique();

            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDuocTheoDoiNavigation).WithMany(p => p.TheoDoiMaNguoiDuocTheoDoiNavigations)
                .HasForeignKey(d => d.MaNguoiDuocTheoDoi)
                .HasConstraintName("FK__TheoDoi__MaNguoi__60A75C0F");

            entity.HasOne(d => d.MaNguoiTheoDoiNavigation).WithMany(p => p.TheoDoiMaNguoiTheoDoiNavigations)
                .HasForeignKey(d => d.MaNguoiTheoDoi)
                .HasConstraintName("FK__TheoDoi__MaNguoi__5FB337D6");
        });

        modelBuilder.Entity<Thich>(entity =>
        {
            entity.HasKey(e => e.MaThich).HasName("PK__Thich__985232E7041EB93A");

            entity.ToTable("Thich");

            entity.HasOne(d => d.MaBaiVietNavigation).WithMany(p => p.Thiches)
                .HasForeignKey(d => d.MaBaiViet)
                .HasConstraintName("FK__Thich__MaBaiViet__5AEE82B9");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Thiches)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__Thich__MaNguoiDu__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
