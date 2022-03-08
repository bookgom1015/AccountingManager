using System;
using System.Collections.Generic;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Infrastructures;
using AccountingManager.Helpers.NavParams;

namespace AccountingManager.ViewModels {
    public class MonthlyNavViewModel : ViewModelBase {
        public MonthlyNavViewModel() { }

        public Result GetMonths(out IEnumerable<int> months, int? year = null, int? month = null, bool receivable = false) {
            return NavParams.DbManager.GetDates(out months, year, month, receivable);
        }

        public MonthlyNavParams NavParams { get; set; }
    }
}
