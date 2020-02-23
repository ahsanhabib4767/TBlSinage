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
    public class MEMReportController : Controller
    {
        // GET: MEMReport
        public LIVEEntities db  = new LIVEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MEMReports()
        {


            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<getVisiCoolerData_Result> paymentStatus = db.getVisiCoolerData().ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\memdocument.rdlc";
            // ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("memdocument", paymentStatus);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");
        }
    }
}