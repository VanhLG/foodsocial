using G09.Models;
using G09.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace G09.Controllers
{
    public class AccessController : Controller
    {
        private readonly DbG09foodContext _context;
        //Tao biên để xử lý lưu người dùng vào Session
        private ssNguoiDung ssNguoiDung;
        public AccessController(DbG09foodContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ListBaiViet", "TrangChu");
            }
        }

        [HttpPost]
        public IActionResult LogIn(NguoiDung nguoiDung)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                var user = _context.NguoiDungs
                    .FirstOrDefault(x => x.Email.Equals(nguoiDung.Email) && x.MatKhau.Equals(nguoiDung.MatKhau));
                if (user != null)
                {
                    HttpContext.Session.SetString("Email", user.Email);

                    HttpContext.Session.SetInt32("id", user.MaNguoiDung);
                    HttpContext.Session.SetString("UrlAnhDD", user.AnhDaiDien);
                    HttpContext.Session.SetString("tenND", user.TenNguoiDung);

                    return RedirectToAction("ListBaiViet", "TrangChu");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(nguoiDung);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email hoặc tên người dùng đã tồn tại chưa
                var existingUser = _context.NguoiDungs
                    .FirstOrDefault(u => u.Email == model.Email || u.TenNguoiDung == model.TenNguoiDung);

                if (existingUser == null)
                {
                    var newUser = new NguoiDung
                    {
                        TenNguoiDung = model.TenNguoiDung,
                        Email = model.Email,
                        // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                        MatKhau = model.MatKhau,
                        NgayTao = DateTime.Now // Nếu bạn cần lưu ngày tạo
                    };

                    try
                    {
                        _context.NguoiDungs.Add(newUser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("LogIn", "Access");
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log lỗi hoặc hiển thị thông báo chi tiết
                        var innerException = ex.InnerException?.Message;
                        ModelState.AddModelError("", $"An error occurred while saving the user. Details: {innerException}");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                }
            }

            // Nếu có lỗi hoặc không hợp lệ, quay lại view với model
            return View(model);
        }


        //public IActionResult LogOut()
        //{
        //    HttpContext.Session.Clear();
        //    HttpContext.Session.Remove("Username");
        //    return RedirectToAction("LogIn", "Access");

        //}
    }
}
