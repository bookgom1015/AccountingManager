using System;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class DetailAccountsReceivableListViewModel : ViewModelBase {
        public DetailAccountsReceivableListViewModel() { }

        public DetailAccountsReceivableListNavParams NavParams { get; set; }
    }
}
