using SSS.BLL.Setups;
using SSS.BLL.Setups.Accounts;
using SSS.BLL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Setups.Accounts;
using SSS.Property.Transactions;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class FinanceController : BaseController
    {
        #region GV
        LP_GeneralVoucher_Property objGV;
        OpenigEntryVM objOP;
        LP_Opening_BLL objOPBLL;
        SalesOrderVM_Property objSalesOrderVM_Property;
        AccountMasterGL objGL;
        LP_SalesOrder_Master_Property objSalesOrderProperty;
        LP_SalesOrder_BLL objSalesOrderBll;
        LP_GeneralVoucher_BLL objGVBLL;

        // GET: Finance
        public ActionResult GeneralVoucher(int? id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];
            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page) && Session["ISADMIN"] != null && Convert.ToBoolean(Session["ISADMIN"].ToString()) == true)
            {
                objGV = new LP_GeneralVoucher_Property();
                objGVBLL = new LP_GeneralVoucher_BLL();
                fourthTier_Property objPropertyChildHead = new fourthTier_Property();
                fourthTier_BLL objLastBLL = new fourthTier_BLL(objPropertyChildHead);
                objGV.AccountLST = Helper.ConvertDataTable<fourthTier_Property>(objLastBLL.ViewAll());

                objSalesOrderVM_Property = new SalesOrderVM_Property();

                if (id > 0)
                {
                    LP_SalesOrder_Detail_Property objmSalesOrderdetail;
                    objSalesOrderProperty = new LP_SalesOrder_Master_Property();
                    objSalesOrderProperty.idx = Convert.ToInt16(id);

                    objSalesOrderBll = new LP_SalesOrder_BLL(objSalesOrderProperty);
                    DataTable dt = objSalesOrderBll.SelectOne();
                    objSalesOrderVM_Property.idx = Convert.ToInt16(dt.Rows[0]["salesorderIdx"].ToString());
                    objSalesOrderVM_Property.customerIdx = Convert.ToInt32(dt.Rows[0]["customerIdx"].ToString());
                    objSalesOrderVM_Property.soNumber = dt.Rows[0]["soNumber"].ToString();
                    objSalesOrderVM_Property.description = dt.Rows[0]["description"].ToString();
                    objSalesOrderVM_Property.qsIdx = Convert.ToInt16(dt.Rows[0]["qsIdx"].ToString());
                    objSalesOrderVM_Property.totalAmount = Convert.ToDecimal(dt.Rows[0]["totalAmount"].ToString());
                    objSalesOrderVM_Property.salesorderDate = (dt.Rows[0]["salesorderdate"].ToString()).ToString();
                    //string ndate = DateTime.Parse(pdate).ToString("yyyy-MM-dd");
                    //objSalesOrderVM_Property.salesorderDate = Convert.ToDateTime(ndate);// DateTime.Parse(dt.Rows[0]["mrnDate"].ToString()).ToString("yyyy-MM-dd");
                    ViewBag.DetailData = Helper.ConvertDataTable<SalesOrderVM_Property>(dt);
                    //update
                    return View("AddNewSalesOrder", objSalesOrderVM_Property);
                }
                else
                {
                    LP_GenerateTransNumber_Property objtrans = new LP_GenerateTransNumber_Property();
                    objtrans.TableName = "accountMasterGL";
                    objtrans.Identityfieldname = "idxx";
                    objtrans.userid = Session["UID"].ToString();
                    objGV.voucherNo = objGVBLL.GenerateSO(objtrans);
                    objGV.voucherDate = DateTime.Now.ToString("yyyy-MM-dd");
                    objSalesOrderVM_Property.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());

                    return View("GeneralVoucher", objGV);

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

        [HttpPost]
        public JsonResult AddUpdate(LP_GeneralVoucher_Property objGVProperty)

        {
            try
            {


                bool flag = false;
                objGV = new LP_GeneralVoucher_Property();
                objGVProperty.voucherNo = objGVProperty.voucherNo;
                objGVProperty.voucherDate = objGVProperty.voucherDate;
                objGVProperty.memo = objGVProperty.memo;
                objGVProperty.totalAmount = objGVProperty.totalAmount;
                objGVProperty.DetailData = Helper.ToDataTable<AccountGJ>(objGVProperty.AccountGJLST);
                if (objGV.idx > 0)
                {
                    objGVProperty.idx = objGV.idx;
                    objGVProperty.createDate = DateTime.Now;
                    objGVProperty.visible = 1;
                    objGVProperty.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());


                    objGVProperty.TableName = "accountGJ";
                    objGVBLL = new LP_GeneralVoucher_BLL(objGVProperty);
                    flag = objGVBLL.Insert();
                    //update
                }
                else
                {

                    //add
                    objGVProperty.idx = objGV.idx;
                    objGVProperty.createDate = DateTime.Now;
                    objGVProperty.visible = 1;
                    objGVProperty.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());
                    objGVProperty.TableName = "accountGJ";
                    objGVBLL = new LP_GeneralVoucher_BLL(objGVProperty);
                    flag = objGVBLL.Insert();

                }


                return Json(new { data = "", success = true, msg = true == true ? "Successfull" : "Failed", statuscode = true == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Opening Customer&Vendor
        public ActionResult opening()
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];
            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page) && Session["ISADMIN"] != null && Convert.ToBoolean(Session["ISADMIN"].ToString()) == true)
            {
                OpenigEntryVM objOpeningVM = new OpenigEntryVM();
                if (objOpeningVM.idx > 0)
                {

                }
                else
                {
                    LP_Opening_BLL objbll = new LP_Opening_BLL(objOpeningVM);
                    LP_GenerateTransNumber_Property objtrans = new LP_GenerateTransNumber_Property();
                    objtrans.TableName = "accountMasterGL";
                    objtrans.Identityfieldname = "idxx";
                    objtrans.userid = (Session["UID"].ToString());
                    objOpeningVM.voucherNo = objbll.GenerateSO(objtrans);
                    objOpeningVM.voucherDate = DateTime.Now.ToString("yyyy-MM-dd");
                    objOpeningVM.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());


                }

                return View("opening", objOpeningVM);
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
        public virtual JsonResult getCustomers()
        {
            fourthTier_Property objProperty = new fourthTier_Property();
            objProperty.idx = 1;
            fourthTier_BLL objbll = new fourthTier_BLL(objProperty);
            var Data = Helper.ConvertDataTable<fourthTier_Property>(objbll.getAlCustomersOrVendors());//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
            return Json(new { data = Data, success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual JsonResult getVendors()
        {
            fourthTier_Property objProperty = new fourthTier_Property();
            objProperty.idx = 2;
            fourthTier_BLL objbll = new fourthTier_BLL(objProperty);
            var Data = Helper.ConvertDataTable<fourthTier_Property>(objbll.getAlCustomersOrVendors());//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
            return Json(new { data = Data, success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddUpdateOP(OpenigEntryVM objVoucher)

        {
            try
            {

                bool flag = false;
                objOP = new OpenigEntryVM();
                objOP.voucherNo = objVoucher.voucherNo;
                objOP.voucherDate = objVoucher.voucherDate;
                objOP.openingType = objVoucher.openingType;
                objOP.accountIdx = objVoucher.accountIdx;
                objOP.totalAmount = objVoucher.totalAmount;
                objOP.Amount = objVoucher.Amount;
                objOP.paidAmount = objVoucher.paidAmount;
                objOP.balance = objVoucher.balance;
                objOP.memo = objVoucher.memo;
                objOP.createDate = DateTime.Parse(Convert.ToDateTime(objVoucher.voucherDate).ToString("yyyy-MM-dd"));
                objOP.createdByUserIdx = int.Parse(Session["UID"].ToString());
                if (objOP.idx > 0)
                {
                    //objSalesOrderProperty.idx = objSalesOrder.idx;
                    //objSalesOrderProperty.creationDate = DateTime.Now;
                    //objSalesOrderProperty.visible = 1;
                    //objSalesOrderProperty.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());
                    //objSalesOrderProperty.visible = 1;
                    //objSalesOrderProperty.status = "0";
                    //objSalesOrderProperty.TableName = "SalesOrderDetails";
                    objOPBLL = new LP_Opening_BLL(objOP);
                    flag = objOPBLL.Insert();
                    //update
                }
                else
                {

                    //add

                    objOPBLL = new LP_Opening_BLL(objOP);
                    flag = objOPBLL.Insert();
                    //var inventory = objSalesOrderBll.ProcessSalesInvoice();

                }




                return Json(new { data = "", success = true, msg = true == true ? "Successfull" : "Failed", statuscode = true == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}