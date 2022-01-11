using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;
using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel() {}

        public void Initialize()
        {
            AccountingDataList = new List<AccountingData>();
            mOrders = new ColumnOrders();
            mSqlManager = new MariaManager();

            LoadSettings();
        }

        public void CleanUp()
        {
            SaveSettings();

            mSqlManager.DisconnnectFromDB();
        }

        private void LoadSettings()
        {
            //
            // Load local settings for application.
            //
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object windowWidthObj = localSettings.Values["WindowWidth"];
            WindowWidth = windowWidthObj == null ? DefaultWindowWidth : (double)windowWidthObj;

            Object windowHeightObj = localSettings.Values["WindowHeight"];
            WindowHeight = windowHeightObj == null ? DefaultWindowHeight : (double)windowHeightObj;

            GridLength width = new GridLength(DefaultColumnWidth);

            Object typeWidthObj = localSettings.Values["TypeColumnWidth"];
            TypeColumnWidth = typeWidthObj == null ? width : new GridLength((double)typeWidthObj);

            Object nameWidthObj = localSettings.Values["NameColumnWidth"];
            NameColumnWidth = nameWidthObj == null ? width : new GridLength((double)nameWidthObj);

            Object dateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = dateWidthObj == null ? width : new GridLength((double)dateWidthObj);

            Object weightWidthObj = localSettings.Values["WeightColumnWidth"];
            WeightColumnWidth = weightWidthObj == null ? width : new GridLength((double)weightWidthObj);

            Object priceWidthObj = localSettings.Values["PriceColumnWidth"];
            PriceColumnWidth = priceWidthObj == null ? width : new GridLength((double)priceWidthObj);

            Object taxWidthObj = localSettings.Values["TaxColumnWidth"];
            TaxColumnWidth = taxWidthObj == null ? width : new GridLength((double)taxWidthObj);

            Object sumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = sumWidthObj == null ? width : new GridLength((double)sumWidthObj);

            Object checkWidthObj = localSettings.Values["CheckColumnWidth"];
            CheckColumnWidth = checkWidthObj == null ? width : new GridLength((double)checkWidthObj);
        }

        private void SaveSettings()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["WindowWidth"] = WindowWidth;
            localSettings.Values["WindowHeight"] = WindowHeight;
            localSettings.Values["TypeColumnWidth"] = TypeColumnWidth.Value;
            localSettings.Values["NameColumnWidth"] = NameColumnWidth.Value;
            localSettings.Values["DateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["WeightColumnWidth"] = WeightColumnWidth.Value;
            localSettings.Values["PriceColumnWidth"] = PriceColumnWidth.Value;
            localSettings.Values["TaxColumnWidth"] = TaxColumnWidth.Value;
            localSettings.Values["SumColumnWidth"] = SumColumnWidth.Value;
            localSettings.Values["CheckColumnWidth"] = CheckColumnWidth.Value;
        }

        public bool FindAccountingData(int id, out AccountingData ret)
        {
            foreach (AccountingData data in AccountingDataList)
            {
                if (data.Id == id)
                {
                    ret = data;
                    return true;
                }
            }

            ret = null;
            return false;
        }

        public void AddAccountingData(int inId, string inClientName, string inDate, int inSteelWeight = 0, int inSupplyPrice = 0, int inTaxAmount = 0, bool inDataType = false, bool inDepositConfirm = false)
        {
            AccountingData data = new AccountingData(inId, inClientName, inDate, inSteelWeight, inSupplyPrice, inTaxAmount, inDataType, inDepositConfirm);
            AccountingDataList.Add(data);
        }        

        public bool RemoveAccountingData(int id)
        {
            foreach (AccountingData data in AccountingDataList)
            {
                if (data.Id == id)
                {
                    if (!AccountingDataList.Remove(data))
                    {
                        Logger.Logln("AccountingDataList.Remove failed");
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }
                
        public void OnWindowSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
        }

        private ColumnOrders mOrders;
        public ColumnOrders Orders
        {
            get => mOrders;
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager
        {
            get => mSqlManager;
        }

        private List<AccountingData> mAccountingDataList;
        public List<AccountingData> AccountingDataList
        {
            get => mAccountingDataList;
            set => SetProperty(ref mAccountingDataList, value);
        }

        private Comparison<AccountingData> mAccountingDataComparision = Comparisions.CompareDate;
        public Comparison<AccountingData> AccountingDataComparision
        {
            get => mAccountingDataComparision;
            set => mAccountingDataComparision = value;
        }

        private int mNextId;
        public int NextId
        {
            get => mNextId;
            set => mNextId = value;
        }

        private int mSelectedYear = -1;
        public int SelectedYear
        {
            get => mSelectedYear;
            set => mSelectedYear = value;
        }

        public double DefaultWindowWidth
        {
            get => 1024;
        }

        public double DefaultWindowHeight
        {
            get => 768;
        }

        private double DefaultColumnWidth
        {
            get => 110;
        }

        public double DefaultMinColumnWidth
        {
            get => 100;
        }

        private double mWindowWidth;
        public double WindowWidth
        {
            get => mWindowWidth;
            set => SetProperty(ref mWindowWidth, value);
        }

        private double mWindowHeight;
        public double WindowHeight
        {
            get => mWindowHeight;
            set => SetProperty(ref mWindowHeight, value);
        }

        private string mDocumentFilePath;
        public string DocumentFilePath
        {
            get => mDocumentFilePath;
            set => SetProperty(ref mDocumentFilePath, value);
        }

        private GridLength mTypeColumnWidth;
        public GridLength TypeColumnWidth
        {
            get => mTypeColumnWidth;
            set => SetProperty(ref mTypeColumnWidth, value);
        }

        private GridLength mNameColumnWidth;
        public GridLength NameColumnWidth
        {
            get => mNameColumnWidth;
            set => SetProperty(ref mNameColumnWidth, value);
        }

        private GridLength mDateColumnWidth;
        public GridLength DateColumnWidth
        {
            get => mDateColumnWidth;
            set => SetProperty(ref mDateColumnWidth, value);
        }

        private GridLength mWeightColumnWidth;
        public GridLength WeightColumnWidth
        {
            get => mWeightColumnWidth;
            set => SetProperty(ref mWeightColumnWidth, value);
        }

        private GridLength mPriceColumnWidth;
        public GridLength PriceColumnWidth
        {
            get => mPriceColumnWidth;
            set => SetProperty(ref mPriceColumnWidth, value);
        }

        private GridLength mTaxColumnWidth;
        public GridLength TaxColumnWidth
        {
            get => mTaxColumnWidth;
            set => SetProperty(ref mTaxColumnWidth, value);
        }

        private GridLength mSumColumnWidth;
        public GridLength SumColumnWidth
        {
            get => mSumColumnWidth;
            set => SetProperty(ref mSumColumnWidth, value);
        }

        private GridLength mCheckColumnWidth;
        public GridLength CheckColumnWidth
        {
            get => mCheckColumnWidth;
            set => SetProperty(ref mCheckColumnWidth, value);
        }

        public int DataTypeTextBlockColumn { get => 0; }
        public int ClientNameTextBlockColumn { get => 1; }
        public int DateTextBlockColumn { get => 2; }
        public int SteelWeightTextBlockColumn { get => 3; }
        public int SupplyPriceTextBlockColumn { get => 4; }
        public int TaxAmountTextBlockColumn { get => 5; }
        public int SumTextBlockColumn { get => 6; }
        public int DepositConfirmBlockColumn { get => 7; }
    }
}
