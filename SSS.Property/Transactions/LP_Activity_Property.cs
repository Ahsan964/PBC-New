using SSS.Property.Setups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSS.Property.Transactions
{
    public class LP_Activity_Property
    {
        
        public int idx { get; set; }
        public int orderIdx { get; set; }
        public int productTypeIdx { get; set; }
        public int vendorCatIdx { get; set; }
        public int vendorIdx { get; set; }
        public int productIdx { get; set; }
        public decimal qty { get; set; }
        public string size { get; set; }
        public string activityDate { get; set; }
        public decimal exchangeRate { get; set; }
        public decimal activityPrice { get; set; }
        public decimal subAmount { get; set; }
        public string description { get; set; }
        public string creationDate { get; set; }
        public int createdBy { get; set; }
        public int lastModifiedByUserIdx { get; set; }
        public string lastModificationDate { get; set; }
        public int status { get; set; }
        public List<Vendors_Property> vendorLST { get; set; }
        public List<Product_Property> productLST { get; set; }
        public List<Product_Type_Property> productTypeLST { get; set; }
        public List<LP_SalesOrder_Master_Property> salesOrderLST { get; set; }
        public List<Vendor_Category_Property> vendorCatLST { get; set; }
    }
}
