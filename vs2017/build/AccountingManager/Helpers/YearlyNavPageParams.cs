﻿using System;
using System.Collections.Generic;

namespace AccountingManager.Helpers
{
    public class YearlyNavPageParams
    {
        public YearlyNavPageParams(List<int> inList, Action<string> inAction)
        {
            mYearList = inList;
            mYearList_SelectionChanged = inAction;
        }

        private List<int> mYearList;
        public List<int> YearList { get => mYearList; }

        private Action<string> mYearList_SelectionChanged;
        public Action<string> YearList_SelectionChanged { get => mYearList_SelectionChanged; }
    }
}