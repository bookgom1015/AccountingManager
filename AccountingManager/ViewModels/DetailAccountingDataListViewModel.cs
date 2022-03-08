using System;

using Prism.Windows.Mvvm;

using AccountingManager.Helpers.NavParams;

namespace AccountingManager.ViewModels {
    public class DetailAccountingDataListViewModel : ViewModelBase {
        public DetailAccountingDataListViewModel() { }

        public DetailAccountingDataListNavParams NavParams { get; set; }
    }
}
