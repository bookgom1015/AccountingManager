using System;
using System.Collections.Generic;

namespace AccountingManager.Helpers
{
    public class DateNavPageParams
    {
        private string mYearText;
        public string YearText
        {
            get => mYearText;
            set => mYearText = value;
        }

        private SortedSet<int> mMonthSet;
        public SortedSet<int> MonthSet
        {
            get => mMonthSet;
            set => mMonthSet = value;
        }

        public Action<string> DateLitView_SelectionChanged { get; set; }
    }
}
