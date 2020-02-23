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
    public class OrderRequestController : Controller
    {

        private LIVEEntities databaseManager = new LIVEEntities();
        // GET: OrderRequest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderGen()
        {
            var w = (from y in databaseManager.sUsers
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.PlantNo }).FirstOrDefault();
            var wn = databaseManager.sPlants.Where(x => x.PlantNo == w.PlantNo).FirstOrDefault();
            //var cust = databaseManager.sBenificiaries.Where(x => x.PlantID == wn.PlantNo && x.Status == "Y").Select(x => new { Text = x.BenificiaryName + " , " + x.BenificiaryID, Value = x.BenificiaryID }).OrderBy(e => e.Text).ToList();
            ViewBag.WarehouseID = wn.PlantNo;
            ViewBag.WarehouseIDLogin = wn.PlantName;

            //ViewBag.CustomerID = new SelectList(cust, "Value", "Text");
            ViewBag.sinagetype = new SelectList(databaseManager.SinageTypes, "sid", "sinageTypeName");
            ViewBag.otherbrand = new SelectList(databaseManager.OtherBrands, "id", "OBrandName");
            return View("OrderGen");
        }
        public JsonResult CheckOutletAvailability(int userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = databaseManager.RequestMasters.Where(x => x.OutletCode == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }
        public JsonResult GetOutletcode(int outletcode)
        {

            try
            {
                databaseManager.Configuration.ProxyCreationEnabled = false;
                var result = databaseManager.getOutletDataFromDurbin(outletcode).ToList();
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "Challan Details not found" });

            }

        }
        public JsonResult GetASMRequest(int reqid)
        {
            try
            {
                databaseManager.Configuration.ProxyCreationEnabled = false;
                var result = databaseManager.getDataForASM(reqid).ToList();
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = "No item found" });

            }
        }
        [HttpPost]
        public ActionResult SaveData(RequestVM item, HttpPostedFileBase UploadImage)
        {
            //bool status = false;
            //string mes = "";
            var w = (from y in databaseManager.sUsers
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.PlantNo }).FirstOrDefault();
            var ltyp = (from p in databaseManager.sUsers
                        where p.UserID.ToString() == User.Identity.Name
                        select p.LoginTypeID).Single();
            int stp = Convert.ToInt32(ltyp);
            var wn = databaseManager.sPlants.Where(x => x.PlantNo == w.PlantNo).FirstOrDefault();

            string s1 = w.PlantNo.ToString();
            string s2 = string.Concat(s1 + "1000000");
            int reqno = Convert.ToInt32(s2);
            var maxreqno = (from n in databaseManager.RequestMasters select n.ReqID).DefaultIfEmpty(reqno).Max();
            var maxrNo = maxreqno + 1;
            //int v = Int32(maxrNo);
            var step = databaseManager.SEFeedbackDo(stp).FirstOrDefault();
            try
            {
                //var requestcount = databaseManager.RequestMasters.Where(x=>x.SE == stp.ToString() && x.AppFlag1=="I" && x.AppFlag2 == "O" && x.AppFlag3=="S").Count();
                using (LIVEEntities db = new LIVEEntities())
                {

                    if (db.RequestMasters.Any(ac => ac.OutletCode == item.OutletCode))
                    {
                        return PartialView("Error");

                    }

                    else
                    {
                        RequestMaster tb = new RequestMaster();
                        //string fileName = Path.GetFileNameWithoutExtension(UploadImage.FileName);
                        //string extension = Path.GetExtension(UploadImage.FileName);
                        //fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;
                        //item.PicUrl = fileName;
                        //item.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images"), fileName));
                        string pic = System.IO.Path.GetFileName(UploadImage.FileName);
                        string fileName = DateTime.Now.ToString("yymmssff") + pic;
                        string path = System.IO.Path.Combine(Server.MapPath("~/AppFile/Images"), fileName);
                        UploadImage.SaveAs(path);
                        ResizeSettings resizeSetting = new ResizeSettings
                        {
                            Width = 350,
                            Height = 300,
                            Format = "png"
                        };
                        ImageBuilder.Current.Build(path, path, resizeSetting);
                        tb.ReqID = maxrNo;
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
                        tb.sinagetype = item.sinagetype;
                        //tb.sinage = item.sinage;
                        tb.otherbrand = item.otherbrand;
                        //tb.otherbrandd = item.otherbrandd;
                        tb.signageheight = item.signageheight;
                        tb.signagewidth = item.signagewidth;
                        tb.remarks = item.remarks;
                        tb.PicUrl = fileName;
                        tb.AppFlag1 = "I";
                        tb.AppFlag2 = "O";
                        tb.AppFlag3 = "S";
                        tb.SalesVolume = item.SalesVolume;
                        tb.SE = ltyp.ToString();
                        tb.CreateDate = DateTime.Now;
                        db.RequestMasters.Add(tb);
                        db.SaveChanges();

                        db.Dispose();
                        ModelState.Clear();
                        return RedirectToAction("OrderGen");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("OrderGen");
            }

        }




        [HttpPost]
        public ActionResult GetRequestById(int reqId)
        {
            List<RequestMaster> req = databaseManager.RequestMasters.Where(x => x.ReqID == reqId).ToList();

            return PartialView("GetRequestById", req);
        }
        //**ASM**//
        //***19/11/2019|AhsanHabibRabby***//

        public ActionResult GetRequest(int? id)
        {
            var request = databaseManager.RequestMasters.FirstOrDefault(x => x.id == id);

            var sup = new SelectList(
         new[]
             {
                new { ID = "A", Name = "Accept" },
                new { ID = "C", Name = "Cancel" },

             },
             "ID",
             "Name"
         );
            ViewBag.AppFlag1 = sup;
            return PartialView(request);
        }

        [HttpPost]
        public ActionResult UpdateRequest(RequestMaster model)
        {
            {
                var cd = (from y in databaseManager.sUsers
                          where y.UserID.ToString() == User.Identity.Name
                          select new { y.LoginTypeID }).FirstOrDefault();
                using (LIVEEntities db = new LIVEEntities())
                {

                    model.ASM = cd.LoginTypeID;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();



                }
                return RedirectToAction("ListofRequest");

            }

        }
        //**ASM New**//
        //***12/10/2019|AhsanHabibRabby***//
        public ActionResult ListofRequest()
        {
            return View();
        }
        public ActionResult GetDataASM()
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            var se = (from y in databaseManager.sUsers
                      join x in databaseManager.CustomerExecutives on y.LoginTypeID equals x.CustomerExecutiveID.ToString()
                      where x.TeritoryDevelopmentManagerID.ToString() == cd.LoginTypeID
                      select new { y.LoginTypeID }).ToList();
            int userId = Convert.ToInt32(cd == null ? "0" : cd.LoginTypeID);

            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.getDataAM(userId).Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEditASM(int id = 0)
        {
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
        /// <summary>
        /// ASM_EDIT
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult ASM_Detail(int id = 0)
        {
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
       
        [HttpPost]
        public ActionResult AddOrEditASM(RequestMaster emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.ASM = cd.LoginTypeID;
                emp.ASMdate = DateTime.Now;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequest");
            }

        }

        //**ASM New END**//

        //**BrandManger**//
        //***12/10/2019|AhsanHabibRabby***//
        public ActionResult ListofRequestHOS()
        {

            return View();
        }
        [HttpGet]
        public ActionResult BM_Detail(int id = 0)
        {
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
        public ActionResult MK_Detail(int id = 0)
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
        public ActionResult GetDataBM()
        {
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.getDataForASMsales().Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
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
        [HttpPost]
        public ActionResult AddOrEdit(RequestMaster emp)
        {
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.HOS = User.Identity.Name;
                emp.HOSdate = DateTime.Now;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequestHOS");
            }

        }
        //**BrandMangerEND**//

        //**MKT Personnel**////Vendor+MKT Pesronnel page 2
        public ActionResult SelectVendorAcknowledgement()
        {
            return View();
        }
        public ActionResult SelectVendor()
        {

            var all = databaseManager.VendorConfirms.Select(a => a.ReqID).ToList();


            var ser = databaseManager.RequestMasters
                            .Where(x => !(all.Contains(x.ReqID)) && x.AppFlag1 == "A" && x.AppFlag2 == "A" && x.AppFlag3 == "S")
                            .Select(x => new { Text = x.ReqID, Value = x.ReqID }).OrderBy(e => e.Text).ToList();

            ViewBag.ReqID = new SelectList(ser, "Value", "Text");
            var red = databaseManager.sUsers.Where(x => x.RoleId == 101 || x.RoleId == 102).Select(x => new { Text = x.UserName, Value = x.UserID }).OrderBy(e => e.Text).ToList();
            ViewBag.VenID = new SelectList(red, "Value", "Text");
            IEnumerable<RequestMaster> list = databaseManager.RequestMasters.Where(y => y.AppFlag1 == "A" && y.AppFlag2 == "A" && y.AppFlag3 == "S").ToList();
            return View(list);

        }
        public ActionResult GetDataMKT()
        {
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.GetVendorcnf().Where(x => x.AppFlag1 == "A" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult VendorReview(int id = 0)
        {
            if (id == 0)
                return View(new RequestMaster());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.VendorConfirms.Where(x => x.id == id).FirstOrDefault<VendorConfirm>());
                }
            }
        }
        [HttpPost]
        public ActionResult VendorReview(VendorConfirm emp)
        {
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.AppFlag3 = "R";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("SelectVendorAcknowledgement");
            }

        }
        //Vendor+MKT Pesronnel page 2 END
        //**Status Of a Request By SE**//
        public ActionResult StatusOfRequest(RequestMaster model)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();

            IEnumerable<RequestMaster> list = databaseManager.RequestMasters.Where(x => x.SE == cd.LoginTypeID || x.ASM == cd.LoginTypeID).ToList();
            return View(list);
        }
        //**Sales officer all Vendor status
        public ActionResult GetVendorStatus()
        {
            return View();
        }

        public ActionResult MKT()
        {

            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.MKTDataUpdate().ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
                
            }
        }

        public ActionResult FinalStatusOfRequest(VendorConfirm model)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();

            IEnumerable<VendorConfirm> list = databaseManager.VendorConfirms.Where(x => x.SE == cd.LoginTypeID).ToList();
            return View(list);

        }

        //**SE Acknowledgement feedback***////
        public ActionResult Sefeedback()
        {

            return View();
        }
        public ActionResult GetDataSE()
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.GetSEcnf().Where(x => x.SE == cd.LoginTypeID && x.AppFlag1 == "A" && x.AppFlag2 == "A" && x.AppFlag3 == "R").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SE(int id = 0)
        {
            if (id == 0)
                return View(new RequestMaster());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.VendorConfirms.Where(x => x.id == id).FirstOrDefault<VendorConfirm>());
                }
            }
        }
        [HttpPost]
        public ActionResult SE(VendorConfirm emp)
        {

            using (LIVEEntities db = new LIVEEntities())
            {
                emp.AppFlag3 = "H";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("Sefeedback");
            }

        }

        [HttpPost]
        public ActionResult SaveDataForVendor(VendorConfirmM item)
        {
            bool status = false;
            string mes = "";
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
                databaseManager.SaveChanges();
                status = true;


                return new JsonResult { Data = new { status = status, mes = mes, v = v } };
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                return Json(new { status = "error", message = "Error Generate" });

            }


        }

        //**RSM New**//
        //***12/10/2019|AhsanHabibRabby***//
        public ActionResult ListofRequestRSM()
        {
            return View();
        }
        public ActionResult GetDataRSM()
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            var se = (from y in databaseManager.sUsers
                      join x in databaseManager.CustomerExecutives on y.LoginTypeID equals x.CustomerExecutiveID.ToString()
                      where x.TeritoryDevelopmentManagerID.ToString() == cd.LoginTypeID
                      select new { y.LoginTypeID }).ToList();
            int userId = Convert.ToInt32(cd == null ? "0" : cd.LoginTypeID);

            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.getDataAM(userId).Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEditRSM(int id = 0)
        {
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
        /// <summary>
        /// ASM_EDIT
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult RSM_Detail(int id = 0)
        {
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

        //**ASM New END**//

    }
}
