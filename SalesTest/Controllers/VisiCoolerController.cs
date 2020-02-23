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
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using ImageResizer;


namespace SalesTest.Controllers
{
    public class VisiCoolerController : Controller
    {

        // GET: VisiCooler
        private LIVEEntities databaseManager = new LIVEEntities();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> LoginAsync()
        {
            string Username = "TBLPSSAccess";
            string Password = "E27Jh*rT6L&=Ckfwn%4QZk2-*rT6L&=Ckfwn";
            var keyValues = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username", Username),
                new KeyValuePair<string, string>("Password", Password),
                new KeyValuePair<string, string>("grant_type", "password")
            };
            var request = new HttpRequestMessage(
               HttpMethod.Post, "https://durbin.com.bd/Api/PSRToken");

            request.Content = new FormUrlEncodedContent(keyValues);


            var client = new HttpClient();
            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
            var user = jwtDynamic.Value<string>("name");
            var accessToken = jwtDynamic.Value<string>("access_token");
            var accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
            return accessToken;
        }
        public async Task<JsonResult> GetOutLetAsync(int outletcode)
        {
            OutLet valueOutLet = new OutLet();
            string accessToken;

            accessToken = await LoginAsync();
            if (accessToken == null)
            {
                accessToken = await LoginAsync();
            }
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", accessToken);

            var json = JsonConvert.SerializeObject(outletcode);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = await client.PostAsync("https://durbin.com.bd/api/ApiTblpssAccess/ApiGetOutletSales?outletcode=" + outletcode, content);

            var result1 = request.Content.ReadAsStringAsync().Result.Split(':')[4].ToString().Split(',')[0];
            //var result1 = result1.ToString().Replace(",\\OutletId","");
            var result2 = databaseManager.getOutletDataFromDurbin(outletcode).ToList();
            var result = new { result1,result2};
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public ActionResult OrderForVisi()
        {
            var w = (from y in databaseManager.sUsers
                     where y.UserID.ToString() == User.Identity.Name
                     select new { y.PlantNo }).FirstOrDefault();
            var wn = databaseManager.sPlants.Where(x => x.PlantNo == w.PlantNo).FirstOrDefault();
            //var cust = databaseManager.sBenificiaries.Where(x => x.PlantID == wn.PlantNo && x.Status == "Y").Select(x => new { Text = x.BenificiaryName + " , " + x.BenificiaryID, Value = x.BenificiaryID }).OrderBy(e => e.Text).ToList();
            ViewBag.WarehouseID = wn.PlantNo;
            ViewBag.WarehouseIDLogin = wn.PlantName;
            ViewBag.VisiSize = new SelectList(databaseManager.VisicoolerSizes, "id", "VisiSize");
            var visi = new SelectList(
            new[]
                {
                       new { ID = "Y", Name = "Yes" },
                       new { ID = "N", Name = "No" },
                },
                "ID",
                "Name"
            );
            ViewBag.NightGuard = visi;
            return View("OrderForVisi");
        }

        [HttpPost]
        public ActionResult SaveData(RequestVisicoolerVM item, HttpPostedFileBase UploadImage1, HttpPostedFileBase UploadImage2, HttpPostedFileBase UploadImage3)
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
            string s2 = string.Concat(s1 + "5000000");
            int reqno = Convert.ToInt32(s2);
            var maxreqno = (from n in databaseManager.RequestVisicoolers select n.ReqID).DefaultIfEmpty(reqno).Max();
            var maxrNo = maxreqno + 1;
            try
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    if (ModelState.IsValid)
                    {
                        RequestVisicooler tb = new RequestVisicooler();
                        string pic1 = System.IO.Path.GetFileName(UploadImage1.FileName);
                        string fileName1 = DateTime.Now.ToString("yymmssff") + pic1;
                        string path1 = System.IO.Path.Combine(Server.MapPath("~/VisiCoolerImage/Application"), fileName1);
                        UploadImage1.SaveAs(path1);
                        ResizeSettings resizeSetting = new ResizeSettings
                        {
                            Width = 350,
                            Height = 300,
                            Format = "png"
                        };
                        ImageBuilder.Current.Build(path1, path1, resizeSetting);
                        string pic2 = System.IO.Path.GetFileName(UploadImage2.FileName);
                        string fileName2 = DateTime.Now.ToString("yymmssff") + pic2;
                        string path2 = System.IO.Path.Combine(Server.MapPath("~/VisiCoolerImage/TradeLicense"), fileName2);
                        UploadImage2.SaveAs(path2);

                        ImageBuilder.Current.Build(path2, path2, resizeSetting);
                        string pic3 = System.IO.Path.GetFileName(UploadImage3.FileName);
                        string fileName3 = DateTime.Now.ToString("yymmssff") + pic3;
                        string path3 = System.IO.Path.Combine(Server.MapPath("~/VisiCoolerImage/VoterID"), fileName3);
                        UploadImage3.SaveAs(path3);

                        ImageBuilder.Current.Build(path3, path3, resizeSetting);
                        tb.ReqID = maxrNo;
                        tb.RegionName = item.RegionName;
                        tb.AreaName = item.AreaName;
                        tb.CeAreaName = item.CeAreaName;
                        tb.DbName = item.DbName;
                        tb.cluster = item.cluster;
                        tb.PSRName = item.PSRName;
                        tb.SubRoute = item.SubRoute;
                        tb.OutletCode = item.OutletCode;
                        tb.Address = item.Address;
                        tb.OutLetName = item.OutLetName;
                        tb.OwnerName = item.OwnerName;
                        tb.ContactNum = item.ContactNum;
                        //tb.ContactNumTwo = item.ContactNumTwo;
                        tb.ChannelName = item.ChannelName;
                        //tb.HaveVisicooler = item.HaveVisicooler;
                        tb.remarks = item.remarks;
                        tb.visicooleNeed = item.visicooleNeed;
                        tb.AppFlag1 = "I";
                        tb.AppFlag2 = "O";
                        tb.AppFlag3 = "S";
                        tb.SE = ltyp.ToString();
                        tb.CreateDate = DateTime.Now;
                        tb.NightGuard = item.NightGuard;
                        tb.VisiSize = item.VisiSize;
                        tb.TotalCase = item.TotalCase;
                        tb.VisiApplicationImg = fileName1;
                        tb.TradeLicenseImg = fileName2;
                        tb.VoterIDImg = fileName3;
                        
                        db.RequestVisicoolers.Add(tb);
                        db.SaveChanges();
                        db.Dispose();
                        ModelState.Clear();
                        return RedirectToAction("OrderForVisi");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("OrderForVisi");
            }
            return View();
        }
           

        public ActionResult ListofRequestASM()
        {
            return View();
        }
        public ActionResult GetDataASMVisiCooler()
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
                var result = databaseManager.getDataAMVisi(userId).Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "O" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult PartialASM(int id = 0)
        {
            if (id == 0)
                return View(new RequestVisicooler());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }
        [HttpGet]
        public ActionResult PartialASMImg(int id = 0)
        {
            if (id == 0)
                return View(new RequestVisicooler());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }
        public JsonResult SendSMStoDB(string phoneno, string sms)
        {
            bool status = false;
            string mes = "";

            HttpWebRequest request;
            HttpWebResponse response = null;
            String url;
            String host;

            if (ModelState.IsValid)
            {
                try
                {
                    //  var customerphone = db.Customers.Where(x=>x.CustomerID == )
                    status = true;
                    host = "https://vas.banglalinkgsm.com/sendSMS/sendSMS?msisdn=" + phoneno + "&message=" + sms + "&userID=" + "tranbevltd" + "&passwd=" + "92b034a909c050209caac7d5ccc0bf64" + "&sender=" + "8801969900240" + "";
                    url = host;
                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    return new JsonResult { Data = new { status = status, mes = mes } };
                }
                catch (Exception ex)
                {
                    return Json(new { status = "error", message = "sms Not Send" });
                    //throw ex;
                }
            }
            else
            {
                status = false;
            }
            return new JsonResult { Data = new { status = status, mes = mes } };
        }

        [HttpPost]
        public ActionResult AddOrEditASM(RequestVisicooler emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.ASM = cd.LoginTypeID;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequestASM");
            }

        }

        //**ASM New END**//
        //**RSM New**//
        //***16/01/2020|AhsanHabibRabby***//
        public ActionResult ListofRequestRSMVisi()
        {
            return View();
        }
        public ActionResult GetDataRSMVisiCooler()
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            var se = (from y in databaseManager.sUsers
                      join x in databaseManager.TerritoryDevelopmentManagers on y.LoginTypeID equals x.TerritoryDevelopmentManagerID.ToString()
                      where x.RSMID.ToString() == cd.LoginTypeID
                      select new { y.LoginTypeID }).ToList();
            int userId = Convert.ToInt32(cd == null ? "0" : cd.LoginTypeID);

            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.getDataRSMVisi(userId).Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "A" && x.AppFlag3 == "S").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult PartialRSM(int id = 0)
        {
            if (id == 0)
                return View(new RequestVisicooler());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }


        [HttpPost]
        public ActionResult AddOrEditRSM(RequestVisicooler emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.RSM = cd.LoginTypeID;
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequestRSMVisi");
            }

        }

        //**RSM New END**//

        //**MEM New Starts**//
        //***16/01/2020|AhsanHabibRabby***//
        public ActionResult ListofRequestMEM()
        {
            return View();
        }

        public ActionResult GetDataMEMVisiCooler()
        {
            using (LIVEEntities db = new LIVEEntities())

            {
                var result = databaseManager.getDataMEMVisi().Where(x => x.AppFlag1 == "I" && x.AppFlag2 == "A" && x.AppFlag3 == "A").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PartialMEM(int id = 0)
        {
            if (id == 0)
                return View(new RequestVisicooler());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }


        [HttpPost]
        public ActionResult AddOrEditMEM(RequestVisicooler emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.MEM = cd.LoginTypeID;
                emp.AppFlag3 = "D";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequestMEM");
            }

        }

        //**END MEM

        //**SE
        public ActionResult ListofRequestSE()
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
                var result = databaseManager.getDataSEVisi().Where(x => x.SE == cd.LoginTypeID && x.AppFlag1 == "I" && x.AppFlag2 == "A" && x.AppFlag3 == "D").ToList();
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PartialSE(int id = 0)
        {
            if (id == 0)
                return View(new RequestVisicooler());
            else
            {
                using (LIVEEntities db = new LIVEEntities())
                {
                    return View(db.RequestVisicoolers.Where(x => x.id == id).FirstOrDefault<RequestVisicooler>());
                }
            }
        }


        [HttpPost]
        public ActionResult AddOrEditSE(RequestVisicooler emp)
        {
            var cd = (from y in databaseManager.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();
            using (LIVEEntities db = new LIVEEntities())
            {
                emp.AppFlag3 = "P";
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return View("ListofRequestMEM");
            }

        }


        public ActionResult OutletInfo()
        {
            return View();
        }
        public ActionResult DeedCopyUploadPage()
        {
            return View();
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

    }
}
