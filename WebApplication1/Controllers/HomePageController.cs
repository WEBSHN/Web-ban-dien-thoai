using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class HomePageController : Controller
    {

        DataQLBanDTDDDataContext db = new DataQLBanDTDDDataContext();

        private List<SANPHAM> Layspmoi(int count)
        {
            return db.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        private List<SANPHAM> LayTatcaSP()
        {
            return db.SANPHAMs.OrderByDescending(a => a.MaSP).ToList();
        }
        public ActionResult LayTatca(int? page)
        {
            int pageSize = 9;
            int pagenum = (page ?? 1);

            var sp = LayTatcaSP();
            return View(sp.ToPagedList(pagenum, pageSize));
        }
        public ActionResult DanhmucSP()
        {
            var hang = from dm in db.HANGs select dm;
            return PartialView(hang);
        }

        public ActionResult SPtheohang(int id, int? page)
        {
            int pageSize = 9;
            int pagenum = (page ?? 1);
            var sp = from s in db.SANPHAMs where s.MaHang == id select s;
            return View(sp.ToPagedList(pagenum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var sanpham = from sp in db.SANPHAMs
                          where sp.MaSP == id
                          select sp;
            return View(sanpham.Single());
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult BaoHanh()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
        // GET: TrangChu
        public ActionResult Index()
        {
            var sanphammoi = Layspmoi(9);
            return View(sanphammoi);
        }
    }
}