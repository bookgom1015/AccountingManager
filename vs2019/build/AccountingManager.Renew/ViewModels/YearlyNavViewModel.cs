using System;
using System.Collections.Generic;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class YearlyNavViewModel : ViewModelBase {
        public YearlyNavViewModel() { }

        public Result GetDates(out IEnumerable<int> years, int? year = null, int? month = null, bool receivable = false) {
            return NavParams.DbManager.GetDates(out years, year, month, receivable);
        }

        public YearlyNavParams NavParams { get; set; }
    }
}
