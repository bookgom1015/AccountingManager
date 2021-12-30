using System;
using System.Windows.Input;
using System.Collections.Generic;

using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Core.Preview;

using Prism.Commands;
using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;

namespace AccountingManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public const Int32 CSIDL_PERSONAL   = 0x0005; // My Documents

        public MainViewModel()
        {
            LoadSettings();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += SaveSettings;

            SetWindowSize();
            SetTitleBarColor();
            GenerateDateMap();

            ClickedButtonCommand = new DelegateCommand(OnClickButtonCommand);
        }

        private void LoadSettings()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            Object widthObj = localSettings.Values["WindowWidth"];
            WindowWidth = widthObj == null ? mDefaultWindowWidth : (double)widthObj;

            Object heightObj = localSettings.Values["WindowHeight"];
            WindowHeight = heightObj == null ? mDefaultWindowHeight : (double)heightObj;

            Object NameWidthObj = localSettings.Values["NameColumnWidth"];
            NameColumnWidth = NameWidthObj == null ? mDefaultColumnWidth : new GridLength((double)NameWidthObj);

            Object DateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = DateWidthObj == null ? mDefaultColumnWidth : new GridLength((double)DateWidthObj);

            Object PriceWidthObj = localSettings.Values["PriceColumnWidth"];
            PriceColumnWidth = PriceWidthObj == null ? mDefaultColumnWidth : new GridLength((double)PriceWidthObj);

            Object TaxWidthObj = localSettings.Values["TaxColumnWidth"];
            TaxColumnWidth = TaxWidthObj == null ? mDefaultColumnWidth : new GridLength((double)TaxWidthObj);

            Object SumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = SumWidthObj == null ? mDefaultColumnWidth : new GridLength((double)SumWidthObj);

            Object CheckWidthObj = localSettings.Values["CheckColumnWidth"];
            CheckColumnWidth = CheckWidthObj == null ? mDefaultColumnWidth : new GridLength((double)CheckWidthObj);
        }

        private void SaveSettings(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["WindowWidth"] = WindowWidth;
            localSettings.Values["WindowHeight"] = WindowHeight;
            localSettings.Values["NameColumnWidth"] = NameColumnWidth.Value;
            localSettings.Values["DateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["PriceColumnWidth"] = PriceColumnWidth.Value;
            localSettings.Values["TaxColumnWidth"] = TaxColumnWidth.Value;
            localSettings.Values["SumColumnWidth"] = SumColumnWidth.Value;
            localSettings.Values["CheckColumnWidth"] = CheckColumnWidth.Value;
        }

        private void SetWindowSize()
        {
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

        private void GenerateDateMap()
        {
            DateMap = new Dictionary<int, List<int>>();

            List<int> months_19 = new List<int>();
            for (int i = 7; i <= 9; ++i)
                months_19.Add(i);
            DateMap.Add(2019, months_19);

            List<int> months_20 = new List<int>();
            for (int i = 4; i <= 6; ++i)
                months_20.Add(i);
            DateMap.Add(2020, months_20);

            List<int> months_21 = new List<int>();
            for (int i = 1; i <= 3; ++i)
                months_21.Add(i);
            DateMap.Add(2021, months_21);
        }

        private async void OnClickButtonCommand()
        {
            MessageDialog msgDialog = new MessageDialog("Clicked the button");
            await msgDialog.ShowAsync();
        }

        public void OnWindowSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
        }

        public ICommand ClickedButtonCommand { get; set; }

        private const double mDefaultWindowWidth = 1024;
        private const double mDefaultWindowHeight = 768;

        private GridLength mDefaultColumnWidth = new GridLength(110);

        private string mStr;
        public string Str
        {
            get => mStr;
            set => SetProperty(ref mStr, value);
        }

        private double mDefaultMinColumnWidth = 80;
        public double DefaultMinColumnWidth
        {
            get => mDefaultMinColumnWidth;
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

        private Dictionary<int, List<int>> mDateMap;
        public Dictionary<int, List<int>> DateMap
        {
            get => mDateMap;
            set => SetProperty(ref mDateMap, value);
        }
    }
}
