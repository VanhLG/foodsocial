using G09.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G09.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    public class TrangChuController : Controller
    {
        private readonly ILogger<TrangChuController> _logger;
        private readonly DbG09foodContext _context;
        private NguoiDung us;
        public TrangChuController(DbG09foodContext context, ILogger<TrangChuController> logger)
        {
            _logger = logger;
            _context = context;


        }

        [HttpGet]
        public IActionResult ListBaiViet()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            us = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);

            var baiViets = _context.BaiViets
                .Select(b => new BaiViet
                {
                    MaBaiViet = b.MaBaiViet,
                    MaNguoiDung = b.MaNguoiDung,
                    TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                    AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
                    TenLoaiMonAn = b.MaLoaiMonAnNavigation.TenLoaiMonAn,
                    NoiDung = b.NoiDung,
                    AnhBaiViet = b.AnhBaiViet,
                    NgayTao = b.NgayTao ?? DateTime.Now,
                    Thiches = b.Thiches,
                    IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == us.MaNguoiDung),
                    SoLuongLike = b.SoLuongLike
                }).ToList();

            var cmts = _context.BinhLuans.Select(b => new BinhLuan
            {
                MaBinhLuan = b.MaBinhLuan,
                MaBaiViet = b.MaBaiViet,
                MaNguoiDung = b.MaNguoiDung,
                TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                NoiDung = b.NoiDung,
                NgayTao = b.NgayTao



            }).ToList();

            ViewBag.us = us; // Pass the current user ID to the view
            ViewBag.cmts = cmts;
            return View(baiViets);
        }

        //  [HttpPost]
        //[Route("LikeEvent")]
        [HttpPost()]
        public IActionResult LikeEvent(int mabaiviet, int tennguoidung)
        {
            if (mabaiviet == null || tennguoidung == null)
            {
                _logger.LogWarning("mabaiviet hoặc tennguoidung là null");
                return Content("mabaiviet hoặc tennguoidung là null");
            }

            // Hiển thị giá trị của các tham số
            _logger.LogInformation($"mabaiviet: {mabaiviet}, tennguoidung: {tennguoidung}");
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var uss = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);
            var existingLike = _context.Thiches
               .FirstOrDefault(t => t.MaBaiViet == mabaiviet && t.MaNguoiDung == uss.MaNguoiDung);
            var baiviet = _context.BaiViets
               .FirstOrDefault(t => t.MaBaiViet == mabaiviet);
            if (existingLike != null)
            {

                baiviet.SoLuongLike--;
                _context.Thiches.Remove(existingLike);
                _context.SaveChanges();
                 return Json(new { success = true   , newLikeCount = baiviet.SoLuongLike });
            }
            else
            {

                var thich = new Thich
                {
                    MaBaiViet = mabaiviet,
                    MaNguoiDung = uss.MaNguoiDung,
                };
                baiviet.SoLuongLike++;
                _context.Thiches.Add(thich);
                _context.SaveChanges();
                return Json(new { success = true, newLikeCount = baiviet.SoLuongLike });
            }

        }


        [HttpPost]
        public IActionResult AddComment(string comment, int mabaiviet, int tennguoidung)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            var uss = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);
            var cmt = new BinhLuan
            {
                MaBaiViet = mabaiviet,
                MaNguoiDung = uss.MaNguoiDung,
                NoiDung = comment
            };

            _context.BinhLuans.Add(cmt);
            _context.SaveChanges();


            return Json(new { success = true, tennguoidung = uss.TenNguoiDung });


        }
    }
}

       


