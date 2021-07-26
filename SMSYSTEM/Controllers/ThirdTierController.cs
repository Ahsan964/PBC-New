using Newtonsoft.Json;
using SSS.BLL.Setups.Accounts;
using SSS.Property.Setups;
using SSS.Property.Setups.Accounts;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class ThirdTierController : Controller
    {
        // GET: ThirdTier
        thirdTier_BLL objthirdTierbll;
        thirdTier_Property objthirdTierproperty;


        public ActionResult ViewthirdTiers()
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

        public JsonResult GetAllthirdTiers()
        {

            if (Session["LOGGEDIN"] != null)
            {
                try
                {


                    objthirdTierbll = new thirdTier_BLL(objthirdTierproperty);
                    var Data = JsonConvert.SerializeObject(objthirdTierbll.ViewAll());
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
        
        public ActionResult AddNew(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {
                objthirdTierproperty = new thirdTier_Property();
                //objBanlBLL = new Bank_BLL();
                //firstTier_Property headAccounts = new firstTier_Property() ;
                firstTier_BLL headAccountBLL = new firstTier_BLL();
                var data = headAccountBLL.ViewAll();
                var headOFAccounts = removeAssetsAndLiability(data);
                data.AcceptChanges();
                objthirdTierproperty.headLST = Helper.ConvertDataTable<firstTier_Property>(headOFAccounts);
                try
                {
                    if (id > 0)
                    {
                        objthirdTierproperty.idx = Convert.ToInt32(id);
                        objthirdTierbll = new thirdTier_BLL(objthirdTierproperty);
                        var dt = objthirdTierbll.GetthirdTierById();
                        objthirdTierproperty.headIdx = int.Parse(dt.Rows[0]["headIdx"].ToString());
                        objthirdTierproperty.subHeadName = dt.Rows[0]["subHeadName"].ToString();
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                }
                return PartialView("_AddNew", objthirdTierproperty);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult AddUpdate(thirdTier_Property objcomapnybank)
        {

            if (Session["LOGGEDIN"] != null)
            {
                try
                {
                    objthirdTierproperty = new thirdTier_Property();
                    objthirdTierproperty = objcomapnybank;
                    objthirdTierproperty.visible = 1;
                    objthirdTierproperty.createdByUserIdx = Convert.ToInt32(Session["UID"].ToString());
                    objthirdTierproperty.creationDate = DateTime.Now;
                    objthirdTierproperty.lastModifiedByUserIdx = Convert.ToInt32(Session["UID"].ToString());
                    objthirdTierproperty.lasModificationDate = DateTime.Now.ToString("yyyy-MM-dd");
                    objthirdTierbll = new thirdTier_BLL(objthirdTierproperty);
            
                        bool flag;
                        if (objcomapnybank.idx > 0)
                        {
                            //update
                            flag = objthirdTierbll.Update();
                            return Json(new { data = objcomapnybank, success = flag, msg = "Success", statuscode = 200, count = 0 }, JsonRequestBehavior.AllowGet);


                        }
                        else
                        {
                            //add
                            flag = objthirdTierbll.Insert();
                            return Json(new { data = objcomapnybank, success = flag, msg = "Success", statuscode = 200, count = 0 }, JsonRequestBehavior.AllowGet);

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

        public JsonResult Delete(int? id)
        {
            if (Session["LOGGEDIN"] != null)
            {

                try
                {
                    if (id > 0)
                    {

                       objthirdTierproperty = new thirdTier_Property();
                        objthirdTierproperty.idx = int.Parse(id.ToString());
                        //objDepartmentsBLL = new Departments_BLL(id);
                        objthirdTierbll = new thirdTier_BLL(objthirdTierproperty);

                        bool flag = objthirdTierbll.DeleteAccount();
                        return Json(new { data = "Deleted", success = flag, statuscode = 200 }, JsonRequestBehavior.AllowGet);



                    }
                    else
                    {
                        return Json(new { data = "Error Occur", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
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
        public DataTable removeAssetsAndLiability(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];
                if (dr["accountName"].ToString() == "Asset" || dr["accountName"].ToString() == "Liability")
                    dr.Delete();
            }
            dt.AcceptChanges();
            return dt;
        }
    }
}