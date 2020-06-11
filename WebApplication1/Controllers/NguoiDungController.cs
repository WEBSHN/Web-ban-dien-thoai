using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class NguoiDungController : Controller
    {
        DataQLBanDTDDDataContext db = new DataQLBanDTDDDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Email không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Điện thoại không được để trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi5"] = "Địa chỉ không được để trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.Matkhau == matkhau);
                    if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;                    
                    return RedirectToAction("Giohang","Giohang");
                } else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
            
        }
        
    }
}