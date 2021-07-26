using Newtonsoft.Json;
using SSS.Property.Setups;
using SSS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSYSTEM.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index(int? id,int? query)
        {
            if (Session["LOGGEDIN"] != null)
            {
                ViewBag.Data = Helper.ConvertDataTable<MRNVM_Property>((GetRecieptData(id, query)));
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        

        
    }
}