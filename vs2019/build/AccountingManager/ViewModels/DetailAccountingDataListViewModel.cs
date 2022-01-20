using System;

using Prism.Windows.Mvvm;

namespace AccountingManager.ViewModels
{
    public class DetailAccountingDataListViewModel : ViewModelBase
    {
        public DetailAccountingDataListViewModel() { }

        public Action<int, string, string, string, int, int, int, bool, bool> AccountingDataList_SelectionChagned { get; set; }
    }
}
