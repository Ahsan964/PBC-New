using SSS.Property.Setups;
using SSS.Property.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SSS.Property.Transactions
{
    public class LP_FinsihProduct_Property
    {
        public int orderIdx { get; set; }
        public List<LP_SalesOrder_Master_Property> salesOrderLST { get; set; }
        public List<LP_Activity_Property> ActivityLST { get; set; }
        public List<Product_Property> ProductLST { get; set; }
        public List<LP_InventoryLogs_Property> InventoryDetails { get; set; }
        private DataTable _detail_data;
        public DataTable DetailData
        {
            get
            {
                return _detail_data;
            }
            set
            {
                _detail_data = value;
            }
        }
    }
}
