using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Core.Models
{
    public class ColumnOrders
    {
        public ColumnOrders()
        {
            TypeOrder = false;
            NameOrder = false;
            DateOrder = false;
            WeightOrder = false;
            PriceOrder = false;
            TaxOrder = false;
            SumOrder = false;
            ConfirmOrder = false;
        }

        private bool mTypeOrder;
        public bool TypeOrder
        {
            get => mTypeOrder;
            set => mTypeOrder = value;
        }

        private bool mNameOrder;
        public bool NameOrder
        {
            get => mNameOrder;
            set => mNameOrder = value;
        }

        private bool mDateOrder;
        public bool DateOrder
        {
            get => mDateOrder;
            set => mDateOrder = value;
        }

        private bool mWeightOrder;
        public bool WeightOrder
        {
            get => mWeightOrder;
            set => mWeightOrder = value;
        }

        private bool mPriceOrder;
        public bool PriceOrder
        {
            get => mPriceOrder;
            set => mPriceOrder = value;
        }

        private bool mTaxOrder;
        public bool TaxOrder
        {
            get => mTaxOrder;
            set => mTaxOrder = value;
        }

        private bool mSumOrder;
        public bool SumOrder
        {
            get => mSumOrder;
            set => mSumOrder = value;
        }

        private bool mConfirmOrder;
        public bool ConfirmOrder
        {
            get => mConfirmOrder;
            set => mConfirmOrder = value;
        }
    }
}
