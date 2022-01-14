using System;

using Windows.Storage;
using Windows.UI.Xaml;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;
using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class AccountingDataListViewModel : ViewModelBase
    {
        public AccountingDataListViewModel() {}

        public void Initialize()
        {
            LoadSettings();

            mSelectedData = new AccountingData();
        }

        public void CleanUp()
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
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

        private void SaveSettings()
        {
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

        public MariaManager SqlManager { get; set; }

        private double DefaultColumnWidth { get => 110; }
        public double DefaultMinColumnWidth { get => 100; }

        private GridLength mDataTypeColumnWidth;
        public GridLength DataTypeColumnWidth
        {
            get => mDataTypeColumnWidth;
            set => SetProperty(ref mDataTypeColumnWidth, value);
        }

        private GridLength mClientNameColumnWidth;
        public GridLength ClientNameColumnWidth
        {
            get => mClientNameColumnWidth;
            set => SetProperty(ref mClientNameColumnWidth, value);
        }

        private GridLength mDateColumnWidth;
        public GridLength DateColumnWidth
        {
            get => mDateColumnWidth;
            set => SetProperty(ref mDateColumnWidth, value);
        }

        private GridLength mSteelWeightColumnWidth;
        public GridLength SteelWeightColumnWidth
        {
            get => mSteelWeightColumnWidth;
            set => SetProperty(ref mSteelWeightColumnWidth, value);
        }

        private GridLength mSupplyPriceColumnWidth;
        public GridLength SupplyPriceColumnWidth
        {
            get => mSupplyPriceColumnWidth;
            set => SetProperty(ref mSupplyPriceColumnWidth, value);
        }

        private GridLength mTaxAmountColumnWidth;
        public GridLength TaxAmountColumnWidth
        {
            get => mTaxAmountColumnWidth;
            set => SetProperty(ref mTaxAmountColumnWidth, value);
        }

        private GridLength mSumColumnWidth;
        public GridLength SumColumnWidth
        {
            get => mSumColumnWidth;
            set => SetProperty(ref mSumColumnWidth, value);
        }

        private GridLength mDepositConfirmColumnWidth;
        public GridLength DepositConfirmColumnWidth
        {
            get => mDepositConfirmColumnWidth;
            set => SetProperty(ref mDepositConfirmColumnWidth, value);
        }

        public bool DataTypeOrder { get; set; }
        public bool ClientNameOrder { get; set; }
        public bool DateOrder { get; set; }
        public bool SteelWeightOrder { get; set; }
        public bool SupplyPriceOrder { get; set; }
        public bool TaxAmountOrder { get; set; }
        public bool SumOrder { get; set; }
        public bool DepositConfirmOrder { get; set; }

        public string SelectedYearText { get; set; }

        private bool mYearSelected = false;
        public bool YearSelected
        {
            get => mYearSelected;
            set => SetProperty(ref mYearSelected, value);
        }

        private AccountingData mSelectedData;
        public AccountingData SelectedData
        {
            get => mSelectedData;
        }
    }
}
