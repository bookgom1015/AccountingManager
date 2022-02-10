using System;

using AccountingManager.ViewModels;

namespace AccountingManager.Helpers {
    public class DetailAccountingDataListPageParams {
        public DetailAccountingDataListPageParams(AccountingDataListViewModel inViewModel, Action<int, string, string, string, int, int, int, bool, bool> inAction) {
            mParentViewModel = inViewModel;
            mAccountingDataList_SelectionChagned = inAction;
        }

        private AccountingDataListViewModel mParentViewModel;
        public AccountingDataListViewModel ParentViewModel { get => mParentViewModel; }

        private Action<int, string, string, string, int, int, int, bool, bool> mAccountingDataList_SelectionChagned;
        public Action<int, string, string, string, int, int, int, bool, bool> AccountingDataList_SelectionChagned { get => mAccountingDataList_SelectionChagned; }
    }
}
