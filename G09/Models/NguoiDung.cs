using System;
using System.Collections.Generic;

namespace G09.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string TenNguoiDung { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? AnhDaiDien { get; set; }

    public string? TieuSu { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<TheoDoi> TheoDoiMaNguoiDuocTheoDoiNavigations { get; set; } = new List<TheoDoi>();

    public virtual ICollection<TheoDoi> TheoDoiMaNguoiTheoDoiNavigations { get; set; } = new List<TheoDoi>();

    public virtual ICollection<Thich> Thiches { get; set; } = new List<Thich>();
}
