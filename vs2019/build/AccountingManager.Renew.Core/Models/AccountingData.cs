using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Renew.Core.Models {
    public enum AccountingDataQueryKeys {
        ENone               = 1 << 0,
        EClientName         = 1 << 1,
        EDate               = 1 << 2,
        ESteelWeight        = 1 << 3,
        ESupplyPrice        = 1 << 4,
        ETaxAmount          = 1 << 5,
        EDataType           = 1 << 6,
        EDepositConfirmed   = 1 << 7,
        EDepositDate        = 1 << 8
    }

    public class AccountingData {
        public uint Uid { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public float SteelWeight { get; set; }
        public uint SupplyPrice { get; set; }
        public uint TaxAmount { get; set; }
        public bool DataType { get; set; }
        public bool DepositConfirmed { get; set; }
        public string DepositDate { get; set; }

        public string DataTypeFormatting { get => DataType ? "매입" : "매출"; }
        public string DateFormatting { get => Date.ToString("yyyy-MM-dd"); }
        public string SteeWeightFormatting { get => string.Format("{0}", SteelWeight); }
        public string SupplyPriceFormatting { get => string.Format("{0:#,##0}", SupplyPrice); }
        public string TaxAmountFormatting { get => string.Format("{0:#,##0}", TaxAmount); }
        public uint Sum { get => SupplyPrice + TaxAmount; }
        public string SumFormatting { get => string.Format("{0:#,##0}", Sum); }
    }
}
