using SSS.DAL.Transactions;
using SSS.Property.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSS.BLL.Transactions
{
    public class LP_Activity_BLL
    {
        private LP_Activity_Property _objACTMasterProperty;
     
        private LP_Activity_DAL _objACTDAL;
        public LP_Activity_BLL()
        {

        }
        public LP_Activity_BLL(LP_Activity_Property objACTMasterProperty)
        {
            _objACTMasterProperty = objACTMasterProperty;
        }
        public bool Insert()
        {
            _objACTDAL = new LP_Activity_DAL(_objACTMasterProperty);
            return _objACTDAL.Insert();
        }

    }
}
