using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTest.Models;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using ReportViewerForMvc;

namespace SalesTest.Controllers
{
    public class RSMReportController : Controller
    {
        // GET: RSMReport
        public LIVEEntities db = new LIVEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyReports(DateTime d1, DateTime d2)
        {
            var cd = (from y in db.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();

            int userId = Convert.ToInt32(cd == null ? "0" : cd.LoginTypeID);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            //var v = (from x in db.FrdRequestMasters where x.ReqID == reqid select x).FirstOrDefault();
            List<getDataRSMMarketing_Result> obj = db.getDataRSMMarketing(userId,d1,d2).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports/rsmdataset.rdlc";

            //ReportParameter rp2 = new ReportParameter("wName", v.OrderId.ToString());

            // reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp2 });

            ReportDataSource rdc = new ReportDataSource("rsmdataset", obj);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");

        }
    }
}