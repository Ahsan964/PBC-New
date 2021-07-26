
#region New


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSS.Property.Setups;
using SSS.Property.Transactions;
using SSS.DAL.Transactions;
using System.Data;

namespace SSS.BLL.Transactions
{
    public class LP_Voucher_BLL
    {
        LP_Voucher_Property objvoucherproperty;
        LP_Voucher_DAL objDAL;

        public LP_Voucher_BLL()
        {

        }

        public LP_Voucher_BLL(LP_Voucher_Property _objvoucherproperty)
        {
            objvoucherproperty = _objvoucherproperty;
        }

        // Added By Ahsan
        public DataTable SelectOneReceiptVoucher()
        {
            objDAL = new LP_Voucher_DAL(objvoucherproperty);
            return objDAL.SelectOneReceiptVoucher();
        }

        // Receipt Delete
        public bool DeleteReceipt()
        {
            objDAL = new LP_Voucher_DAL(objvoucherproperty);
            return objDAL.DeleteReceipt();
        }

        public bool InsertReceipt()
        {
            objDAL = new LP_Voucher_DAL(objvoucherproperty);
            return objDAL.InsertReceipt();
        }
        public bool InsertPayment()
        {
            objDAL = new LP_Voucher_DAL(objvoucherproperty);
            return objDAL.InsertPayment();
        }
        public DataTable SelectAll()
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.SelectAll();
        }
        public DataTable SelectAllPaymentVoucher()
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.SelectAllPaymentVoucher();
        }
        public DataTable SelectAllReceiptVoucher()
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.SelectAllReceiptVoucher();
        }
        public DataTable SelectOne()
        {
            objDAL = new LP_Voucher_DAL(objvoucherproperty);
            return objDAL.SelectOne();
        }


        public string GenerateTransNo(LP_GenerateTransNumber_Property objtransno)
        {
            string TransactionNumber = "";
            objDAL = new LP_Voucher_DAL();
            DataTable dt = objDAL.GenerateVoucherNo(objtransno);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (objtransno.tranTypeIdx == "5")
                    {
                        TransactionNumber = dr["TransNumber"].ToString();
                        TransactionNumber = "REC-00" + TransactionNumber + "-" + objtransno.userid;
                    }
                    else
                    {
                        TransactionNumber = dr["TransNumber"].ToString();
                        TransactionNumber = "PAY-00" + TransactionNumber + "-" + objtransno.userid;
                    }



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
            objDAL = new LP_Voucher_DAL();
            return objDAL.getCashBalance();
        }
        public DataTable getcustomerPaymentBalance()
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.getCustomerPaymentBalance();
        }
        public DataTable getvendorBalance(int id)
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.getvendorBalance(id);
        }
        public DataTable getcustomerInvoices(int id)
        {
            objDAL = new LP_Voucher_DAL();
            return objDAL.getcustomerInvoices(id);
        }
        
    }
}

#endregion



#region Old

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SSS.Property.Setups;
//using SSS.Property.Transactions;
//using SSS.DAL.Transactions;
//using System.Data;

//namespace SSS.BLL.Transactions
//{
//    public class LP_Voucher_BLL
//    {
//        LP_Voucher_Property objvoucherproperty;
//        LP_Voucher_DAL objDAL;

//        public LP_Voucher_BLL()
//        {

//        }

//        public LP_Voucher_BLL(LP_Voucher_Property _objvoucherproperty)
//        {
//            objvoucherproperty = _objvoucherproperty;
//        }

//        public bool Insert()
//        {
//            objDAL = new LP_Voucher_DAL(objvoucherproperty);
//            return objDAL.Insert();
//        }


//        public DataTable SelectDepartment()
//        {
//            objDAL = new LP_Voucher_DAL(objvoucherproperty);
//            return objDAL.SelectByDepartmentId();
//        }


//        public DataTable SelectAll()
//        {
//            objDAL = new LP_Voucher_DAL();
//            return objDAL.SelectAll();
//        }
//        public DataTable SelectOne()
//        {
//            objDAL = new LP_Voucher_DAL(objvoucherproperty);
//            return objDAL.SelectOne();
//        }


//        public string GenerateTransNo(LP_GenerateTransNumber_Property objtransno)
//        {
//            string TransactionNumber = "";
//            objDAL = new LP_Voucher_DAL();
//            DataTable dt = objDAL.GenerateVoucherNo(objtransno);
//            if (dt.Rows.Count > 0)
//            {
//                var check = DateTime.Now.ToString("yy-MM-dd");
//                var check1 = check.Split('-');
//                foreach (DataRow dr in dt.Rows)
//                {
//                    TransactionNumber = dr["TransNumber"].ToString();
//                    //TransactionNumber = "VR-00" + TransactionNumber + "-" + objtransno.userid;
//                    TransactionNumber = "VR-" + check1[0] + "-" + check1[1] + "-00" + TransactionNumber;


//                }
//                return TransactionNumber;
//            }
//            else
//            {

//                TransactionNumber = "VR-001-" + objtransno.userid;

//                return TransactionNumber;
//            }
//            //return _objMRNDAL.GenerateMRNNo(objtransno);
//        }
//    }
//}


#endregion