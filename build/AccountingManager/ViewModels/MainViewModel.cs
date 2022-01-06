using System;
using System.Collections.Generic;
using System.Windows.Input;

using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

using Prism.Commands;
using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;
using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            LoadSettings();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CleanUp;

            SetWindowSize();
            SetTitleBarColor();

            ClickedButtonCommand = new DelegateCommand(OnClickButtonCommand);

            AccountingDataList = new List<AccountingData>();
            Orders = new ColumnOrders();

            SqlManager = new MariaManager();
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            SaveSettings();
            SqlManager.DisconnnectFromDB();
        }

        private void LoadSettings()
        {
            //
            // Load local settings for application.
            //
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object widthObj = localSettings.Values["WindowWidth"];
            WindowWidth = widthObj == null ? DefaultWindowWidth : (double)widthObj;

            Object heightObj = localSettings.Values["WindowHeight"];
            WindowHeight = heightObj == null ? DefaultWindowHeight : (double)heightObj;

            GridLength width = new GridLength(DefaultColumnWidth);

            Object TypeWidthObj = localSettings.Values["TypeColumnWidth"];
            TypeColumnWidth = TypeWidthObj == null ? width : new GridLength((double)TypeWidthObj);

            Object NameWidthObj = localSettings.Values["NameColumnWidth"];
            NameColumnWidth = NameWidthObj == null ? width : new GridLength((double)NameWidthObj);

            Object DateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = DateWidthObj == null ? width : new GridLength((double)DateWidthObj);

            Object WeigthWidthObj = localSettings.Values["WeightColumnWidth"];
            WeightColumnWidth = WeigthWidthObj == null ? width : new GridLength((double)WeigthWidthObj);

            Object PriceWidthObj = localSettings.Values["PriceColumnWidth"];
            PriceColumnWidth = PriceWidthObj == null ? width : new GridLength((double)PriceWidthObj);

            Object TaxWidthObj = localSettings.Values["TaxColumnWidth"];
            TaxColumnWidth = TaxWidthObj == null ? width : new GridLength((double)TaxWidthObj);

            Object SumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = SumWidthObj == null ? width : new GridLength((double)SumWidthObj);

            Object CheckWidthObj = localSettings.Values["CheckColumnWidth"];
            CheckColumnWidth = CheckWidthObj == null ? width : new GridLength((double)CheckWidthObj);
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

        private void SetWindowSize()
        {
            // For compatibility with Windows content size options.
            double scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            ApplicationView.PreferredLaunchViewSize = new Size(WindowWidth * scaleFactor, WindowHeight * scaleFactor);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private void SetTitleBarColor()
        {
            // Extract bytes from hex code.
            Color titleColor = Helpers.HexToColor.Convet("#FF202120");

            //
            // Set title bar and system menu buttons color.
            //
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = titleColor;
            titleBar.ButtonBackgroundColor = titleColor;

            titleBar.InactiveBackgroundColor = titleColor;
            titleBar.ButtonInactiveBackgroundColor = titleColor;
        }

        public bool FindAccountingData(int id, out AccountingData ret)
        {
            ret = null;
            foreach (AccountingData data in AccountingDataList)
            {
                if (data.Id == id)
                {
                    ret = data;
                    return true;
                }
            }
            return false;
        }

        public void AddAccountingData(string clientName, string date, int steelWeight = 0, int supplyPrice = 0, int taxAmount = 0, bool dataType = false, bool depositConfirm = false)
        {
            AccountingData data = new AccountingData(NextId++, clientName, date, steelWeight, supplyPrice, taxAmount, dataType, depositConfirm);
            AccountingDataList.Add(data);
        }        

        public bool RemoveAccountingData(int id)
        {
            foreach (AccountingData data in AccountingDataList)
            {
                if (data.Id == id)
                {
                    AccountingDataList.Remove(data);
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

        public ICommand ClickedButtonCommand { get; set; }
        private async void OnClickButtonCommand()
        {
            MessageDialog msgDialog = new MessageDialog("Clicked the button");
            await msgDialog.ShowAsync();
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

        private List<AccountingData> mAccountingDataList;
        public List<AccountingData> AccountingDataList
        {
            get => mAccountingDataList;
            set => SetProperty(ref mAccountingDataList, value);
        }

        private ColumnOrders mOrders;
        public ColumnOrders Orders
        {
            get => mOrders;
            set => mOrders = value;
        }

        private Comparison<AccountingData> mAccountingDataComparision = Comparisions.CompareName;
        public Comparison<AccountingData> AccountingDataComparision
        {
            get => mAccountingDataComparision;
            set => mAccountingDataComparision = value;
        }

        private string mDisplayedStr;
        public string DisplayedStr
        {
            get => mDisplayedStr;
            set => mDisplayedStr = value;
        }

        private int mNextId = 0;
        public int NextId
        {
            get => mNextId;
            set => mNextId = value;
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager
        {
            get => mSqlManager;
            set => mSqlManager = value;
        }
    }
}
