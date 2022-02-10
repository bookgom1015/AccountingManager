
namespace AccountingManager.Core.Models {
    public class AccountingData {
        public enum QueryKeys {
            ENone           = 1 << 0,
            EClientName     = 1 << 1,
            EDate           = 1 << 2,
            ESteelWeight    = 1 << 3,
            ESupplyPrice    = 1 << 4,
            ETaxAmount      = 1 << 5,
            EDataType       = 1 << 6,
            EDepositConfirm = 1 << 7,
            EDepositDate    = 1 << 8
        }

        public class QueryValues {
            public string ClientName { get; set; }
            public string Date { get; set; }
            public int SteelWeight { get; set; }
            public int SupplyPrice { get; set; }
            public int TaxAmount { get; set; }
            public bool DataType { get; set; }
            public bool DepositConfirm { get; set; }
        }

        public AccountingData(int id = -1, string clientName = "", string date = "", string depositDate = "", int steelWeight = 0, int supplyPrice = 0, int taxAmount = 0, bool dataType = false, bool depositConfirm = false) {
            Id = id;
            DataType = dataType;
            ClientName = clientName;
            Date = date;
            DepositDate = depositDate;
            SteelWeight = steelWeight;
            SupplyPrice = supplyPrice;
            TaxAmount = taxAmount;
            DepositConfirm = depositConfirm;
        }

        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Date { get; set; }
        public int SteelWeight { get; set; }
        public int SupplyPrice { get; set; }
        public int TaxAmount { get; set; }
        public bool DataType { get; set; }
        public bool DepositConfirm { get; set; }
        public string DepositDate { get; set; }
    }
}
