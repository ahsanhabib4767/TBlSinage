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
    [Authorize]
    public class DateWiseReportController : Controller
    {
        // GET: Report
        public LIVEEntities db = new LIVEEntities();
        public ActionResult Index()
        {
            ViewBag.VenID = new SelectList(db.tblVendors, "VenID", "VendorName");
            return View();
        }

        public ActionResult InstallDateWise(string bDate, string eDate)
        {
            DateTime d1 = Convert.ToDateTime(bDate);
            DateTime d2 = Convert.ToDateTime(eDate);
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<ReportForDatewiseInstall_Result> paymentStatus = db.ReportForDatewiseInstall(d1, d2).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\installdatewisereport.rdlc";
            // ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("installdatewisereport", paymentStatus);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");
        }
    }
}