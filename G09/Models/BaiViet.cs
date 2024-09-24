using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace G09.Models;

public partial class BaiViet
{
    public int MaBaiViet { get; set; }

    public int? MaNguoiDung { get; set; }
    [NotMapped]
    public string? TenNguoiDung { get; set; }
    [NotMapped]
    public string? AnhDaiDien {  get; set; }
    [NotMapped]
    public string? TenLoaiMonAn { get; set; }
    public string NoiDung { get; set; } = null!;

    public string? AnhBaiViet { get; set; }

    public int? MaLoaiMonAn { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual LoaiMonAn? MaLoaiMonAnNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }

    public virtual ICollection<Thich> Thiches { get; set; } = new List<Thich>();
    // New property to indicate if the current user liked the post
    [NotMapped]
    public bool IsLiked { get; set; }
    [Column("luotthich")]
    public int? SoLuongLike { get; set; }
}
