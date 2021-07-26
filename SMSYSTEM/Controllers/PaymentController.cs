using Newtonsoft.Json;
using SSS.BLL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Setups.Accounts;
using SSS.Property.Transactions;
using SSS.Property.Transactions.ViewModels;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class PaymentController : BaseController
    {
        // GET: Payment

        LP_Voucher_ViewModel objvoucherVM;
        Payment_BLL objpaymentBll;
        payment_property objpaymentprop;
      
        public ActionResult Vouchers()
        {
            if (Session["LOGGEDIN"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public JsonResult GetAllVoucher()
        {
            try
            {
                objpaymentprop = new payment_property();
                objpaymentBll = new Payment_BLL(objpaymentprop);

                var Data = JsonConvert.SerializeObject(objpaymentBll.SelectAllReceiptVoucher());
                return Json(new { data = Data, success = true, statuscode = 200, count = Data.Length }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        #region Add payment
        public ActionResult AddVouchers(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                objvoucherVM = new LP_Voucher_ViewModel();
                if (objvoucherVM.idx > 0)
                {

                }
                else
                {
                    objvoucherVM.date_created = DateTime.Now;
                    //   objvoucherVM.vouchertypelist = Helper.ConvertDataTable<LP_Transaction_Type_Property>(GetAllTransactionType());
                    objvoucherVM.voucher_amount = 0.00m;
                    objvoucherVM.description = "";
                    objvoucherVM.BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetAllCompanyBanks());
                    objvoucherVM.Vendorlist = Helper.ConvertDataTable<Vendors_Property>(GetAllVendors());

                    LP_GenerateTransNumber_Property objtransnumber = new LP_GenerateTransNumber_Property();
                    objtransnumber.TableName = "accountMasterGL";
                    objtransnumber.Identityfieldname = "idxx";
                    objtransnumber.userid = Session["UID"].ToString();
                    objtransnumber.tranTypeIdx = "5";//for Receipt
                    objpaymentBll = new Payment_BLL();
                    objvoucherVM.voucher_no = objpaymentBll.GenerateTransNo(objtransnumber);

                }

                return View("_AddVoucher", objvoucherVM);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public JsonResult AddUpdate(LP_Voucher_ViewModel objVoucher)
        {
            try
            {
                bool flag = false;
                objpaymentprop = new payment_property();
                objpaymentprop.idx = objVoucher.idx;
                objpaymentprop.voucher_no = objVoucher.voucher_no;
                objpaymentprop.vendor_id = objVoucher.vendor_id;
                objpaymentprop.IncometaxPercent = objVoucher.IncometaxPercent;
                objpaymentprop.whSalesTaxAmount = objVoucher.whSalesTaxAmount;
                objpaymentprop.ITAmount = objVoucher.ITAmount;
                //objvouchermaster.voucher_type = objVoucher.voucher_type;
                objpaymentprop.date_created = objVoucher.date_created.ToString("yyyy-MM-dd");
                if (objVoucher.description != null)
                {
                    objpaymentprop.description = objVoucher.description;
                }
                else
                {
                    objpaymentprop.description = "";
                }
                // objMRNProperty.description = objMRN.description.Length>0? objMRN.description : "";

                //  objMRNProperty.paidDate = ;// objMRN.paidDate;


                if (objVoucher.idx > 0)
                {
                    ////objMRNProperty.creationDate = DateTime.Now;
                    ////objMRNProperty.visible = 1;
                    ////// objMRNProperty.status = "0";
                    ////objMRNProperty.createdByUserIdx = Convert.ToInt16(Session["UID"].ToString());
                    //objvouchermaster.creationDate = DateTime.Now;
                    //objvouchermaster.lastModificationDate = DateTime.Now;
                    //objvouchermaster.lastModifiedByUserIdx = Convert.ToInt16(Session["UID"].ToString());
                    ////  objMRNVM_Property.createdByUserIdx = DateTime.Now; ;
                    //objvouchermaster.TableName = "MRNDetails";
                    //objMRNBll = new LP_MRN_BLL(objvouchermaster);
                    //flag = objMRNBll.Insert();
                    //update
                }
                else
                {
                    //add

                    objpaymentprop.status = 0;
                    objpaymentprop.accorChequeNumber = objVoucher.accorChequeNumber;
                    objpaymentprop.bankIdx = objVoucher.bankIdx;
                    objpaymentprop.paymentModeIdx = objVoucher.paymentModeIdx;
                    objpaymentprop.voucher_amount = objVoucher.voucher_amount;
                    objpaymentprop.DetailData = Helper.ToDataTable<AccountGJ>(objVoucher.AccountGJLST);
                    objpaymentprop.u_id = Convert.ToInt16(Session["UID"].ToString());

                    objpaymentprop.TableName = "accountGJ";
                    objpaymentBll = new Payment_BLL(objpaymentprop);
                    flag = objpaymentBll.InsertPayment();

                }
                return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchSaleInvoice(LP_Voucher_ViewModel objsearchPI)
        {
            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    //DataTable Data = GetAllPIByDate(objsearchPI);
                  // var Data = Helper.ConvertDataTable<LP_SalesOrder_Master_Property>(GetAllSIByDate(objsearchPI));//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
                    return Json(new { data ="", success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);

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

        public JsonResult SearchPIInvoiceForVendors(int Id)
        {
            try
            {
                LP_PInvoice_BLL objbll = new LP_PInvoice_BLL();
                //DataTable tblFiltered;
                if (Id != null)
                {



                    var Data = Helper.ConvertDataTable<LP_P_Invoice_Property>(objbll.SelectPIByVendorId(Id));//JsonConvert.SerializeObject(GetAllPIByDate(objsearchPI));
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

        public JsonResult getvendorBalance(int id)
        {
            try
            {
                objvoucherVM = new LP_Voucher_ViewModel();
                Payment_BLL objBLL = new Payment_BLL();
                var Data = JsonConvert.SerializeObject(objBLL.getvendorInvoices(id));
                return Json(new { data = Data, success = true, statuscode = 200, count = Data.Length }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }






    }
}

#endregion