using System;
using System.Collections.Generic;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Helpers;
using AccountingManager.Core.Infrastructures;
using AccountingManager.Core.Models;
using AccountingManager.Helpers.NavParams;

namespace AccountingManager.ViewModels {
    public class AccountsReceivableListViewModel : ViewModelBase {
        public AccountsReceivableListViewModel() {
            AccountingDataList = new List<AccountingData>();

            CurrentComparision = AccountingDataComparisions.CompareDate;

            ColumnDirIndicators = new List<BitmapIcon>();
        }

        public void LoadSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object dataTypeWidthObj = localSettings.Values["RecvDataTypeColumnWidth"];
            DataTypeColumnWidth = new GridLength(dataTypeWidthObj == null ? DefaultColumnWidth : (double)dataTypeWidthObj);

            Object clientNameWidthObj = localSettings.Values["RecvClientNameColumnWidth"];
            ClientNameColumnWidth = new GridLength(clientNameWidthObj == null ? DefaultColumnWidth : (double)clientNameWidthObj);

            Object dateWidthObj = localSettings.Values["RecvDateColumnWidth"];
            DateColumnWidth = new GridLength(dateWidthObj == null ? DefaultColumnWidth : (double)dateWidthObj);

            Object sumWidthObj = localSettings.Values["RecvSumColumnWidth"];
            SumColumnWidth = new GridLength(sumWidthObj == null ? DefaultColumnWidth : (double)sumWidthObj);
        }

        public void SaveSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["RecvDataTypeColumnWidth"] = DataTypeColumnWidth.Value;
            localSettings.Values["RecvClientNameColumnWidth"] = ClientNameColumnWidth.Value;
            localSettings.Values["RecvDateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["RecvSumColumnWidth"] = SumColumnWidth.Value;
        }

        public Result GetData(out IEnumerable<AccountingData> data, int? year = null, int? month = null, int? day = null, bool receivable = false) {
            return NavParams.DbManager.GetData(out data, year, month, day, receivable);
        }

        public Result GetData(out IEnumerable<AccountingData> data, DateTime begin, DateTime end, string clientName, bool receivable = false) {
            return NavParams.DbManager.GetData(out data, begin, end, clientName, receivable);
        }

        public Result AddData(AccountingData data) {
            return NavParams.DbManager.Add(data);
        }

        public Result EditData(AccountingData data) {
            AccountingDataQueryKeys queryKeys = AccountingDataQueryKeys.ENone;

            if (SelectedData.ClientName != data.ClientName) {
                queryKeys |= AccountingDataQueryKeys.EClientName;
                SelectedData.ClientName = data.ClientName;
            }
            if (SelectedData.Date != data.Date) {
                queryKeys |= AccountingDataQueryKeys.EDate;
                SelectedData.Date = data.Date;
            }
            if (SelectedData.SteelWeight != data.SteelWeight) {
                queryKeys |= AccountingDataQueryKeys.ESteelWeight;
                SelectedData.SteelWeight = data.SteelWeight;
            }
            if (SelectedData.SupplyPrice != data.SupplyPrice) {
                queryKeys |= AccountingDataQueryKeys.ESupplyPrice;
                SelectedData.SupplyPrice = data.SupplyPrice;
            }
            if (SelectedData.TaxAmount != data.TaxAmount) {
                queryKeys |= AccountingDataQueryKeys.ETaxAmount;
                SelectedData.TaxAmount = data.TaxAmount;
            }
            if (SelectedData.DataType != data.DataType) {
                queryKeys |= AccountingDataQueryKeys.EDataType;
                SelectedData.DataType = data.DataType;
            }
            if (SelectedData.DepositConfirmed != data.DepositConfirmed) {
                queryKeys |= AccountingDataQueryKeys.EDepositConfirmed;
                SelectedData.DepositConfirmed = data.DepositConfirmed;
            }
            if (SelectedData.DepositDate != data.DepositDate) {
                queryKeys |= AccountingDataQueryKeys.EDepositDate;
                SelectedData.DepositDate = data.DepositDate;
            }

            return NavParams.DbManager.Update(SelectedData, queryKeys);
        }

        public Result RemoveData(AccountingData data) {
            return NavParams.DbManager.Delete(data);
        }

        public void SortAccountingData() {
            AccountingDataList.Sort(CurrentComparision);
        }

        public void ConfigureColumnDirIndicators(BitmapIcon visibleIndicator) {
            foreach (var indicator in ColumnDirIndicators)
                indicator.Visibility = indicator == visibleIndicator ? Visibility.Visible : Visibility.Collapsed;
        }

        public AccountsReceivableListNavParams NavParams { get; set; }

        private double DefaultColumnWidth { get => 110; }
        public double DefaultMinColumnWidth { get => 100; }

        private GridLength dataTypeColumnWidth;
        public GridLength DataTypeColumnWidth {
            get => dataTypeColumnWidth;
            set => SetProperty(ref dataTypeColumnWidth, value);
        }

        private GridLength clientNameColumnWidth;
        public GridLength ClientNameColumnWidth {
            get => clientNameColumnWidth;
            set => SetProperty(ref clientNameColumnWidth, value);
        }

        private GridLength dateColumnWidth;
        public GridLength DateColumnWidth {
            get => dateColumnWidth;
            set => SetProperty(ref dateColumnWidth, value);
        }

        private GridLength sumColumnWidth;
        public GridLength SumColumnWidth {
            get => sumColumnWidth;
            set => SetProperty(ref sumColumnWidth, value);
        }

        public AccountingData SelectedData { get; set; }
        public bool DataIsSelected { get => SelectedData != null; }

        public int? SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }

        public string LookingForClietnName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsForSearching { get => BeginDate != null && EndDate != null; }

        public List<AccountingData> AccountingDataList { get; set; }
        public Comparison<AccountingData> CurrentComparision { get; set; }

        public List<BitmapIcon> ColumnDirIndicators { get; }

        public bool DataEditted { get; set; }
    }
}
