using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Core.Models
{
    public class AccountingData
    {
        public AccountingData(string clientName, string date, int supplyPrice = 0, int taxAmount = 0, bool dataType = false, bool depositConfirm = false)
        {
            DataType = dataType;
            ClientName = clientName;
            Date = date;
            SupplyPrice = supplyPrice;
            TaxAmount = taxAmount;
            DepositConfirm = depositConfirm;
        }

        private bool mDataType;
        public bool DataType
        {
            get => mDataType;
            set => mDataType = value;
        }

        private string mClientName;
        public string ClientName
        {
            get => mClientName;
            set => mClientName = value;
        }

        private string mDate;
        public string Date
        {
            get => mDate;
            set => mDate = value;
        }

        private int mSupplyPrice;
        public int SupplyPrice
        {
            get => mSupplyPrice;
            set => mSupplyPrice = value;
        }

        private int mTaxAmount;
        public int TaxAmount
        {
            get => mTaxAmount;
            set => mTaxAmount = value;
        }

        private bool mDepositConfirm;
        public bool DepositConfirm
        {
            get => mDepositConfirm;
            set => mDepositConfirm = value;
        }
    }
}
