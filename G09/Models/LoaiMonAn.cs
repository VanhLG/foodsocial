using System;
using System.Collections.Generic;

namespace G09.Models;

public partial class LoaiMonAn
{
    public int MaLoaiMonAn { get; set; }

    public string TenLoaiMonAn { get; set; } = null!;

    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
}
