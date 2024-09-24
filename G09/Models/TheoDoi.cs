using System;
using System.Collections.Generic;

namespace G09.Models;

public partial class TheoDoi
{
    public int MaTheoDoi { get; set; }

    public int? MaNguoiTheoDoi { get; set; }

    public int? MaNguoiDuocTheoDoi { get; set; }

    public DateTime? NgayTao { get; set; }

    public virtual NguoiDung? MaNguoiDuocTheoDoiNavigation { get; set; }

    public virtual NguoiDung? MaNguoiTheoDoiNavigation { get; set; }
}
