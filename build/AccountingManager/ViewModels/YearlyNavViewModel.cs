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

        public void GenerateDateMap()
        {
            DateMap = new Dictionary<int, List<int>>();

            List<int> months = new List<int>();
            for (int i = 1; i <= 12; ++i)
                months.Add(i);

            DateMap.Add(2011, months);
            DateMap.Add(2012, months);
            DateMap.Add(2013, months);
            DateMap.Add(2014, months);
            DateMap.Add(2015, months);
            DateMap.Add(2016, months);
            DateMap.Add(2017, months);
            DateMap.Add(2018, months);
            DateMap.Add(2019, months);
            DateMap.Add(2020, months);
            DateMap.Add(2021, months);
        }

        private Dictionary<int, List<int>> mDateMap;
        public Dictionary<int, List<int>> DateMap
        {
            get => mDateMap;
            set => SetProperty(ref mDateMap, value);
        }
    }
}
