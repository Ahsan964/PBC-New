using System;

namespace SSS.DAL.Report
{


    partial class InvRportDS
    {
        partial class INVREPORTDataTable
        {
            public int idx { get; set; }
            public int productIdx { get; set; }
            public string productName { get; set; }

            public int BranchId { get; set; }
            public string BranchName { get; set; }

            //public int BranchId { get; set; }
            public string Action { get; set; }

            public decimal stock { get; set; }
            public int productTypeIdx { get; set; }
            public decimal unitPrice { get; set; }
            public decimal totalAmount { get; set; }
            public DateTime creationDate { get; set; }
        }
    }
}
