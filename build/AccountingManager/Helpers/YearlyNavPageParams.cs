using System;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Helpers
{
    public class YearlyNavPageParams
    {
        public YearlyNavPageParams()
        {
            mYearList = new List<int>();
        }

        private List<int> mYearList;
        public List<int> YearList
        {
            get => mYearList;
        }

        public Action<string> YearList_SelectionChanged { get; set; }
    }
}
