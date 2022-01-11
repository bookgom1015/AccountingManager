using System;
using System.Collections.Generic;

using Prism.Windows.Mvvm;

namespace AccountingManager.ViewModels
{
    public class MonthlyNavViewModel : ViewModelBase
    {
        public MonthlyNavViewModel()
        {
        }

        public Action<string> DataLitView_SelectionChanged { get; set; }
    }
}
