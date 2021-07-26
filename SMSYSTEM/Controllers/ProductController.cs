using Newtonsoft.Json;
using SSS.BLL.Setups;
using SSS.Property.Setups;
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
    public class ProductController : BaseController
    {
        // GET: Product
        Product_BLL objProductBLL;
        Product_Property objProductProperty;
        public ActionResult ViewProduct()
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

        public JsonResult GetAllProducts()
        {
            if (Session["LOGGEDIN"] != null)
            {

                try
                {
                    objProductProperty = new Product_Property();
                    //objProductProperty.branchIdx = 1;//user logged in session branchIdx
                    objProductBLL = new Product_BLL(objProductProperty);
                    var Data = JsonConvert.SerializeObject(objProductBLL.ViewAll());
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

        public ActionResult AddNewProduct(int? id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string pagename = @"/" + controllerName + @"/" + actionName;
            var page = (List<LP_Pages_Property>)Session["PageList"];

            if (Session["LoggedIn"] != null && Helper.CheckPageAccess(pagename, page))
            {
                
                objProductProperty = new Product_Property();
                objProductProperty.idx = Convert.ToInt32(id);
                //objProductProperty.branchIdx = 1;//It will have the value of session branchIdx
                objProductBLL = new Product_BLL(objProductProperty);
                DataTable categories = objProductBLL.ddlCategory();
                List<Product_Category_Property> catLST = new List<Product_Category_Property>();
                DataTable subCategories = objProductBLL.ddlSubCategory();
                List<Product_SubCategory_Property> subCatLST = new List<Product_SubCategory_Property>();
                DataTable productTypes = objProductBLL.ddlProductType();
                List<Product_Type_Property> productTypeLST = new List<Product_Type_Property>();
                DataTable units = objProductBLL.ddlUnit();
                List<itemUnit_Property> unitLST = new List<itemUnit_Property>();
                foreach (DataRow dr in categories.Rows)
                {
                    Product_Category_Property objProductCat = new Product_Category_Property();
                    objProductCat.Category = dr["category"].ToString();
                    objProductCat.idx = Convert.ToInt32(dr["idx"].ToString());
                    catLST.Add(objProductCat);
                }
                ViewBag.catLST = catLST;
                foreach (DataRow dr in subCategories.Rows)
                {
                    Product_SubCategory_Property objProductCat = new Product_SubCategory_Property();
                    objProductCat.subCategory = dr["subCategory"].ToString();
                    objProductCat.idx = Convert.ToInt32(dr["idx"].ToString());
                    subCatLST.Add(objProductCat);
                }
                ViewBag.subCatLST = subCatLST;
                foreach (DataRow dr in productTypes.Rows)
                {
                    Product_Type_Property objProductCat = new Product_Type_Property();
                    objProductCat.productType = dr["productType"].ToString();
                    objProductCat.idx = Convert.ToInt32(dr["idx"].ToString());
                    productTypeLST.Add(objProductCat);
                }
                ViewBag.productTypeLST = productTypeLST;
                foreach (DataRow dr in units.Rows)
                {
                    itemUnit_Property objProductCat = new itemUnit_Property();
                    objProductCat.itemUnit = dr["itemUnit"].ToString();
                    objProductCat.idx = Convert.ToInt32(dr["idx"].ToString());
                    unitLST.Add(objProductCat);
                }
                ViewBag.unitLST = unitLST;
                
                objProductProperty.ProductLST = Helper.ConvertDataTable<Product_Property>(objProductBLL.ViewAll());
                if (id != null && id != 0)
                {
                    decimal cp, sp, pt;
                    var dt = objProductBLL.GetById(id);
                    //objProductProperty.companyIdx = 1;
                    objProductProperty.idx = int.Parse(dt.Rows[0]["idx"].ToString());
                    objProductProperty.productTypeIdx = int.Parse(dt.Rows[0]["productTypeIdx"].ToString());
                    objProductProperty.productCatIdx = int.Parse(dt.Rows[0]["productCatIdx"].ToString());
                    objProductProperty.productSubCatIdx = int.Parse(dt.Rows[0]["productSubCatIdx"].ToString());
                    objProductProperty.unitIdx = int.Parse(dt.Rows[0]["unitIdx"].ToString());
                    objProductProperty.HSCODE = (dt.Rows[0]["HSCODE"].ToString());
                    objProductProperty.itemCode = (dt.Rows[0]["itemCode"].ToString());
                    objProductProperty.itemName = (dt.Rows[0]["itemName"].ToString());
                    objProductProperty.description = (dt.Rows[0]["description"].ToString());
                    //objProductProperty.costPrice = 0.00m;
                    string costPrice = (dt.Rows[0]["costPrice"].ToString());
                    decimal.TryParse(costPrice, out cp);                    
                    objProductProperty.costPrice = cp;
                    string salePrice = (dt.Rows[0]["salePrice"].ToString());
                    decimal.TryParse(salePrice, out sp);
                    objProductProperty.salePrice = sp;                    
                    string productTax = (dt.Rows[0]["productTax"].ToString());
                    decimal.TryParse(productTax, out pt);
                    objProductProperty.productTax = pt;
                    //objProductProperty.product_catIdx = int.Parse(dt.Rows[0]["product_catIdx"].ToString());
                    //objProductProperty.subCategory = dt.Rows[0]["subCategory"].ToString();
                    //objProductProperty.HS_CodeSub = dt.Rows[0]["HS_CodeSub"].ToString();
                    ////objProductProperty.firstName = dt.Rows[0]["firstName"].ToString();
                    //objProductProperty.lastName = dt.Rows[0]["lastName"].ToString();
                    //objProductProperty.CNIC = (dt.Rows[0]["CNIC"].ToString());
                    //objProductProperty.cellNumber = (dt.Rows[0]["cellNumber"].ToString());
                    //objProductProperty.loginId = (dt.Rows[0]["loginId"].ToString());
                    //objProductProperty.password = dt.Rows[0]["password"].ToString();
                }
                else
                {
                    LP_GenerateTransNumber_Property objtrans = new LP_GenerateTransNumber_Property();
                    objtrans.TableName = "products";
                    objtrans.Identityfieldname = "idx";
                    objtrans.userid = Session["UID"].ToString();
                    objProductProperty.itemCode = objProductBLL.GenerateProductCode(objtrans);
                }


                return PartialView("_AddNewProduct", objProductProperty);
            }
            else
            {
                
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public JsonResult AddUpdate(Product_Property objProductCategory)
        {
            if (Session["LOGGEDIN"] != null)
            {

                try
                {   if (objProductCategory.productTypeIdx == 2)

                    {
                        objProductCategory.DetailData = Helper.ToDataTable<LP_ProductDetailsForCombo_Property>(objProductCategory.ProductDetailLST);
                    }

                    if (objProductCategory.idx > 0)
                    {

                        objProductCategory.lastModifiedByUserIdx = Convert.ToInt32(Session["UID"].ToString());
                        objProductCategory.lastModificationDate = DateTime.Now.ToString("dd/MM/yyyy");
                        objProductBLL = new Product_BLL(objProductCategory);

                        bool flag = objProductBLL.Update();
                        return Json(new { data = "Updated", success = flag, statuscode = 200 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //objProductCategory.companyIdx = 1;
                        objProductCategory.createdByUserIdx = Convert.ToInt32(Session["UID"].ToString());
                        objProductCategory.TableName = "productDetailsForCombo";
                        objProductBLL = new Product_BLL(objProductCategory);
                        //if (objProductCategory.isMainBranch == 1)
                        //{
                        //    var check = objProductCategory.MainBranch();
                        //    if (check.Rows.Count > 0)
                        //    {
                        //        return Json(new { data = "Main Branch Already Exist", success = false, statuscode = 500 }, JsonRequestBehavior.AllowGet);
                        //    }
                        //}

                        bool flag = objProductBLL.Insert();
                        return Json(new { data = "Inserted", success = flag, statuscode = 200 }, JsonRequestBehavior.AllowGet);
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

                        Product_Property branchProperty = new Product_Property();
                        branchProperty.idx = int.Parse(id.ToString());
                        objProductBLL = new Product_BLL(id);
                        Product_BLL branhcBll = new Product_BLL(branchProperty);
                        var flag1 = branhcBll.Delete(id);
                        //if (flag1.Rows.Count > 0)
                        //{
                        if (true)
                        {
                            bool flag = objProductBLL.Delete(id);
                            
                            return Json(new { data = "Deleted", success = flag, statuscode = 200 }, JsonRequestBehavior.AllowGet);
                            
                        }
                        //else
                        //{
                        //    return Json(new { data = "Mian Branch Cannot be Delete ", success = false, statuscode = 400, count = 0 }, JsonRequestBehavior.AllowGet);
                        //}

                        //}
                        // return Json(new { data = "Process Completed ", success = true, statuscode = 200 }, JsonRequestBehavior.AllowGet);


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
    }
}