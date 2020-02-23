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
    public class ReportController : Controller
    {
        // GET: Report
        public LIVEEntities db = new LIVEEntities();
        public ActionResult Index()
        {
            var cd = (from y in db.sUsers
                      where y.UserID.ToString() == User.Identity.Name
                      select new { y.LoginTypeID }).FirstOrDefault();

           
            var req = db.VendorConfirms.Where(x =>x.SE==cd.LoginTypeID).Select(x => new { Text = x.ReqID, Value = x.ReqID }).OrderBy(e => e.Text).ToList();

            ViewBag.ReqID = new SelectList(req, "Value", "Text");
            
            return View();
        }
        public ActionResult WOStatus(Nullable<int> reqid)
        {

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            //var v = (from x in db.FrdRequestMasters where x.ReqID == reqid select x).FirstOrDefault();
            List<workOrderFinal_Result> obj = db.workOrderFinal(reqid).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports/statusworkorder.rdlc";

            //ReportParameter rp2 = new ReportParameter("wName", v.OrderId.ToString());

            // reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp2 });

            ReportDataSource rdc = new ReportDataSource("requeststatus", obj);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");

        }

        public ActionResult Workorder(Nullable<int> reqid)
        {

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            //var v = (from x in db.FrdRequestMasters where x.ReqID == reqid select x).FirstOrDefault();
            List<workOrderforVendor_Result> obj = db.workOrderforVendor(reqid).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports/Invoice.rdlc";

            //ReportParameter rp2 = new ReportParameter("wName", v.OrderId.ToString());

            // reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp2 });

            ReportDataSource rdc = new ReportDataSource("workorderdataset", obj);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");

        }

        public ActionResult MyVendor(Nullable<int> reqid)
        {

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            //var v = (from x in db.FrdRequestMasters where x.ReqID == reqid select x).FirstOrDefault();
            List<GetVendorcnfALL_Result> obj = db.GetVendorcnfALL(reqid).ToList();
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports/vendorstatus.rdlc";

            //ReportParameter rp2 = new ReportParameter("wName", v.OrderId.ToString());

            // reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp2 });

            ReportDataSource rdc = new ReportDataSource("vendorstatus", obj);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");

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

        public ActionResult VendorNameWise(int vid)
        {
            
            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<ReportForVendorwiseInstall_Result> paymentStatus = db.ReportForVendorwiseInstall(vid).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\vendorwisereport.rdlc";
            // ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("vendorwiseinstall", paymentStatus);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");
        }

        public ActionResult AllVendorNameWise()
        {

            ReportViewer reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local,
                SizeToReportContent = true,
                Width = Unit.Percentage(50),
                Height = Unit.Percentage(50)
            };


            List<ReportForAllVendorwiseInstall_Result> ct = db.ReportForAllVendorwiseInstall().ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\allvendorwisereport.rdlc";
            // ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("allvendordataset", ct);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");
        }
        public ActionResult VendorOwnReport()
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


            List<ReportForMyVendor_Result> paymentStatus = db.ReportForMyVendor(userId).ToList();

            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\ownvendor.rdlc";
            // ReportParameter rp1 = new ReportParameter("DateParameter", d1.ToString("dd-MMM-yy") + " to " + d2.ToString("dd-MMM-yy"));



            //reportViewer.LocalReport.SetParameters(new ReportParameter[] { rp1 });



            ReportDataSource rdc = new ReportDataSource("ownvendor", paymentStatus);
            reportViewer.LocalReport.DataSources.Add(rdc);

            reportViewer.LocalReport.Refresh();
            reportViewer.Visible = true;

            ViewBag.ReportViewer = reportViewer;
            return PartialView("Status");
        }

        public ActionResult MEMReport()
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