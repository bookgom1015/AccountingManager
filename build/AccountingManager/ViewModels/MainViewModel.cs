using System;
using Windows.Storage;
using Windows.UI.Xaml;

using Prism.Windows.Mvvm;

namespace AccountingManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel() {}

        public void Initialize()
        {
            LoadSettings();
        }

        public void CleanUp()
        {
            SaveSettings();
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
            WindowHeight = windowHeightObj == null ? DefaultWindowHeight : (double)windowHeightObj;;
        }

        private void SaveSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["WindowWidth"] = WindowWidth;
            localSettings.Values["WindowHeight"] = WindowHeight;
        }
                
        public void OnWindowSizeChanged(Object sender, SizeChangedEventArgs e)
        {
            WindowWidth = e.NewSize.Width;
            WindowHeight = e.NewSize.Height;
        }

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
    }
}
