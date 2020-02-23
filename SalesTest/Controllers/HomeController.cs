using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTest.Models;
using Newtonsoft.Json;

namespace SalesTest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public LIVEEntities db = new LIVEEntities();
        public ActionResult Index()
        {
            var un = (from y in db.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.UserName }).FirstOrDefault();
            ViewBag.userName = un.UserName;
            ViewData["ReqID"] = db.VendorConfirms.Count();
            ViewData["User"] = db.sUsers.Count();
            ViewData["Request"] = db.RequestMasters.Count();

            return View();
        }
        public ActionResult GetData()
        {


            var query = db.VendorConfirms.Include("installDate")
                   .GroupBy(p => p.installDate.ToString())
                   .Select(g => new { date = g.Key, count = g.Count()}).ToList();

            //var model = db.VendorConfirms
            //   .Include("installDate")
            //   .GroupBy(x => new {
            //       x = x.sinagetype,
            //       Name = x.sinage
            //   })
            //   .Select(x => new {
            //       installDate = x.Key.Name,
            //       Name = x.Key.Name,
            //       Count = x.Count()
            //   });
            //var query = db.VendorConfirms.Select(s => new { Text = s.sinage , Value = s.sinagetype}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDataPie()
        {
            int paint  = db.VendorConfirms.Where(x => x.otherbrandd == "Painting").Count();
            int sticker = db.VendorConfirms.Where(x => x.otherbrandd == "Sticker").Count();
            int food = db.VendorConfirms.Where(x => x.otherbrandd == "Food Combo").Count();
            int cs = db.VendorConfirms.Where(x => x.otherbrandd == "Cooler Sticker").Count();
            Ratio obj = new Ratio();
            obj.Paint = paint;
            obj.Sticker = sticker;
            obj.Food = food;
            obj.Cs = cs;

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int Paint { get; set; }
            public int Sticker { get; set; }
            public int Food { get; set; }
            public int Cs { get; set; }
        }
    }
}