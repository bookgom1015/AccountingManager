using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using AccountingManager.Core.Models;

namespace AccountingManager.Helpers.NavParams {
    public class DetailAccountingDataListNavParams {
        public Binding DataTypeColWidthBinding { get; set; }
        public Binding ClientNameColWidthBinding { get; set; }
        public Binding DateColWidthBinding { get; set; }
        public Binding SteelWeightColWidthBinding { get; set; }
        public Binding SupplyPriceColWidthBinding { get; set; }
        public Binding TaxAmountColWidthBinding { get; set; }
        public Binding SumColWidthBinding { get; set; }
        public Binding DepositConfirmedColWidthBinding { get; set; }

        public IEnumerable<AccountingData> AccountingData { get; set; }
        public Action<AccountingData> SelectedAccountingDataChanged { get; set; }
    }
}
