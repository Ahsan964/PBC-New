using Newtonsoft.Json;
using SSS.BLL.Setups;
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
    public class GRNController : BaseController
    {
        LP_GRN_ViewModel objGRNVM_Property;
        LP_GRN_Master_Property objGRNProperty;
        LP_Purchase_BLL objpurchaseBll;
        LP_GRN_BLL objGRNBll;
        // GET: GRN
        public ActionResult ViewGRN()
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

        public ActionResult AddNewGRN(int? id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];
            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page) && Session["ISADMIN"] != null && Convert.ToBoolean(Session["ISADMIN"].ToString()) == true)
            {
                objGRNVM_Property = new LP_GRN_ViewModel();
                objpurchaseBll = new LP_Purchase_BLL();
                objGRNBll = new LP_GRN_BLL();
                objGRNVM_Property.POList = Helper.ConvertDataTable<LP_Purchase_Master_Property>(objpurchaseBll.SelectAllPOForGRN());
                objGRNVM_Property.Date_Created = DateTime.Now.ToString("yyyy-MM-dd");
                objGRNVM_Property.WareHouseList = Helper.ConvertDataTable<WareHouse_Property>(ViewWareHouses());
                //objGRNVM_Property.Doc_No = "GRN-001";
                if (id > 0)
                {
                    //update 
                    LP_GRN_Detail_Property objdetail;
                    objGRNProperty = new LP_GRN_Master_Property();
                    objGRNProperty.ID = Convert.ToInt16(id);

                    objGRNBll = new LP_GRN_BLL(objGRNProperty);
                    DataTable dt = objGRNBll.SelectOne();
                    objGRNVM_Property.ID = Convert.ToInt16(dt.Rows[0]["ID"].ToString());
                    //objGRNVM_Property.vendorIdx = Convert.ToInt16(dt.Rows[0]["vendorIdx"].ToString());

                    objGRNVM_Property.Doc_No = dt.Rows[0]["Doc_No"].ToString();
                    objGRNVM_Property.Narration = dt.Rows[0]["Narration"].ToString();
                    objGRNVM_Property.Parent_DocID = Convert.ToInt16(dt.Rows[0]["Parent_DocID"].ToString());
                    objGRNVM_Property.Total_Amount = Convert.ToDecimal(dt.Rows[0]["Total_Amount"].ToString());
                    //objPurchaseVM_Property.purchaseDate = DateTime.Parse(dt.Rows[0]["mrnDate"].ToString()).ToString("yyyy-MM-dd");
                    //foreach(DataRow dr in dt.Rows)
                    //{
                    //    objmrndetail

                    //}
                    objGRNVM_Property.GRNDeatilList = Helper.ConvertDataTable<LP_GRN_Detail_Property>(dt);
                    //ViewBag.DetailData = Helper.ConvertDataTable<LP_GRN_ViewModel>(dt);
                    //update
                    return View("_AddNewGRN", objGRNVM_Property);
                }
                else
                {
                    LP_GenerateTransNumber_Property objtrans = new LP_GenerateTransNumber_Property();
                    objtrans.TableName = "GRN_Master";
                    objtrans.Identityfieldname = "ID";
                    objtrans.userid = Session["UID"].ToString();
                    objGRNVM_Property.Doc_No = objGRNBll.GenerateMRNNo(objtrans);
                   
                    return View("_AddNewGRN", objGRNVM_Property);

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
        public JsonResult AddUpdate(LP_GRN_ViewModel objGRN)
        {
            try
            {
                bool flag = false;
                objGRNProperty = new LP_GRN_Master_Property();
                objGRNProperty.Doc_No = objGRN.Doc_No;
                objGRNProperty.Parent_DocID = objGRN.Parent_DocID;
                
                objGRNProperty.Date_Created = DateTime.Parse(objGRN.Date_Created);
                objGRNProperty.Narration = objGRN.Narration;
                objGRNProperty.Total_Amount = objGRN.Total_Amount;
                objGRNProperty.User_ID =Convert.ToInt16(Session["UID"].ToString());
                //  objGRNProperty.paidDate = ;// objGRN.paidDate;

                objGRNProperty.DetailData = Helper.ToDataTable<LP_GRN_Detail_Property>(objGRN.GRNDeatilList);
                if (objGRN.ID > 0)
                {
                    //update
                    objGRNProperty.ID = objGRN.ID;
                    objGRNProperty.Date_Created = DateTime.Now;
                    objGRNProperty.TableName = "GRN_Detail";
                    objGRNProperty.Status = "Active";
                    objGRNProperty.BRANCHID = Convert.ToInt16(Session["BRANCHID"].ToString());
                    objGRNProperty.User_ID = Convert.ToInt16(Session["UID"].ToString());
                    objGRNBll = new LP_GRN_BLL(objGRNProperty);
                    flag = objGRNBll.Insert();
                }
                else
                {
                    //add
                    objGRNProperty.Date_Created = DateTime.Now;
                    objGRNProperty.Status = "Active";
                    objGRNProperty.BRANCHID= Convert.ToInt16(Session["BRANCHID"].ToString()); 
                    objGRNProperty.WarerHouseID = objGRN.WarerHouseID;
                    objGRNProperty.User_ID = Convert.ToInt16(Session["UID"].ToString());
                   // objGRNProperty.visible = objGRN.visible;
                    objGRNProperty.TableName = "GRN_Detail";
                    objGRNBll = new LP_GRN_BLL(objGRNProperty);
                    flag = objGRNBll.Insert();

                }
                 GenerateReport(objGRNProperty.ID, 5, "GRNReport");
                return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message, success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        // Delete
        public JsonResult Delete(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    objGRNProperty = new LP_GRN_Master_Property();
                    objGRNProperty.ID = Convert.ToInt16(id);

                    objGRNBll = new LP_GRN_BLL(objGRNProperty);
                    var flag1 = objGRNBll.Delete();
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
        public JsonResult GetAllGRN()
        {

            if (Session["LOGGEDIN"] != null)
            {
                try
                {


                    objGRNBll = new LP_GRN_BLL();
                    var Data = JsonConvert.SerializeObject(objGRNBll.SelectAll());
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

        public JsonResult ProcessGRN(int id)
        {

            if (Session["LOGGEDIN"] != null)
            {
                bool flag=false;
                try
                {

                    objGRNProperty = new LP_GRN_Master_Property();
                    objGRNProperty.ID=id;
                    objGRNBll = new LP_GRN_BLL(objGRNProperty);
                    objGRNProperty.DetailData = objGRNBll.SelectOne();
                    flag = objGRNBll.ProcessGRN();
                    return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    return Json(new { data = "", success = flag, msg = flag == true ? "Successfull" : "Failed", statuscode = flag == true ? 200 : 401 }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { data = "Session Expired", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}