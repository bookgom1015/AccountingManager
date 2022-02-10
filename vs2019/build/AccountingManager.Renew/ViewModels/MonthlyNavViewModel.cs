using System;
using System.Collections.Generic;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class MonthlyNavViewModel : ViewModelBase {
        public MonthlyNavViewModel() { }

        public Result GetMonths(out IEnumerable<int> months, int year) {
            return NavParams.DbManager.GetDates(out months, year);
        }

        public MonthlyNavParams NavParams { get; set; }
    }
}
