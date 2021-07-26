using SSS.DAL.Transactions;
using SSS.Property.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSS.BLL.Transactions
{
    public class LP_PIReturn_BLL
    {
        private LP_PIReturn_Property _objMRMasterProperty;
        private LP_PIReturn_Details_Property _objMRDetailProperty;
        private LP_PIReturn_DAL _objMRNDAL;
        public LP_PIReturn_BLL()
        {

        }
        public LP_PIReturn_BLL(LP_PIReturn_Property objMRMasterProperty)
        {
            _objMRMasterProperty = objMRMasterProperty;
        }
        public LP_PIReturn_BLL(LP_PIReturn_Details_Property objMRDetailProperty)
        {
            _objMRDetailProperty = objMRDetailProperty;
        }
        public bool Insert()
        {
            _objMRNDAL = new LP_PIReturn_DAL(_objMRMasterProperty);
            return _objMRNDAL.Insert();

        }
    }
}
