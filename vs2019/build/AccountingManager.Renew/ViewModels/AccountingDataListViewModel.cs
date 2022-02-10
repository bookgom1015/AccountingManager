using System;
using System.Collections.Generic;

using Windows.Storage;
using Windows.UI.Xaml;

using Prism.Windows.Mvvm;

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Core.Helpers;
using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.ViewModels {
    public class AccountingDataListViewModel : ViewModelBase {
        public AccountingDataListViewModel() {
            CurrentComparision = AccountingDataComparisions.CompareDate;
        }

        public void LoadSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object dataTypeWidthObj = localSettings.Values["DataTypeColumnWidth"];
            DataTypeColumnWidth = new GridLength(dataTypeWidthObj == null ? DefaultColumnWidth : (double)dataTypeWidthObj);

            Object clientNameWidthObj = localSettings.Values["ClientNameColumnWidth"];
            ClientNameColumnWidth = new GridLength(clientNameWidthObj == null ? DefaultColumnWidth : (double)clientNameWidthObj);

            Object dateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = new GridLength(dateWidthObj == null ? DefaultColumnWidth : (double)dateWidthObj);

            Object steelWeightWidthObj = localSettings.Values["SteelWeightColumnWidth"];
            SteelWeightColumnWidth = new GridLength(steelWeightWidthObj == null ? DefaultColumnWidth : (double)steelWeightWidthObj);

            Object supplyPriceWidthObj = localSettings.Values["SupplyPriceColumnWidth"];
            SupplyPriceColumnWidth = new GridLength(supplyPriceWidthObj == null ? DefaultColumnWidth : (double)supplyPriceWidthObj);

            Object taxAmountWidthObj = localSettings.Values["TaxAmountColumnWidth"];
            TaxAmountColumnWidth = new GridLength(taxAmountWidthObj == null ? DefaultColumnWidth : (double)taxAmountWidthObj);

            Object sumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = new GridLength(sumWidthObj == null ? DefaultColumnWidth : (double)sumWidthObj);

            Object depositConfirmWidthObj = localSettings.Values["DepositConfirmColumnWidth"];
            DepositConfirmColumnWidth = new GridLength(depositConfirmWidthObj == null ? DefaultColumnWidth : (double)depositConfirmWidthObj);
        }

        public void SaveSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["DataTypeColumnWidth"] = DataTypeColumnWidth.Value;
            localSettings.Values["ClientNameColumnWidth"] = ClientNameColumnWidth.Value;
            localSettings.Values["DateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["SteelWeightColumnWidth"] = SteelWeightColumnWidth.Value;
            localSettings.Values["SupplyPriceColumnWidth"] = SupplyPriceColumnWidth.Value;
            localSettings.Values["TaxAmountColumnWidth"] = TaxAmountColumnWidth.Value;
            localSettings.Values["SumColumnWidth"] = SumColumnWidth.Value;
            localSettings.Values["DepositConfirmColumnWidth"] = DepositConfirmColumnWidth.Value;
        }

        public Result GetData(out IEnumerable<AccountingData> data, int? year = null, int? month = null, int? day = null) {
            return NavParams.DbManager.GetData(out data, year, month, day);
        }

        public Result GetData(out IEnumerable<AccountingData> data, DateTime begin, DateTime end, string clientName) {
            return NavParams.DbManager.GetData(out data, begin, end, clientName);
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
            if (SelectedData.Year != data.Year) {
                queryKeys |= AccountingDataQueryKeys.EYear;
                SelectedData.Year = data.Year;
            }
            if (SelectedData.Month != data.Month) {
                queryKeys |= AccountingDataQueryKeys.EMonth;
                SelectedData.Month = data.Month;
            }
            if (SelectedData.Day != data.Day) {
                queryKeys |= AccountingDataQueryKeys.EDay;
                SelectedData.Day = data.Day;
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

        public AccountingDataListNavParams NavParams { get; set; }

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

        private GridLength steelWeightColumnWidth;
        public GridLength SteelWeightColumnWidth {
            get => steelWeightColumnWidth;
            set => SetProperty(ref steelWeightColumnWidth, value);
        }

        private GridLength supplyPriceColumnWidth;
        public GridLength SupplyPriceColumnWidth {
            get => supplyPriceColumnWidth;
            set => SetProperty(ref supplyPriceColumnWidth, value);
        }

        private GridLength taxAmountColumnWidth;
        public GridLength TaxAmountColumnWidth {
            get => taxAmountColumnWidth;
            set => SetProperty(ref taxAmountColumnWidth, value);
        }

        private GridLength sumColumnWidth;
        public GridLength SumColumnWidth {
            get => sumColumnWidth;
            set => SetProperty(ref sumColumnWidth, value);
        }

        private GridLength depositConfirmColumnWidth;
        public GridLength DepositConfirmColumnWidth {
            get => depositConfirmColumnWidth;
            set => SetProperty(ref depositConfirmColumnWidth, value);
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
    }
}
