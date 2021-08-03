using Newtonsoft.Json;
using SSS.BLL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using SSS.Property.Transactions.ViewModels;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class PInvoiceController : BaseController
    {
        // GET: PInvoice
        LP_PI_ViewModel objPInvoiceVM;
        LP_P_Invoice_Property objPIProperty;
        LP_PInvoice_BLL objPIBLL;
        LP_Purchase_BLL objPurchaseBLL;
        LP_GRN_BLL objGRNBLL;

        public ActionResult ViewPI()
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

        public ActionResult AddNewPI(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                objPInvoiceVM = new LP_PI_ViewModel();
                objPIBLL = new LP_PInvoice_BLL();
                objPurchaseBLL = new LP_Purchase_BLL();

                objPInvoiceVM.POLIST = Helper.ConvertDataTable<LP_Purchase_Master_Property>(objPurchaseBLL.SelectAllProcessedGRN());
                objPInvoiceVM.TaxesList = Helper.ConvertDataTable<Taxes_Property>(GetAllTaxes());
                ViewBag.PITAXLIST = objPInvoiceVM.TaxesList;
                objPInvoiceVM.VendorList = Helper.ConvertDataTable<Vendors_Property>(GetAllVendors());
                objPInvoiceVM.ProductList = Helper.ConvertDataTable<Product_Property>(ViewAllProducts());
                objPInvoiceVM.BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetAllCompanyBanks());
                objPInvoiceVM.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd");
                objPInvoiceVM.WareHouseList = Helper.ConvertDataTable<WareHouse_Property>(ViewWareHouses());
                //objGRNVM_Property.Doc_No = "GRN-001";
                if (id > 0)
                {
                    //update 
                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.idx = Convert.ToInt16(id);
                    objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                    DataSet DS = objPIBLL.SelectByID();
                    objPInvoiceVM.InvoiceProperty = Helper.ConvertDataTable<LP_P_Invoice_Property>(DS.Tables[0]);
                    objPInvoiceVM.InvoiceDetails = Helper.ConvertDataTable<LP_P_Invoice_Details>(DS.Tables[1]);
                    objPInvoiceVM.PITAXLIST = Helper.ConvertDataTable<LP_PI_Taxes_Property>(DS.Tables[2]);
                    objPInvoiceVM.idx = objPIProperty.idx;
                    //
                    objPInvoiceVM.ParentDocID = objPInvoiceVM.InvoiceProperty[0].ParentDocID;
                    objPInvoiceVM.InvoiceNo = objPInvoiceVM.InvoiceProperty[0].InvoiceNo;
                    objPInvoiceVM.VendorID = objPInvoiceVM.InvoiceProperty[0].VendorID;
                    //objPInvoiceVM.CreatedDate = objPInvoiceVM.InvoiceProperty[0].CreatedDate;
                    objPInvoiceVM.Reference = objPInvoiceVM.InvoiceProperty[0].Reference;
                    objPInvoiceVM.Description = objPInvoiceVM.InvoiceProperty[0].Description;
                    objPInvoiceVM.NetAmount = objPInvoiceVM.InvoiceProperty[0].NetAmount;
                    objPInvoiceVM.TotalAmount = objPInvoiceVM.InvoiceProperty[0].TotalAmount;
                    objPInvoiceVM.TaxAmount = objPInvoiceVM.InvoiceProperty[0].TaxAmount;
                    objPInvoiceVM.BalanceAmount = objPInvoiceVM.InvoiceProperty[0].BalanceAmount;
                    objPInvoiceVM.PaidAmount = objPInvoiceVM.InvoiceProperty[0].PaidAmount;
                    objPInvoiceVM.PaymentType = objPInvoiceVM.InvoiceProperty[0].PaymentType;
                    objPInvoiceVM.bankIdx = objPInvoiceVM.InvoiceProperty[0].BankId;
                    objPInvoiceVM.accorChequeNumber = objPInvoiceVM.InvoiceProperty[0].AccountChequeNo;
                    ViewBag.update = true;

                    return PartialView("_AddNewPI", objPInvoiceVM);
                }
                else
                {
                    objPInvoiceVM.InvoiceProperty = Helper.ConvertDataTable<LP_P_Invoice_Property>(new DataTable());
                    objPInvoiceVM.InvoiceDetails = Helper.ConvertDataTable<LP_P_Invoice_Details>(new DataTable());
                    objPInvoiceVM.PITAXLIST = Helper.ConvertDataTable<LP_PI_Taxes_Property>(new DataTable());
                    LP_GenerateTransNumber_Property objtrans = new LP_GenerateTransNumber_Property();
                    objtrans.TableName = "P_Invoice";
                    objtrans.Identityfieldname = "idx";
                    objtrans.userid = Session["UID"].ToString();
                    objPInvoiceVM.InvoiceNo = objPIBLL.GeneratePINo(objtrans);
                    
                    //objPInvoiceVM.CreatedDate =DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));
                    return PartialView("_AddNewPI", objPInvoiceVM);

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpPost]
        public JsonResult AddUpdate(LP_PI_ViewModel objPI)
         {
            try
            {
                bool flag = false;

                if (objPI.idx > 0)
                {
                    //update
                    objPInvoiceVM = new LP_PI_ViewModel();

                    var BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetCompanyBankById(objPI.bankIdx));
                    var VendorsData = Helper.ConvertDataTable<Vendors_Property>(GetVendorById(objPI.VendorID));


                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.InvoiceNo = objPI.InvoiceNo;
                    objPIProperty.InvoiceType = objPI.InvoiceType;
                    if (objPI.TotalAmount == objPI.PaidAmount)
                    {
                        objPIProperty.IsPaid = true;
                    }
                    else
                    {
                        objPIProperty.IsPaid = false;
                    }
                    //objPIProperty.IsPaid = objPI.IsPaid;
                    objPIProperty.NetAmount = objPI.NetAmount;
                    objPIProperty.PaidAmount = objPI.PaidAmount;
                    objPIProperty.BalanceAmount = objPI.BalanceAmount;

                    objPIProperty.PaymentType = objPI.PaymentType;
                    objPIProperty.Status = objPI.Status;
                    objPIProperty.Taxable = objPI.Taxable;
                    objPIProperty.TotalAmount = objPI.TotalAmount;
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.CreatedBy = Convert.ToInt16(Session["UID"].ToString());
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.Reference = objPI.Reference;
                    objPIProperty.BankId = objPI.bankIdx;
                    objPIProperty.AccountChequeNo = objPI.accorChequeNumber;
                    objPIProperty.CreatedDate = DateTime.Now;
                    objPIProperty.Status = 0;
                    objPIProperty.WarerHouseID = objPI.WarerHouseID;
                    objPIProperty.idx = objPI.idx;

                    if (objPI.Description == null)
                    {
                        objPIProperty.Description = "";
                    }
                    else
                    {
                        objPIProperty.Description = objPI.Description;
                    }
                   
                    objPIProperty.VendorID = objPI.VendorID;
                    objPIProperty.ParentDocID = objPI.ParentDocID;
                    //tax
                    if (objPI.PITAXLIST != null)
                    {
                        objPIProperty.TaxData = Helper.ToDataTable<LP_PI_Taxes_Property>(objPI.PITAXLIST);
                    }
              
                    if (BankList.Count > 0)
                    {
                        objPIProperty.CoaIDx = Convert.ToInt32(BankList[0].coaidx);
                    }

                    objPIProperty.VendorCoaidx = Convert.ToInt32(VendorsData[0].coaIdx);

                    objPIProperty.InvoiceDetails = Helper.ToDataTable<LP_P_Invoice_Details>(objPI.InvoiceDetails);

                    objPIProperty.visible = 1;

                    // Added By Ahsan 6/11/2021
                    if (objPIProperty.ParentDocID > 0)
                    {

                        objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                        flag = objPIBLL.Insert();

                    }
                    else
                    {

                        objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                        flag = objPIBLL.InsertDP();

                    }
                }
                else
                {
                    //add
                    objPInvoiceVM = new LP_PI_ViewModel();

                    var BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetCompanyBankById(objPI.bankIdx));
                    var VendorsData = Helper.ConvertDataTable<Vendors_Property>(GetVendorById(objPI.VendorID));


                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.InvoiceNo = objPI.InvoiceNo;
                    objPIProperty.InvoiceType = objPI.InvoiceType;
                    if (objPI.TotalAmount == objPI.PaidAmount)
                    {
                        objPIProperty.IsPaid = true;
                    }
                    else
                    {
                        objPIProperty.IsPaid = false;
                    }
                    //objPIProperty.IsPaid = objPI.IsPaid;
                    objPIProperty.NetAmount = objPI.NetAmount;
                    objPIProperty.PaidAmount = objPI.PaidAmount;
                    objPIProperty.BalanceAmount = objPI.BalanceAmount;

                    objPIProperty.PaymentType = objPI.PaymentType;
                    objPIProperty.Status = objPI.Status;
                    objPIProperty.Taxable = objPI.Taxable;
                    objPIProperty.TotalAmount = objPI.TotalAmount;
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.CreatedBy = Convert.ToInt16(Session["UID"].ToString());
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.Reference = objPI.Reference;
                    objPIProperty.BankId = objPI.bankIdx;
                    objPIProperty.AccountChequeNo = objPI.accorChequeNumber;
                    objPIProperty.CreatedDate = DateTime.Now;
                    objPIProperty.Status = 0;
                    objPIProperty.WarerHouseID = objPI.WarerHouseID;
                    if (objPI.Description == null)
                    {
                        objPIProperty.Description = "";
                    }
                    else
                    {
                        objPIProperty.Description = objPI.Description;
                    }
                    //if (objPI.PITAXLIST.Count() == null)
                    //{
                    //    objPIProperty.Taxable = false;
                    //}
                    //else
                    //{
                    //    objPIProperty.Taxable = true;
                    //}
                    //objPIProperty.Taxable= ((objPI.TaxesList.Count()>0)==true?true:false);
                    objPIProperty.VendorID = objPI.VendorID;
                    objPIProperty.ParentDocID = objPI.ParentDocID;
                    //tax
                    if (objPI.PITAXLIST != null)
                    {
                        objPIProperty.TaxData = Helper.ToDataTable<LP_PI_Taxes_Property>(objPI.PITAXLIST);
                    }
                    //if (objPIProperty.PITAXLIST != null)
                    //{
                    //    objPIProperty.TaxData = Helper.ToDataTable<LP_PI_Taxes_Property>(objPI.PITAXLIST);
                    //}
                    if (BankList.Count > 0)
                    {
                        objPIProperty.CoaIDx = Convert.ToInt32(BankList[0].coaidx);
                    }
                    
                    objPIProperty.VendorCoaidx = Convert.ToInt32(VendorsData[0].coaIdx);

                    objPIProperty.InvoiceDetails = Helper.ToDataTable<LP_P_Invoice_Details>(objPI.InvoiceDetails);
                    
                    // Added By Ahsan 6/11/2021
                    objPIProperty.visible = 1;
                    if (objPIProperty.ParentDocID > 0)
                    {

                        objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                        flag = objPIBLL.Insert();

                    }
                    else
                    {
                       
                        objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                        flag = objPIBLL.InsertDP();

                    }

                }
                return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.InnerException, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllPI()
        {

            if (Session["LOGGEDIN"] != null)
            {
                try
                {


                    objPIBLL = new LP_PInvoice_BLL();
                    var Data = JsonConvert.SerializeObject(objPIBLL.SelectAll());
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

        public JsonResult SelectGRNById(int id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    LP_GRN_Master_Property objPurchaseProperty = new LP_GRN_Master_Property();
                    objPurchaseProperty.ID = id;

                    objGRNBLL = new LP_GRN_BLL(objPurchaseProperty);
                    var Data = JsonConvert.SerializeObject(objGRNBLL.SelectOne());
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

        public JsonResult SelectPOById(int id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    LP_GRN_Master_Property objPurchaseProperty = new LP_GRN_Master_Property();
                    objPurchaseProperty.ID = id;

                    objGRNBLL = new LP_GRN_BLL(objPurchaseProperty);

                    LP_Purchase_Master_Property objpurchase = new LP_Purchase_Master_Property();
                    objpurchase.idx = id;
                    objPurchaseBLL = new LP_Purchase_BLL(objpurchase);
                    var Data = JsonConvert.SerializeObject(objPurchaseBLL.SelectOne());
                    //var Data = JsonConvert.SerializeObject(objGRNBLL.SelectOne());
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

        // Delete
        public JsonResult Delete(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.idx = Convert.ToInt16(id);

                    objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                    var flag1 = objPIBLL.Delete();
                    if (flag1)
                    {
                        return Json(new { data = "Deleted", success = flag1, statuscode = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = "Error", success = flag1, statuscode = 200 }, JsonRequestBehavior.DenyGet);
                    }

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


        #region PurchaseReturn

        [HttpGet]
        public JsonResult CheckInverntoryforProductStock(int? id)
        {

            var Data = JsonConvert.SerializeObject(CheckInverntoryforProductStock(id));

            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);






        }

        public ActionResult PurchaseReturn()
        {
            LP_PurchaseReturnVM_Property obj = new LP_PurchaseReturnVM_Property();
            LP_PInvoice_BLL objPIBLL = new LP_PInvoice_BLL();
            obj.VendorLST = Helper.ConvertDataTable<Vendors_Property>(GetAllVendors());
            obj.PurchaseLST = Helper.ConvertDataTable<LP_P_Invoice_Property>(objPIBLL.SelectAll());
            return View(obj);
        }


        [HttpPost]
        public JsonResult SearchPurchase(LP_PurchaseReturnVM_Property objsale)
        {


            int salesIdx = int.Parse(objsale.vendorIdx.ToString());

            var Data = JsonConvert.SerializeObject(GetAllSalesDetailsByIdx(salesIdx));

            return Json(new { data = Data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnPurchase(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                objPInvoiceVM = new LP_PI_ViewModel();
                objPIProperty = new LP_P_Invoice_Property();
                objPIProperty.idx = Convert.ToInt32(id);
                objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                objPInvoiceVM.TaxesList = Helper.ConvertDataTable<Taxes_Property>(GetAllTaxes());
                objPInvoiceVM.ProductList = Helper.ConvertDataTable<Product_Property>(ViewAllProducts());
                objPInvoiceVM.BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetAllCompanyBanks());
                DataSet DS = objPIBLL.SelectPIWithDetailData(objPIProperty.idx);
                if (DS.Tables.Count > 0)
                {
                    ViewBag.isReturn = 1;
                    objPInvoiceVM.idx = Convert.ToInt32(id);
                    objPInvoiceVM.InvoiceDetails = Helper.ConvertDataTable<LP_P_Invoice_Details>(DS.Tables[0]);
                    if (DS.Tables[1].Rows.Count > 0)
                    {
                        var data = objPIBLL.SelectByID();
                        var masterData = data.Tables[0];
                        var purchaseTaxData = DS.Tables[1];
                        decimal netAmount, totalAmount, taxAmount, paidAmount, balanceAmount;
                        decimal.TryParse(masterData.Rows[0]["NetAmount"].ToString(), out netAmount);
                        decimal.TryParse(masterData.Rows[0]["TotalAmount"].ToString(), out totalAmount);
                        decimal.TryParse(masterData.Rows[0]["PaidAmount"].ToString(), out paidAmount);
                        decimal.TryParse(masterData.Rows[0]["BalanceAmount"].ToString(), out balanceAmount);
                        if (masterData.Rows[0]["Taxable"].ToString() == "True")
                        {
                            decimal.TryParse(purchaseTaxData.Compute("Sum(taxPercent)", "").ToString(), out taxAmount);
                            objPInvoiceVM.TaxAmount = (((netAmount) / 100) * taxAmount);
                        }
                        objPInvoiceVM.NetAmount = netAmount;
                        objPInvoiceVM.TotalAmount = totalAmount;
                        objPInvoiceVM.PaidAmount = paidAmount;
                        objPInvoiceVM.BalanceAmount = balanceAmount;

                        objPInvoiceVM.PITAXLIST = Helper.ConvertDataTable<LP_PI_Taxes_Property>(DS.Tables[1]);
                    }

                }

                return View("_AddNewPI", objPInvoiceVM);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        [HttpPost]
        public JsonResult SalesReturnEntry(LP_PI_ViewModel objPI)
        {
            try
            {
                bool flag = false;
                decimal previousTotalAmount,previousNetAmount,previousPaidAmount,previousBalanceAmount,totaltaxPercent,netAmountAfterReturn,taxAmountAfterReturn,totalReturnAmountInclusiveTax,taxAmountOnReturn=0.00m;
                if (objPI.idx > 0)
                {
                    
                   // var BankList = Helper.ConvertDataTable<Company_Bank_Property>(GetCompanyBankById(objPI.bankIdx));
                  
                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.idx = Convert.ToInt32(objPI.idx);
                    objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                    var data = objPIBLL.SelectByID();
                    var vendorIdx = int.Parse(data.Tables[0].Rows[0]["VendorID"].ToString());
                    var VendorsData = Helper.ConvertDataTable<Vendors_Property>(GetVendorById(vendorIdx));

                    var list = objPI.InvoiceDetails;
                    previousNetAmount = decimal.Parse(data.Tables[0].Rows[0]["NetAmount"].ToString());
                    previousTotalAmount = decimal.Parse(data.Tables[0].Rows[0]["TotalAmount"].ToString());
                    previousPaidAmount = decimal.Parse(data.Tables[0].Rows[0]["PaidAmount"].ToString());
                    previousBalanceAmount = decimal.Parse(data.Tables[0].Rows[0]["BalanceAmount"].ToString());
                    decimal totalReturnAmount = decimal.Parse(list.Sum(x => x.ReturnAmount).ToString());
                    netAmountAfterReturn = previousNetAmount - totalReturnAmount;
                    totalReturnAmountInclusiveTax = totalReturnAmount;//if No tax
                    
                    if (totalReturnAmount > 0)
                    {
                        LP_PIReturn_Property objReturn = new LP_PIReturn_Property();
                        LP_PIReturn_BLL objReturnBLL = new LP_PIReturn_BLL(objReturn);
                        objReturn.piIdx = objPI.idx;
                        objReturn.invoiceNo = data.Tables[0].Rows[0]["InvoiceNo"].ToString();
                        objReturn.returnAmount = totalReturnAmount;
                        objReturn.createdBy = Convert.ToInt16(Session["UID"].ToString());
                       objReturn.VendorID = int.Parse(data.Tables[0].Rows[0]["VendorID"].ToString());
                        objReturn.DetailData = Helper.ToDataTable<LP_P_Invoice_Details>(objPI.InvoiceDetails.Where(x => x.Qty > 0 && x.ReturnQty > 0).ToList());
                        objReturn.UpdatedDetailData = Helper.ToDataTable<LP_P_Invoice_Details>(objPI.UpdatedInvoiceDetails.ToList());
                        objReturn.coaVendorIdx = VendorsData[0].coaIdx;
                        
                        objReturn.PaidAmount = previousPaidAmount;
                        objReturn.BalancAmount = previousBalanceAmount;
                        objReturn.NetAmount = previousNetAmount;
                        objReturn.TotalAmount = previousTotalAmount;
                        //Account and Tax Updation After Return
                        var taxData = data.Tables[2];
                        if (taxData.Rows.Count > 0)
                        {

                            object sumObject;
                            sumObject = taxData.Compute("Sum(taxPercent)", string.Empty);
                            totaltaxPercent = decimal.Parse(sumObject.ToString());
                            taxAmountAfterReturn = ((netAmountAfterReturn) / 100) * totaltaxPercent;
                            taxAmountOnReturn = ((totalReturnAmount) / 100) * totaltaxPercent;//Return Ka Tax Amount Hai
                            objReturn.UpdatedTaxAmount = taxAmountAfterReturn;                            
                            totalReturnAmountInclusiveTax = totalReturnAmount + taxAmountOnReturn;
                            objReturn.taxData = taxData;

                        }
                       
                        else
                        {
                            objReturn.UpdatedTaxAmount = 0.00m;
                        }
                        objReturn.returnAmountInclusiveTax = totalReturnAmountInclusiveTax;
                        if (totalReturnAmountInclusiveTax <= previousBalanceAmount)
                        {
                            objReturn.UpdatedTotalAmount = netAmountAfterReturn + objReturn.UpdatedTaxAmount;
                            objReturn.UpdatedNetAmount = netAmountAfterReturn;
                            //objReturn.UpdatedBalanceAmount = previousBalanceAmount - totalReturnAmount;
                            objReturn.UpdatedBalanceAmount = previousBalanceAmount - totalReturnAmountInclusiveTax;
                            objReturn.UpdatedPaidAmount = previousPaidAmount;
                            objReturn.amountTobeReceived = 0.00m;
                            if (totalReturnAmount < previousBalanceAmount)
                            {
                                //only balance Amount will be updated means only payable will decrease only
                                //isPaid Check will remain zero
                             
                                objReturn.isPaid = 0;


                            }
                            else
                            {
                                //only balance Amount will be updated means only payable will decrease only
                                //isPaid Check will remain one
                            
                                objReturn.isPaid = 1;
                            }
                        }
                        else if (totalReturnAmountInclusiveTax > previousBalanceAmount)
                        {
                            if (totalReturnAmountInclusiveTax == previousTotalAmount)
                            {
                                objReturn.UpdatedTotalAmount = 0;
                                objReturn.UpdatedNetAmount = 0;
                                objReturn.UpdatedBalanceAmount = 0;
                                objReturn.UpdatedPaidAmount = 0;
                                decimal Amount = previousBalanceAmount - totalReturnAmountInclusiveTax;
                                objReturn.UpdatedPaidAmount = previousPaidAmount + Amount;
                                objReturn.amountTobeReceived = -(Amount);
                                objReturn.isPaid = 1;
                            }
                            else
                            {
                                objReturn.UpdatedTotalAmount = netAmountAfterReturn + objReturn.UpdatedTaxAmount;
                                objReturn.UpdatedNetAmount = netAmountAfterReturn;
                                objReturn.UpdatedBalanceAmount = 0;
                                decimal Amount = previousBalanceAmount - totalReturnAmountInclusiveTax;
                                objReturn.UpdatedPaidAmount = previousPaidAmount+ Amount;
                                objReturn.amountTobeReceived = -(Amount);
                                objReturn.isPaid = 1;
                            }
                        }
                        objReturnBLL.Insert();
                    }



                }
                else
                {
                    //add
                    objPIProperty = new LP_P_Invoice_Property();
                    objPIProperty.InvoiceNo = objPI.InvoiceNo;
                    objPIProperty.InvoiceType = objPI.InvoiceType;
                    objPIProperty.IsPaid = objPI.IsPaid;
                    objPIProperty.NetAmount = objPI.NetAmount;
                    objPIProperty.PaidAmount = objPI.PaidAmount;
                    objPIProperty.BalanceAmount = objPI.BalanceAmount;

                    objPIProperty.PaymentType = objPI.PaymentType;
                    objPIProperty.Status = objPI.Status;
                    objPIProperty.Taxable = objPI.Taxable;
                    objPIProperty.TotalAmount = objPI.TotalAmount;
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.CreatedBy = Convert.ToInt16(Session["UID"].ToString());
                    objPIProperty.Description = objPI.Description;
                    objPIProperty.Reference = objPI.Reference;
                    objPIProperty.BankId = objPI.bankIdx;
                    objPIProperty.AccountChequeNo = objPI.accorChequeNumber;
                    objPIProperty.CreatedDate = DateTime.Now;
                    objPIProperty.Status = 0;
                    if (objPI.Description == null)
                    {
                        objPIProperty.Description = "";
                    }
                    else
                    {
                        objPIProperty.Description = objPI.Description;
                    }
                    if (objPI.PITAXLIST.Count() > 0)
                    {
                        objPIProperty.Taxable = true;
                    }
                    else
                    {
                        objPIProperty.Taxable = false;
                    }
                    //objPIProperty.Taxable= ((objPI.TaxesList.Count()>0)==true?true:false);
                    objPIProperty.VendorID = objPI.VendorID;
                    objPIProperty.ParentDocID = objPI.ParentDocID;
                    //tax
                    objPIProperty.TaxData = Helper.ToDataTable<LP_PI_Taxes_Property>(objPI.PITAXLIST);
                    objPIProperty.InvoiceDetails = Helper.ToDataTable<LP_P_Invoice_Details>(objPI.InvoiceDetails);

                    objPIBLL = new LP_PInvoice_BLL(objPIProperty);
                    flag = objPIBLL.Insert();

                }
                return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.InnerException, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}