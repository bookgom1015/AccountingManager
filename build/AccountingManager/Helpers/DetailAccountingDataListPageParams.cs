using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml;

using AccountingManager.Core.Models;
using AccountingManager.ViewModels;

namespace AccountingManager.Helpers
{
    public class DetailAccountingDataListPageParams
    {
        private bool mReversed;
        public bool Reversed
        {
            get => mReversed;
            set => mReversed = value;
        }

        private List<AccountingData> mAccountingDataList;
        public List<AccountingData> AccountingDataList
        {
            get => mAccountingDataList;
            set => mAccountingDataList = value;
        }

        private AccountingDataListViewModel mParentViewModel;
        public AccountingDataListViewModel ParentViewModel
        {
            get => mParentViewModel;
            set => mParentViewModel = value;
        }

        public Action<int, string, string, int, int, int, bool, bool> AccountingDataList_SelectionChagned { get; set; }
    }
}
