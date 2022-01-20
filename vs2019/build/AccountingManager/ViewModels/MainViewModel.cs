using System;

using Windows.Storage;

using Prism.Windows.Mvvm;

using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            mSqlManager = new MariaDbManager();
            IsConnected = false;
        }

        public void LoadSettings()
        {
            //
            // Load local settings for application.
            //
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object windowWidthObj = localSettings.Values["WindowWidth"];
            WindowWidth = windowWidthObj == null ? DefaultWindowWidth : (double)windowWidthObj;

            Object windowHeightObj = localSettings.Values["WindowHeight"];
            WindowHeight = windowHeightObj == null ? DefaultWindowHeight : (double)windowHeightObj;
        }

        public void SaveSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["WindowWidth"] = WindowWidth;
            localSettings.Values["WindowHeight"] = WindowHeight;
        }

        private MariaDbManager mSqlManager = null;
        public MariaDbManager SqlManager
        {
            get => mSqlManager;
        }

        public bool IsConnected { get; set; }

        public double DefaultWindowWidth { get => 1024; }
        public double DefaultWindowHeight { get => 768; }

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

        public string HostName { get => "stdaewon.synology.me"; }
        public short Port { get => 5252; }
        public string UserId { get => "dw_user"; }
        public string Password { get => "@dbUSER901901@"; }
        public string DatabaseName
        {
#if DEBUG
            get => "Test";
#else
            get => "AccountingData";
#endif
        }
    }
}
