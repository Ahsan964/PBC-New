using Newtonsoft.Json;
using SSS.BLL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class ActivityController : BaseController
    {
        // GET: Activity
        #region Activity
        public ActionResult Activity()
        {  // GET: Activity

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];
            LP_Activity_Property objActivityVM;
            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page) && Session["ISADMIN"] != null && Convert.ToBoolean(Session["ISADMIN"].ToString()) == true)
            // if (Session["LOGGEDIN"] != null)

            {
                objActivityVM = new LP_Activity_Property();
                if (objActivityVM.idx > 0)
                {

                }
                else
                {
                    LP_Activity_BLL objbll = new LP_Activity_BLL();
                    objActivityVM.salesOrderLST = Helper.ConvertDataTable<LP_SalesOrder_Master_Property>(GetAllSalesInvoice());
                    objActivityVM.vendorCatLST = Helper.ConvertDataTable<Vendor_Category_Property>(GetAllVendorsCategory());


                }

                return View("Activity", objActivityVM);
            }
            else
            {
                if (Session["LoggedIn"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("NotAuthorized", "Account");
                }

            }

        }
        [HttpGet]
        public JsonResult SearchProductsInDetail(int Id)
        {
            try
            {
                LP_PInvoice_BLL objbll = new LP_PInvoice_BLL();
                //DataTable tblFiltered;
                if (Id != null)
                {



                    var Data = Helper.ConvertDataTable<LP_SalesOrder_Detail_Property>(GetAllSalesInvoiceDetails(Id));//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
                    return Json(new { data = Data, success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "Error Occured", success = false, statuscode = 500 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { data = "Session Expired", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult SearchVendorsOnCatIdx(int Id)
        {
            try
            {

                if (Id > 0)
                {



                    var Data = Helper.ConvertDataTable<Vendors_Property>(GetVendorByVendorCat(Id));//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
                    return Json(new { data = Data, success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "Error Occured", success = false, statuscode = 500 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(new { data = "Session Expired", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult AddUpdate(LP_Activity_Property objVoucher)
        {
            try
            {
                bool flag = false;

                LP_Activity_Property obj = new LP_Activity_Property();
                obj.idx = objVoucher.idx;
                obj.activityDate = DateTime.Now.ToString("yyyy-MM-dd");
                obj.creationDate = DateTime.Now.ToString("yyyy-MM-dd");
                obj.createdBy = Convert.ToInt16(Session["UID"].ToString());
                obj.orderIdx = objVoucher.orderIdx;
                obj.productIdx = objVoucher.productIdx;
                obj.vendorCatIdx = objVoucher.vendorCatIdx;
                obj.vendorIdx = objVoucher.vendorIdx;
                obj.size = objVoucher.size;
                obj.qty = objVoucher.qty;
                obj.activityPrice = objVoucher.activityPrice;
                obj.description = objVoucher.description;
                LP_Activity_BLL objBLL = new LP_Activity_BLL(obj);
                flag = objBLL.Insert();
                if (flag)
                {
                    return Json(new { data = "Inserted", success = flag, msg = flag == true ? "Successfull" : "Success", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "", success = flag, msg = flag == true ? "Failure" : "Failure", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Local Finish Good
        public ActionResult FinsihProducts(int? id)
        {  // GET: Finish Product

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];

            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page) && Session["ISADMIN"] != null && Convert.ToBoolean(Session["ISADMIN"].ToString()) == true)
            // if (Session["LOGGEDIN"] != null)

            {
                LP_FinsihProduct_Property objPInvoiceVM = new LP_FinsihProduct_Property();
                LP_FinishProduct_BLL objFPBLL = new LP_FinishProduct_BLL();


                objPInvoiceVM.salesOrderLST = Helper.ConvertDataTable<LP_SalesOrder_Master_Property>(objFPBLL.SelectAll());

                objPInvoiceVM.ProductLST = Helper.ConvertDataTable<Product_Property>(ViewAllProducts());

                //objGRNVM_Property.Doc_No = "GRN-001";
                if (id > 0)
                {
                    //update 


                    return View("FinsihProducts", objPInvoiceVM);
                }
                else
                {

                    return View("FinsihProducts", objPInvoiceVM);

                }
            }
            else
            {
                if (Session["LoggedIn"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return RedirectToAction("NotAuthorized", "Account");
                }

            }

        }

        public JsonResult GetAllAcitvityBYOrderId(int id)
        {

            if (Session["LOGGEDIN"] != null)
            {
                try
                {


                    LP_FinishProduct_BLL objPIBLL = new LP_FinishProduct_BLL();
                    var Data = JsonConvert.SerializeObject(objPIBLL.SelectAllActivityOnSoNumber(id));
                    return Json(new { data = Data, success = true, statuscode = 200, count = Data.Length }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { data = "Session Expired", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult AddUpdateFP(LP_FinsihProduct_Property objPI)
        {
            try
            {
                bool flag = false;

                LP_FinsihProduct_Property obj = new LP_FinsihProduct_Property();
                obj.orderIdx = objPI.orderIdx;
                if (objPI.InventoryDetails.Count > 0)
                {
                    obj.DetailData = Helper.ToDataTable<LP_InventoryLogs_Property>(objPI.InventoryDetails);
                    LP_FinishProduct_BLL objBLL = new LP_FinishProduct_BLL(obj);
                    flag = objBLL.Insert();
                    if (flag)
                    {
                        return Json(new { data = "Inserted", success = flag, msg = flag == true ? "Successfull" : "Success", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = "", success = flag, msg = flag == true ? "Failure" : "Failure", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { data = "", success = flag, msg = flag == true ? "Failure" : "Failure", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                }
                //obj.activityDate = DateTime.Now.ToString("yyyy-MM-dd");
                //obj.creationDate = DateTime.Now.ToString("yyyy-MM-dd");
                //obj.createdBy = Convert.ToInt16(Session["UID"].ToString());
                //obj.orderIdx = objVoucher.orderIdx;
                //obj.productIdx = objVoucher.productIdx;
                //obj.vendorCatIdx = objVoucher.vendorCatIdx;
                //obj.vendorIdx = objVoucher.vendorIdx;
                //obj.size = objVoucher.size;
                //obj.qty = objVoucher.qty;
                //obj.activityPrice = objVoucher.activityPrice;
                //obj.description = objVoucher.description;



            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}