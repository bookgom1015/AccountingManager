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

            mSelectedData = new AccountingData(-1, "", "");
        }

        public void CleanUp()
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            
            Object dataTypeWidthObj = localSettings.Values["DataTypeColumnWidth"];
            DataTypeColumnWidth = dataTypeWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)dataTypeWidthObj);

            Object clientNameWidthObj = localSettings.Values["ClientNameColumnWidth"];
            ClientNameColumnWidth = clientNameWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)clientNameWidthObj);

            Object dateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = dateWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)dateWidthObj);

            Object steelWeightWidthObj = localSettings.Values["SteelWeightColumnWidth"];
            SteelWeightColumnWidth = steelWeightWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)steelWeightWidthObj);

            Object supplyPriceWidthObj = localSettings.Values["SupplyPriceColumnWidth"];
            SupplyPriceColumnWidth = supplyPriceWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)supplyPriceWidthObj);

            Object taxAmountWidthObj = localSettings.Values["TaxAmountColumnWidth"];
            TaxAmountColumnWidth = taxAmountWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)taxAmountWidthObj);

            Object sumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = sumWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)sumWidthObj);

            Object depositConfirmWidthObj = localSettings.Values["DepositConfirmColumnWidth"];
            DepositConfirmColumnWidth = depositConfirmWidthObj == null ? new GridLength(DefaultColumnWidth) : new GridLength((double)depositConfirmWidthObj);
        }

        private void SaveSettings()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["DataTypeColumnWidth"] = DataTypeColumnWidth.Value;
            localSettings.Values["ClientNameColumnWidth"] = ClientNameColumnWidth.Value;
            localSettings.Values["DateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["SteelWeightColumnWidth"] = SteelWeightColumnWidth.Value;
            localSettings.Values["SupplyPriceColumnWidth"] = SupplyPriceColumnWidth.Value;
            localSettings.Values["TaxAmountColumnWidth"] = TaxAmountColumnWidth.Value;
            localSettings.Values["SumColumnWidth"] = SumColumnWidth.Value;
            localSettings.Values["DepositConfirmColumnWidth"] = DepositConfirmColumnWidth.Value;
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager
        {
            get => mSqlManager;
            set => mSqlManager = value;
        }

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

        private bool mDataTypeOrder;
        public bool DataTypeOrder
        {
            get => mDataTypeOrder;
            set => mDataTypeOrder = value;
        }

        private bool mClientNameOrder;
        public bool ClientNameOrder
        {
            get => mClientNameOrder;
            set => mClientNameOrder = value;
        }

        private bool mDateOrder;
        public bool DateOrder
        {
            get => mDateOrder;
            set => mDateOrder = value;
        }

        private bool mSteelWeightOrder;
        public bool SteelWeightOrder
        {
            get => mSteelWeightOrder;
            set => mSteelWeightOrder = value;
        }

        private bool mSupplyPriceOrder;
        public bool SupplyPriceOrder
        {
            get => mSupplyPriceOrder;
            set => mSupplyPriceOrder = value;
        }

        private bool mTaxAmountOrder;
        public bool TaxAmountOrder
        {
            get => mTaxAmountOrder;
            set => mTaxAmountOrder = value;
        }

        private bool mSumOrder;
        public bool SumOrder
        {
            get => mSumOrder;
            set => mSumOrder = value;
        }

        private bool mDepositConfirmOrder;
        public bool DepositConfirmOrder
        {
            get => mDepositConfirmOrder;
            set => mDepositConfirmOrder = value;
        }

        private string mSelectedYearText;
        public string SelectedYearText
        {
            get => mSelectedYearText;
            set => mSelectedYearText = value;
        }

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
