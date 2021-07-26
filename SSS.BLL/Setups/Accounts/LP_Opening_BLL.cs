using SSS.DAL.Setups.Accounts;
using SSS.Property.Setups;
using SSS.Property.Setups.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSS.BLL.Setups.Accounts
{
    public class LP_Opening_BLL
    {
        private OpenigEntryVM objActivityPropery;
        private LP_OpenningEntry_DAL objOpenningEntryDAL;
        public LP_Opening_BLL()
        {
        }
        public LP_Opening_BLL(OpenigEntryVM obOpenigEntryVM)
        {
            objActivityPropery = obOpenigEntryVM;

        }
        public DataTable ViewAll()
        {
            objOpenningEntryDAL = new LP_OpenningEntry_DAL(objActivityPropery);
            return objOpenningEntryDAL.SelectAll();
        }

        public DataTable GetfourthTierById()
        {
            objOpenningEntryDAL = new LP_OpenningEntry_DAL(objActivityPropery);
            return objOpenningEntryDAL.SelectById();
        }
        public bool Insert()
        {
            objOpenningEntryDAL = new LP_OpenningEntry_DAL(objActivityPropery);
            return objOpenningEntryDAL.Insert();
        }
        public bool Update()
        {
            objOpenningEntryDAL = new LP_OpenningEntry_DAL(objActivityPropery);
            return objOpenningEntryDAL.Update();
        }
        public bool DeleteAccount()
        {
            objOpenningEntryDAL = new LP_OpenningEntry_DAL(objActivityPropery);
            return objOpenningEntryDAL.DeleteAccounts();
        }
        public string GenerateSO(LP_GenerateTransNumber_Property objtransno)
        {
            string TransactionNumber = "";
            objOpenningEntryDAL = new LP_OpenningEntry_DAL();
            DataTable dt = objOpenningEntryDAL.GenerateSONo(objtransno);
            if (dt.Rows.Count > 0)
            {
                var check = DateTime.Now.ToString("yy-MM-dd");
                var check1 = check.Split('-');
                foreach (DataRow dr in dt.Rows)
                {
                    TransactionNumber = dr["TransNumber"].ToString();
                    //TransactionNumber = "OP-00" + (int.Parse(TransactionNumber) + 1) + "-" + objtransno.userid;
                    TransactionNumber = "OP-" + check1[0] + "-" + check1[1] + "-00" + (int.Parse(TransactionNumber) + 1);


                }
                return TransactionNumber;
            }
            else
            {

                TransactionNumber = "SO-001-" + objtransno.userid;

                return TransactionNumber;
            }
        }
    }
}
