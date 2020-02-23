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
using System.Data.Entity.Validation;

namespace SalesTest.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        // GET: Status
        public LIVEEntities db = new LIVEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllStatusOfRequest(VendorConfirm model)
        {
            var sup = new SelectList(
            new[]
                {
                new { ID = "R", Name = "Accept" },
                new { ID = "C", Name = "Cancel" },

                },
                "ID",
                "Name"
            );
            ViewBag.AppFlag3 = sup;
            IEnumerable<VendorConfirm> list = db.VendorConfirms.ToList();
            return View(list);

        }
    }
}