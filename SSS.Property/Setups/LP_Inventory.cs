using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSS.Property.Setups
{
   public class LP_Inventory
    {
        public int idx { get; set; }
        public int productIdx { get; set; }
        public decimal stock { get; set; }
        public int productTypeIdx { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalAmount { get; set; }
        public DateTime creationDate { get; set; }
    }
}
