using System;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;

namespace AccountingManager.ViewModels
{
    public class DetailAccountingDataListViewModel : ViewModelBase
    {
        public DetailAccountingDataListViewModel() {}

        public Action<int, string, string, int, int, int, bool, bool> AccountingDataList_SelectionChagned { get; set; }
    }
}
