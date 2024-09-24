using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace G09.Models;

public partial class BinhLuan
{
    public int MaBinhLuan { get; set; }

    public int? MaBaiViet { get; set; }

    public int? MaNguoiDung { get; set; }
    [NotMapped]
    public string? TenNguoiDung { get; set; }
    public string NoiDung { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public virtual BaiViet? MaBaiVietNavigation { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
