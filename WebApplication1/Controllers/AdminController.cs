using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Models;
using System.IO;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        DataQLBanDTDDDataContext db = new DataQLBanDTDDDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SanPham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Hang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.HANGs.ToList().OrderBy(n => n.MaHang).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSP()
        {
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiHang()
        {
            return View();
        }
        public ActionResult SuaSP(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", sp.MaSP);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSP(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồ tại";

                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sp.Anhbia = fileName;
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiHang(HANG sp)
        {
            db.HANGs.InsertOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Hang");
        }
        public ActionResult ChitietSP(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 4004;
                return null;
            }
            return View(sp);
        }
        public ActionResult XoaSP(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("XoaSP")]
        public ActionResult Xacnhanxoa(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 4004;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        [HttpGet]
        public ActionResult SuaSP(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiểm tra đường dẫn file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";

                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sp.Anhbia = fileName;
                    UpdateModel(sp);
                    db.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
    }
}