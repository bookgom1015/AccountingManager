using System;

using System.Collections.Generic;

using Prism.Windows.Mvvm;

namespace AccountingManager.ViewModels
{
    public class YearlyNavViewModel : ViewModelBase
    {
        public YearlyNavViewModel()
        {
        }

        private Dictionary<int, List<int>> mDateMap;
        public Dictionary<int, List<int>> DateMap
        {
            get => mDateMap;
            set => SetProperty(ref mDateMap, value);
        }

        public Action<string> DataLitView_SelectionChanged { get; set; }
    }
}
