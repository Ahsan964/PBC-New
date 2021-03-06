using SSS.DAL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using SSS.Property.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSS.BLL.Transactions
{
   public class LP_PInvoice_BLL
    {    
        LP_P_Invoice_Property _objpinvoice;

        LP_Purchase_Invoice_DAL objDAL;
        public LP_PInvoice_BLL()
        {

        }
        public LP_PInvoice_BLL(LP_P_Invoice_Property objpinvoice)
        {
            _objpinvoice = objpinvoice;
        }

        public bool Delete()
        {
            objDAL = new LP_Purchase_Invoice_DAL(_objpinvoice);
            return objDAL.Delete();
        }

        // Insert If ParentDocId > 0
        public bool Insert()
        {
            objDAL = new LP_Purchase_Invoice_DAL(_objpinvoice);
            return objDAL.Insert();
        }

        // Insert Direct Purchase
        public bool InsertDP()
        {
            objDAL = new LP_Purchase_Invoice_DAL(_objpinvoice);
            return objDAL.InsertDP();
        }

        public DataTable SelectAll()
        {
            objDAL = new LP_Purchase_Invoice_DAL();
            return objDAL.SelectAll();
        }
        public DataTable SelectOne()
        {
            objDAL = new LP_Purchase_Invoice_DAL(_objpinvoice);
            return objDAL.SelectOne();
        }

        public DataSet SelectByID()
        {
            objDAL = new LP_Purchase_Invoice_DAL(_objpinvoice);
            return objDAL.SelectByID();
        }
        public DataTable SelectPIByDate(LP_Voucher_ViewModel objvoucherinvoice)
        {
            objDAL = new LP_Purchase_Invoice_DAL();
            return objDAL.SelectPIBYDate(objvoucherinvoice);
        }

        public string GeneratePINo(LP_GenerateTransNumber_Property objtransno)
        {
            string TransactionNumber = "";
            objDAL = new LP_Purchase_Invoice_DAL();
            DataTable dt = objDAL.GeneratePINo(objtransno);
            if (dt.Rows.Count > 0)
            {
                var check = DateTime.Now.ToString("yy-MM-dd");
                var check1 = check.Split('-');
                foreach (DataRow dr in dt.Rows)
                {
                    TransactionNumber = dr["TransNumber"].ToString();
                    //TransactionNumber = "PI-00" + TransactionNumber + "-" + objtransno.userid;
                    TransactionNumber = "PI-" + check1[0] + "-" + check1[1] + "-00" + TransactionNumber;


                }
                return TransactionNumber;
            }
            else
            {

                TransactionNumber = "PI-001-" + objtransno.userid;

                return TransactionNumber;
            }
            //return _objMRNDAL.GenerateMRNNo(objtransno);
        }

        public DataTable SelectPIByVendorId(int id )
        {
            objDAL = new LP_Purchase_Invoice_DAL();
            return objDAL.SelectAllByVendorID(id);
        }
        public DataSet SelectPIWithDetailData(int id)
        {
            objDAL = new LP_Purchase_Invoice_DAL();
            return objDAL.SelectAllPIDetailsDataById(id);
        }


    }
}
