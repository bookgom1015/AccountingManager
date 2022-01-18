using System;

using Prism.Windows.Mvvm;

namespace AccountingManager.ViewModels
{
    public class YearlyNavViewModel : ViewModelBase
    {
        public YearlyNavViewModel() { }

        public Action<string> YearList_SelectionChanged { get; set; }
    }
}
