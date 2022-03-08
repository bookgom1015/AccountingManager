using System;

using Prism.Windows.Mvvm;

using AccountingManager.Helpers.NavParams;

namespace AccountingManager.ViewModels {
    public class DetailAccountsReceivableListViewModel : ViewModelBase {
        public DetailAccountsReceivableListViewModel() { }

        public DetailAccountsReceivableListNavParams NavParams { get; set; }
    }
}
