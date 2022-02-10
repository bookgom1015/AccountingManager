using System;
using System.Collections.Generic;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class YearlyNavViewModel : ViewModelBase {
        public YearlyNavViewModel() { }

        public Result GetDates(out IEnumerable<int> years) {
            return NavParams.DbManager.GetDates(out years);
        }

        public YearlyNavParams NavParams { get; set; }
    }
}
