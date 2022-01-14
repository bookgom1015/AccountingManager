using System;
using System.Collections.Generic;
using System.Linq;

using AccountingManager.Core.Models;
using AccountingManager.ViewModels;

namespace AccountingManager.Helpers
{
    public class DetailAccountingDataListPageParams
    {
        public DetailAccountingDataListPageParams(bool inReversed, List<AccountingData> inDataList, AccountingDataListViewModel inViewModel, Action<int, string, string, int, int, int, bool, bool> inAction)
        {
            mReversed = inReversed;
            mAccountingDataList = inDataList;
            mParentViewModel = inViewModel;
            mAccountingDataList_SelectionChagned = inAction;
        }

        bool mReversed;
        public bool Reversed { get => mReversed; }

        private List<AccountingData> mAccountingDataList;
        public List<AccountingData> AccountingDataList { get => mAccountingDataList; }

        private AccountingDataListViewModel mParentViewModel;
        public AccountingDataListViewModel ParentViewModel { get => mParentViewModel; }

        private Action<int, string, string, int, int, int, bool, bool> mAccountingDataList_SelectionChagned;
        public Action<int, string, string, int, int, int, bool, bool> AccountingDataList_SelectionChagned { get => mAccountingDataList_SelectionChagned; }
    }
}
