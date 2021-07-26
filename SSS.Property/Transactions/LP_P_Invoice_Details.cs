using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSS.Property.Transactions
{
  public  class LP_P_Invoice_Details
    {
        private int _idx;
        public int idx
        {
            get { return _idx; }
            set { _idx = value; }
        }

        private int _PIIdx;
        public int PIIdx
        {
            get { return _PIIdx; }
            set { _PIIdx = value; }
        }
        private int _returnIdx;
        public int returnIdx
        {
            get { return _returnIdx; }
            set { _returnIdx = value; }
        }

        private int _ItemIdx;
        public int ItemIdx
        {
            get { return _ItemIdx; }
            set { _ItemIdx = value; }
        }

        private int _Qty;
        public int Qty
        {
            get { return _Qty; }
            set { _Qty = value; }
        }
        private int? _ReturnQty;
        public int? ReturnQty
        {
            get { return _ReturnQty; }
            set { _ReturnQty = value; }
        }
        private decimal _UnitPrice;
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { _UnitPrice = value; }
        }

        private decimal _TotalAmount;
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set { _TotalAmount = value; }
        }

        private decimal? _ReturnAmount;
        public decimal? ReturnAmount
        {
            get { return _ReturnAmount; }
            set { _ReturnAmount = value; }
        }

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
