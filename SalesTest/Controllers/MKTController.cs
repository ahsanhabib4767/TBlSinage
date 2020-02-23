using SalesTest.ModelData;
using SalesTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using System.Data.Entity;

namespace SalesTest.Controllers
{
    [Authorize]
    public class MKTController : Controller
    {
        // GET: MKT
        private LIVEEntities databaseManager = new LIVEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SelectVendorAcknowledgement()
        {
            ViewBag.VenID = new SelectList(databaseManager.tblVendors, "VenID", "VendorName");
            return View();
        }
        [HttpGet]
        public ActionResult AddOrEditMarketing(int id = 0)
        {
            ViewBag.VenID = new SelectList(databaseManager.tblVendors, "VenID", "VendorName");
            if (id == 0)
                return View(new RequestMaster());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestMasters.Where(x => x.id == id).FirstOrDefault<RequestMaster>());
                }
            }
        }
        public ActionResult GetDataMKT()
        {
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.GetRequestDataForMKT().Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "A" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SaveDataForVendor(VendorConfirmM item, RequestMaster emp)
        {
            //bool status = false;
            //string mes = "";
            var w = (from y in databaseManager.sUsers
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.PlantNo }).FirstOrDefault();
            var wn = databaseManager.sPlants.Where(x => x.PlantNo == w.PlantNo).FirstOrDefault();
            string s1 = w.PlantNo.ToString();
            string s2 = string.Concat(s1 + "2000000");
            int reqno = Convert.ToInt32(s2);
            var maxreqno = (from n in databaseManager.VendorConfirms select n.VenReqID).DefaultIfEmpty(reqno).Max();
            var v = maxreqno + 1;

            try
            {

                VendorConfirm tb = new VendorConfirm();
                tb.VenReqID = v;
                tb.VenID = item.VenID;
                tb.ReqID = item.ReqID;
                tb.RegionName = item.RegionName;
                tb.AreaName = item.AreaName;
                tb.CeAreaName = item.CeAreaName;
                tb.DbName = item.DbName;
                tb.Address = item.Address;
                tb.cluster = item.cluster;
                tb.PSRName = item.PSRName;
                tb.SubRoute = item.SubRoute;
                tb.OutletCode = item.OutletCode;
                tb.OutLetName = item.OutLetName;
                tb.OwnerName = item.OwnerName;
                tb.ContactNum = item.ContactNum;
                tb.ContactNumTwo = item.ContactNumTwo;
                tb.ChannelName = item.ChannelName;
                tb.SE = item.SE;
                tb.signageheight = item.signageheight;
                tb.signagewidth = item.signagewidth;
                tb.remarks = item.remarks;
                tb.msg = item.msg;
                tb.dateLimit = item.dateLimit;
                tb.AppFlag1 = "I";
                tb.AppFlag2 = "O";
                tb.AppFlag3 = "S";
                tb.sinagetype = item.sinagetype;
                tb.PicUrl = item.PicUrl;
                tb.sinage = item.sinage;
                tb.otherbrand = item.otherbrand;
                tb.otherbrandd = item.otherbrandd;
                tb.OrderBy = User.Identity.Name;
                databaseManager.VendorConfirms.Add(tb);
                emp.AppFlag1 = "A";
                emp.AppFlag2 = "A";
                emp.AppFlag3 = "D";
                databaseManager.Entry(emp).State = EntityState.Modified;
                databaseManager.SaveChanges();
                //status = true;


                return RedirectToAction("SelectVendorAcknowledgement");
            }
            catch (Exception ex)
            {
                //string mess = ex.Message;
                //return Json(new { status = "error", message = "Error Generate" });
                return RedirectToAction("SelectVendorAcknowledgement");
            }


        }
        [HttpPost]
        public ActionResult AddOrEditRSM(RequestMaster emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.ASM = cd.LoginTypeID;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequest");
            }

        }
    }
}