using System;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class DetailAccountingDataListViewModel : ViewModelBase {
        public DetailAccountingDataListViewModel() { }

        public DetailAccountingDataListNavParams NavParams { get; set; }
    }
}
