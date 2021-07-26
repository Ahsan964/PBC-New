using SSS.Property.Setups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSS.Property.Transactions.ViewModels
{
   public class LP_PI_ViewModel
    {
        // Added By Ahsan
        public int coaIdx { get; set; }
        public int idx { get; set; }
        public string InvoiceNo { get; set; }
        public int InvoiceType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetAmount { get; set; }
        public int CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public string CreatedDate { get; set; }
        public int Status { get; set; }
        public int PaymentType { get; set; }
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }

        public bool Taxable { get; set; }
        public int ParentDocID { get; set; }
        public int VendorID { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }

        public string Reference { get; set; }

        public string itemName { get; set; }
        public int itemIdx { get; set; }
        public decimal unitPrice { get; set; }
        public decimal qty { get; set; }
        public decimal returnQty { get; set; }//Added For  Return
        public decimal costPrice { get; set; } //Added By Arsalan
        public decimal stock { get; set; } //Added By Arsalan
        public decimal amount { get; set; }
        public decimal returnAmount { get; set; }//Added For  Return

        public int bankIdx { get; set; }
        public string accorChequeNumber { get; set; }

        public int GRNMasterID { get; set; }
        public List<Taxes_Property> TaxesList { get; set; }

        public List<Vendors_Property> VendorList { get; set; }
        public List<LP_Purchase_Master_Property> POLIST { get; set; }
        public List<LP_PI_Taxes_Property> PITAXLIST { get; set; }
        public List<Product_Property> ProductList { get; set; }
        public List<Company_Bank_Property> BankList { get; set; }
        public List<LP_P_Invoice_Details> InvoiceDetails { get; set; }
        public List<LP_P_Invoice_Details> UpdatedInvoiceDetails { get; set; }

        public List<LP_P_Invoice_Property> InvoiceProperty { get; set; }
        //Added By Arsalan kazmi 08-07-21
        public List<WareHouse_Property> WareHouseList { get; set; }
        [Required]
        public int WarerHouseID { get; set; }
    }
}
