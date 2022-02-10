using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Renew.Core.Models {
    public enum AccountingDataQueryKeys {
        ENone               = 1 << 0,
        EClientName         = 1 << 1,
        EYear               = 1 << 2,
        EMonth              = 1 << 3,
        EDay                = 1 << 4,
        ESteelWeight        = 1 << 5,
        ESupplyPrice        = 1 << 6,
        ETaxAmount          = 1 << 7,
        EDataType           = 1 << 8,
        EDepositConfirmed   = 1 << 9,
        EDepositDate        = 1 << 10
    }

    public class AccountingData {
        public uint Uid { get; set; }
        public string ClientName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public float SteelWeight { get; set; }
        public uint SupplyPrice { get; set; }
        public uint TaxAmount { get; set; }
        public bool DataType { get; set; }
        public bool DepositConfirmed { get; set; }
        public string DepositDate { get; set; }
    }
}
