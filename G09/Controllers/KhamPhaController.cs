using G09.Models;
using Microsoft.AspNetCore.Mvc;

namespace G09.Controllers
{
    [Route("g09/")]
    public class KhamPhaController : Controller
    {
        private readonly DbG09foodContext _context;

        public KhamPhaController(DbG09foodContext context)
        {
            _context = context;
        }

        [Route("khampha")]
        public IActionResult Index(int? loaiMonAnId)
        {
            var loaiMonAns = _context.LoaiMonAns.ToList();

            if (!loaiMonAnId.HasValue && loaiMonAns.Any())
            {
                loaiMonAnId = loaiMonAns.First().MaLoaiMonAn;
            }

            var baiViets = _context.BaiViets
                .Where(b => !loaiMonAnId.HasValue || b.MaLoaiMonAn == loaiMonAnId.Value)
                .Select(b => new BaiViet
                {
                    MaBaiViet = b.MaBaiViet,
                    TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                    AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
                    NoiDung = b.NoiDung,
                    AnhBaiViet = b.AnhBaiViet,
                    NgayTao = b.NgayTao ?? DateTime.Now
                }).ToList();

            ViewBag.LoaiMonAns = loaiMonAns;
            ViewBag.SelectedLoaiMonAnId = loaiMonAnId;

            return View(baiViets);
        }
    }
}
