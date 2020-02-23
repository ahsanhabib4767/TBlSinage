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
using ImageResizer;

namespace SalesTest.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private LIVEEntities databaseManager = new LIVEEntities();
        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }

        //**Vendor step 1 Start**//
        //***12/10/2019|AhsanHabibRabby***//
        public ActionResult SelectVendorRequest()
        {

            return View();
        }
        public ActionResult GeGetVendorcnf()
        {
            var cd = User.Identity.Name;
            int id = Convert.ToInt32(cd);
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.GetVendorcnfDate(id).Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GeGetVendorcnf2()
        {
            var cd = User.Identity.Name;
            int id = Convert.ToInt32(cd);
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.GetVendorcnfDate(id).Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "O" && x.AppFlag3 == "R").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEditVendor(int id = 0)
        {
            if (id == 0)
                return View(new VendorConfirm());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.VendorConfirms.Where(x => x.id == id).FirstOrDefault<VendorConfirm>());
                }
            }
        }
        [HttpPost]
        public ActionResult AddOrEditVendor(VendorConfirm emp)
        {
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.AppFlag1 = "A";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("SelectVendorRequest");
            }

        }
        //**Vendor Step 1 END**//
        [HttpGet]
        public ActionResult ImageUploadVendor(int id = 0)
        {
            if (id == 0)
                return View(new VendorConfirm());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.VendorConfirms.Where(x => x.id == id).FirstOrDefault<VendorConfirm>());
                }
            }
        }
        [HttpPost]
        public ActionResult ImageUploadVendor(VendorConfirm emp)
        {
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.AppFlag1 = "A";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("SelectVendorRequest");
            }

        }
        //**Vendor step 2 Start**//
        //public ActionResult VendorAcknowledgement()
        //{

        //    var req = databaseManager.VendorConfirms.Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "O" && x.AppFlag3 == "R" && x.VenID.ToString() == User.Identity.Name).Select(x => new { Text = x.VenReqID, Value = x.VenReqID }).OrderBy(e => e.Text).ToList();
        //    ViewBag.VenReq = new SelectList(req, "Value", "Text");

        //    return View("VendorAcknowledgement");


        //}
        public ActionResult VendorAcknowledgement()
        {
            var req = databaseManager.VendorConfirms.Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "O" && x.AppFlag3 == "R" && x.VenID.ToString() == User.Identity.Name).Select(x => new { Text = x.VenReqID, Value = x.VenReqID }).OrderBy(e => e.Text).ToList();
            ViewBag.VenReq = new SelectList(req, "Value", "Text");
            return View("VendorAcknowledgement");

        }
        public ActionResult GetVendorLastRequest(int reqid)
        {

            try
            {
                databaseManager.Configuration.ProxyCreationEnabled = false;
                var result = databaseManager.GetVendorcnfALL(reqid).ToList();
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "No item found" });

            }
        }


        public ActionResult SaveData(int? id, VendorConfirmM item, HttpPostedFileBase UploadImageb)
        {
            //bool status = false;
            //string mes = "";
            try
            {
                //var requestcount = databaseManager.RequestMasters.Where(x=>x.SE == stp.ToString() && x.AppFlag1=="I" && x.AppFlag2 == "O" && x.AppFlag3=="S").Count();
                using (LIVEEntities db = new LIVEEntities())
                {

                    if (item.UploadImageb != null)
                    {
                        VendorConfirm tb = new VendorConfirm();

                        //string fileNameb = Path.GetFileNameWithoutExtension(item.UploadImageb.FileName);
                        //string extensionb = Path.GetExtension(item.UploadImageb.FileName);
                        //fileNameb = fileNameb + DateTime.Now.ToString("yymmssff") + extensionb;
                        //item.PicUrl2 = fileNameb;
                        //item.UploadImageb.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images"), fileNameb));
                        string pic = System.IO.Path.GetFileName(UploadImageb.FileName);
                        string fileName = DateTime.Now.ToString("yymmssff") + pic;
                        string path = System.IO.Path.Combine(Server.MapPath("~/AppFile/Images"), fileName);
                        UploadImageb.SaveAs(path);
                        ResizeSettings resizeSetting = new ResizeSettings
                        {
                            Width = 350,
                            Height = 300,
                            Format = "png"
                        };
                        ImageBuilder.Current.Build(path, path, resizeSetting);
                        tb.id = item.id;
                        tb.VenReqID = item.VenReqID;
                        tb.VenID = item.VenID;
                        tb.ReqID = item.ReqID;
                        tb.RegionName = item.RegionName;
                        tb.AreaName = item.AreaName;
                        tb.CeAreaName = item.CeAreaName;
                        tb.DbName = item.DbName;
                        tb.cluster = item.cluster;
                        tb.PSRName = item.PSRName;
                        tb.SubRoute = item.SubRoute;
                        tb.OutletCode = item.OutletCode;
                        tb.OutLetName = item.OutLetName;
                        tb.OwnerName = item.OwnerName;
                        tb.ContactNum = item.ContactNum;
                        tb.ContactNumTwo = item.ContactNumTwo;
                        tb.ChannelName = item.ChannelName;
                        tb.signageheight = item.signageheight;
                        tb.signagewidth = item.signagewidth;
                        tb.remarks = item.remarks;
                        tb.msg = item.msg;
                        tb.SE = item.SE;
                        tb.signageheightNew = item.signageheightNew;
                        tb.signagewidthNew = item.signagewidthNew;
                        tb.dateLimit = item.dateLimit;
                        tb.dateLimitfinal = item.dateLimitfinal;
                        tb.feedback = item.feedback;
                        tb.PicUrl = item.PicUrl;
                        tb.Address = item.Address;
                        tb.PicUrl2 = fileName;
                        tb.AppFlag1 = item.AppFlag1;
                        tb.AppFlag2 = "A";
                        tb.AppFlag3 = item.AppFlag3;
                        tb.OrderBy = item.OrderBy;
                        tb.sinagetype = item.sinagetype;
                        tb.sinage = item.sinage;
                        tb.otherbrand = item.otherbrand;
                        tb.otherbrandd = item.otherbrandd;
                        var dateAndTime = DateTime.Now;
                        tb.installDate = dateAndTime.Date;
                        //tb.installDate = item.installDate;
                        databaseManager.VendorConfirms.Add(tb);
                        databaseManager.Entry(tb).State = EntityState.Modified;
                        databaseManager.SaveChanges();

                        databaseManager.Dispose();
                        ModelState.Clear();


                    }
                    return RedirectToAction("WorkOrderVendor");
                }
            }
            catch(Exception ex) {
                return RedirectToAction("WorkOrderVendor");
            }
        }

        public ActionResult WorkOrderVendor(VendorConfirm model)
        {
            var cd = User.Identity.Name;

            IEnumerable<VendorConfirm> list = databaseManager.VendorConfirms.Where(x => x.VenID.ToString() == cd && x.AppFlag3=="R").ToList();
            return View(list);

        }
        //**Vendor step 2 END**//

        public ActionResult PrintSelected(string[] ids,VendorConfirmM mod)
        {
            //Delete Selected 
            int[] id = null;
            if (ids != null)
            {
                id = new int[ids.Length];
                int j = 0;
                foreach (string i in ids)
                {
                    int.TryParse(i, out id[j++]);
                }
            }

            if (id != null && id.Length > 0)
            {
                List<VendorConfirm> allSelected = new List<VendorConfirm>();
                using (LIVEEntities dc = new LIVEEntities())
                {
                    allSelected = dc.VendorConfirms.Where(a => id.Contains(a.id)).ToList();
                    foreach (var i in allSelected)
                    {
                        
                        i.PrintFlag = "P";
                        i.AppFlag3 = "p";
                        i.PrintDate = DateTime.Now;
                        dc.Entry(i).State = EntityState.Modified;
                        
                    }
                    dc.SaveChanges();
                }
            }

            return RedirectToAction("WorkOrderVendor");
        }
        [HttpGet]
        public ActionResult PartialDeed(int id = 0)
        {
            if (id == 0)
                return View(new VendorConfirm());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }
    }
}


