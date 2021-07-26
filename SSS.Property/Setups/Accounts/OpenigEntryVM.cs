using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSS.Property.Setups.Accounts
{
    public class OpenigEntryVM
    {
        public int idx { get; set; }
        [Required]
        public int openingType { get; set; }
        public string voucherNo { get; set; }
        [DataType(DataType.Date)]
        public string voucherDate { get; set; }
        public int accountIdx { get; set; }
        public List<fourthTier_Property> AccountLST { get; set; }
        public List<AccountGJ> AccountGJLST { get; set; }
        public int tranTypeIdx { get; set; }
        public int userIdx { get; set; }
        public decimal debit { get; set; }
        public decimal credit { get; set; }
        public DateTime createDate { get; set; }
        public decimal paidAmount { get; set; }
        public decimal balance { get; set; }
        public decimal Amount { get; set; }
        public int isCredit { get; set; }
        public int visible { get; set; }
        public bool isChecked { get; set; }
        public int createdByUserIdx { get; set; }
        public string lastModificationDate { get; set; }
        public int lastModifiedByUserIdx { get; set; }
        public decimal totalAmount { get; set; }
        public string memo { get; set; }
        public int glIdx { get; set; }
    }
}
