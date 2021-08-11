using Microsoft.Reporting.WebForms;
using SSS.BLL.Setups.Reporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;
using SSS.Property.Setups.Reports;


namespace SMSYSTEM.Views.Reporting
{
    public class WebFormController : Controller { }

    public static class WebFormMVCUtil
    {

        public static void RenderPartial(string partialName, object model)
        {
            //get a wrapper for the legacy WebForm context
            var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

            //create a mock route that points to the empty controller
            var rt = new RouteData();
            rt.Values.Add("controller", "WebFormController");

            //create a controller context for the route and http context
            var ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new WebFormController());

            //find the partial view using the viewengine
            var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;

            //create a view context and assign the model
            var vctx = new ViewContext(ctx, view,
                new ViewDataDictionary { Model = model },
                new TempDataDictionary(),TextWriter.Null);

            //render the partial view
            view.Render(vctx, System.Web.HttpContext.Current.Response.Output);
        }

    }
    public partial class Report : System.Web.Mvc.ViewPage
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               
                string technology = "";
                if (!this.IsPostBack)
                {


                    //    name = DropDownList1.SelectedItem.Text;
                     technology = DropDownList1.SelectedItem.Value;

                }

                LP_Report_Property objreportprprty = new LP_Report_Property();
                objreportprprty.ReportID = 11;
                objreportprprty.ReportName = "Report1";
                LP_Reporting_BLL objrprtbll = new LP_Reporting_BLL(objreportprprty);
                DataTable dt = objrprtbll.SelectReportData();
                string path = Path.Combine(Server.MapPath("~/Reports"), objreportprprty.ReportName + ".rdlc");
                ReportViewer1.LocalReport.ReportPath = path;//(Server.MapPath("~/Reports"), objreportprprty.ReportName + ".rdlc");
                ReportDataSource rds = new ReportDataSource();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                rds.Name = "DataSet1";


                rds.Value = ds.Tables[0];


                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                // this.ReportViewer1.LocalReport.DataSources.Add(rds1);

                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("DeliveryMan", DeliveryMan));
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter("Date", Convert.ToString(System.DateTime.Today)));
                ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {

            }
            //ReportViewer1 rv = new ReportViewer1();
            //ReportViewer1 rv = new Microsoft.Reporting.WebForms.ReportViewer();
            //rv.ProcessingMode = ProcessingMode.Local;
            //rv.LocalReport.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");
            //rv.LocalReport.Refresh();

            //DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            //// e.g. via DataAdapter
            ////// If your report needs parameters, they need to be set ...
            ////ReportParameter[] parameters = new ReportParameter[...];

            //ReportDataSource reportDataSource = new ReportDataSource();
            ////// Must match the DataSource in the RDLC
            //reportDataSource.Name = "Test_Pakistan_Consultant";
            //reportDataSource.Value = ds.Tables[0];

            ////// Add any parameters to the collection
            ////rv.LocalReport.SetParameters(parameters);
            //rv.LocalReport.DataSources.Add(reportDataSource);
            //rv.DataBind();

            //byte[] streamBytes = null;
            //string mimeType = "";
            //string encoding = "";
            //string filenameExtension = "";
            //string[] streamids = null;
            //Warning[] warnings = null;

            //streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            //return File(streamBytes, mimeType, "Report1.pdf");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}