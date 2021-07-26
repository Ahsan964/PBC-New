using SSS.BLL.Report;
using SSS.Property.Setups;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using excel = Microsoft.Office.Interop.Excel;

namespace SMSYSTEM.Controllers
{
    public class OpeningController : Controller
    {
        // GET: Opening
        public ActionResult ProductOpening()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];

            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public JsonResult UploadExcel(ProductOpening ObjOpening)
        {
            ViewBag.msg = "";
            try
            {

                if (SaveExcelFile(ObjOpening.ExcelFile))
                {
                    LP_Inventory objINV = new LP_Inventory();
                    DataTable dt =Helper.ToDataTable(ReadExcelFile(ObjOpening.ExcelFile.FileName));
                    LP_Inventory_BLL objINVBLL = new LP_Inventory_BLL();
                    objINVBLL.UploadExcel(dt, dt);
                    // objattendancebll = new Attendance_BLL();
                    var flag = true;// objattendancebll.Insert(objattendanceproperty.tbl_Attendance);
                    if (flag)
                    {
                        ViewBag.msg = "Success";
                    }
                    else
                    {
                        ViewBag.msg = "Contact Administrator";
                    }

                }

                return Json(new { success = true, msg = ViewBag.msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = "Contact Administrator" }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool SaveExcelFile(HttpPostedFileBase ExcelFileAttendence)
        {
            try
            {
                var file = ExcelFileAttendence;
                // var filename = Path.GetFileName(ExcelFileAttendence.FileName);
                var filename = ExcelFileAttendence.FileName;
                ExcelFileAttendence.SaveAs(Server.MapPath("/ExcelFiles/" + filename));
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private List<LP_Inventory> ReadExcelFile(string filename)
        {
            try
            {

                int rowstart, productIdx, stock, productTypeIdx, unitPrice, totalAmount, creationDate;
                rowstart = Convert.ToInt32(ConfigurationManager.AppSettings["RowStartReading"].ToString());
                productIdx = Convert.ToInt32(ConfigurationManager.AppSettings["productIdx"].ToString());
                stock = Convert.ToInt32(ConfigurationManager.AppSettings["stock"].ToString());
                productTypeIdx = Convert.ToInt32(ConfigurationManager.AppSettings["productTypeIdx"].ToString());
                unitPrice = Convert.ToInt32(ConfigurationManager.AppSettings["unitPrice"].ToString());
                totalAmount = Convert.ToInt32(ConfigurationManager.AppSettings["totalAmount"].ToString());
                creationDate = Convert.ToInt32(ConfigurationManager.AppSettings["creationDate"].ToString());

                List<LP_Inventory> Listattendance = new List<LP_Inventory>();
                LP_Inventory Inventory;
                string filepath = Server.MapPath("/ExcelFiles/" + filename).ToString();



                excel.Application xlApp = new excel.Application();
                excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filepath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                excel._Worksheet xlWorksheet = (excel._Worksheet)xlWorkbook.Sheets[1];
                excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                for (int i = rowstart; i <= rowCount; i++)
                {
                    
                    Inventory = new LP_Inventory();

                    //PID
                    if (xlRange.Cells[i, productIdx] != null && xlRange.Cells[i, productIdx].Value2 != null)
                    {
                        Inventory.productIdx =Convert.ToInt16( xlRange.Cells[i, productIdx].Value2.ToString());
                    }
                    //Stock
                    if (xlRange.Cells[i, stock] != null && xlRange.Cells[i, stock].Value2 != null)
                    {
                        Inventory.stock = Convert.ToInt16(xlRange.Cells[i, stock].Value2.ToString());
                    }
                    //Producttype
                    if (xlRange.Cells[i, productTypeIdx] != null && xlRange.Cells[i, productTypeIdx].Value2 != null)
                    {
                        Inventory.productTypeIdx = xlRange.Cells[i, productTypeIdx].Value2.ToString();
                    }
                    //UnitPrice
                    if (xlRange.Cells[i, unitPrice] != null && xlRange.Cells[i, unitPrice].Value2 != null)
                    {
                        Inventory.unitPrice = Convert.ToDecimal(xlRange.Cells[i, unitPrice].Value2.ToString());
                    }
                    //Total
                    if (xlRange.Cells[i, totalAmount] != null && xlRange.Cells[i, totalAmount].Value2 != null)
                    {
                        Inventory.totalAmount =Convert.ToDecimal(xlRange.Cells[i, totalAmount].Value2.ToString());
                    }
                    //CreateDate
                    if (xlRange.Cells[i, creationDate] != null && xlRange.Cells[i, creationDate].Value2 != null)
                    {
                        String date = xlRange.Cells[i, creationDate].Value2.ToString();

                        //string newdate = DateTime.ParseExact(date, "yy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy/MM/dd");
                        Inventory.creationDate = DateTime.Now;// Convert.ToDateTime(newdate);// DateTime.ParseExact(xlRange.Cells[i, AttendanceDate].Value2.ToString(), "yyyy/MM/dd", null);
                        //attendance.Attendance_Date =Convert.ToDateTime(xlRange.Cells[i, AttendanceDate].Value2.ToString());
                    }

                   

                    
                    Listattendance.Add(Inventory);

                }

                return Listattendance;


            }


            catch (Exception ex)
            {
                using (var tw = new StreamWriter(Server.MapPath("/ExcelFiles/" + "abc.txt"), true))
                {
                    tw.WriteLine(ex.Message);
                }
                return new List<LP_Inventory>();
            }
        }

        //public DataTable InvLogsData(DataTable dt)
        //{
        //    try
        //    {
                
                
        //    }

        //    catch(Exception ex)
        //    {
        //        using (var tw = new StreamWriter(Server.MapPath("/ExcelFiles/" + "InvLogsData.txt"), true))
        //        {
        //            tw.WriteLine(ex.Message);
        //        }
        //        return new DataTable();
        //    }

        //}
    }
}