using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSS.Property.Setups.Accounts
{
    public class AccountGJ
    {
        // Added By Ahsan
        public string IncometaxPercent { get; set; }
        public string ITAmount { get; set; }
        public string whSalesTaxAmount { get; set; }
        public decimal previousBalance { get; set; }
        public decimal paidAmount { get; set; }
        public string customerName { get; set; }
        public decimal balanceAmount { get; set; }
        public decimal paid { get; set; }

        public int idx { get; set; }
        public int GLIdx { get; set; }
        public int tranTypeIdx { get; set; }
        public int userIdx { get; set; }
        public int vendorIdx { get; set; }
        public int employeeIdx { get; set; }
        public int customerIdx { get; set; }
        public int coaIdx { get; set; }
        public string invoiceNo { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public DateTime createDate { get; set; }
        public string modifiedDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
