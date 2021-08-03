using SSS.DAL.Transactions;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSS.BLL.Transactions
{
     public class Payment_BLL
    {

        payment_property objpaymentprop;
        Payment_DAL objDAL;

        public Payment_BLL()
        {

        }

        public Payment_BLL(payment_property _objpaymentprop)
        {
            objpaymentprop = _objpaymentprop;
        }

        public bool InsertReceipt()
        {
            objDAL = new Payment_DAL(objpaymentprop);
            return objDAL.InsertReceipt();
        }
        public bool InsertPayment()
        {
            objDAL = new Payment_DAL(objpaymentprop);
            return objDAL.InsertPayment();
        }
        public DataTable SelectAll()
        {
            objDAL = new Payment_DAL();
            return objDAL.SelectAll();
        }
        public DataTable SelectAllPaymentVoucher()
        {
            objDAL = new Payment_DAL();
            return objDAL.SelectAllPaymentVoucher();
        }
        public DataTable SelectAllReceiptVoucher()
        {
            objDAL = new Payment_DAL();
            return objDAL.SelectAllReceiptVoucher();
        }

        // Added By Ahsan
        public DataTable SelectOnePaymentVoucher()
        {
            objDAL = new Payment_DAL(objpaymentprop);
            return objDAL.SelectOnePaymentVoucher();
        }

        public DataTable SelectOne()
        {
            objDAL = new Payment_DAL(objpaymentprop);
            return objDAL.SelectOne();
        }


        public string GenerateTransNo(LP_GenerateTransNumber_Property objtransno)
        {
            string TransactionNumber = "";
            objDAL = new Payment_DAL();
            DataTable dt = objDAL.GenerateVoucherNo(objtransno);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //if (objtransno.tranTypeIdx == "5")
                    //{
                    //    TransactionNumber = dr["TransNumber"].ToString();
                    //    TransactionNumber = "REC-00" + TransactionNumber + "-" + objtransno.userid;
                    //}
                    //else
                    //{
                        TransactionNumber = dr["TransNumber"].ToString();
                        TransactionNumber = "PAY-00" + TransactionNumber + "-" + objtransno.userid;
                 //   }



                }
                return TransactionNumber;
            }
            else
            {

                TransactionNumber = "VR-001-" + objtransno.userid;

                return TransactionNumber;
            }
            //return _objMRNDAL.GenerateMRNNo(objtransno);
        }

        public DataTable getcashBalance()
        {
            objDAL = new Payment_DAL();
            return objDAL.getCashBalance();
        }
        public DataTable getcustomerPaymentBalance()
        {
            objDAL = new Payment_DAL();
            return objDAL.getCustomerPaymentBalance();
        }
        public DataTable getvendorBalance(int id)
        {
            objDAL = new Payment_DAL();
            return objDAL.getvendorBalance(id);
        }
        public DataTable getvendorInvoices(int id)
        {
            objDAL = new Payment_DAL();
            return objDAL.getcustomerInvoices(id);
        }
    }
}

    