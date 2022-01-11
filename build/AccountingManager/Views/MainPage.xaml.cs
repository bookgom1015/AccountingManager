using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Helpers;
using AccountingManager.ViewModels;

namespace AccountingManager.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CleanUp;
        }

        private async Task Initialize()
        {
            //
            // Initialization must be called first.
            //
            Task initTask = Logger.StaticInit();

            ViewModel.Initialize();

            //
            // After intialization.
            //
            SetWindowSize();
            SetTitleBar();

            await initTask;
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewModel.CleanUp();
        }

        private void SetWindowSize()
        {
            // For compatibility with Windows content size options.
            double scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            ApplicationView.PreferredLaunchViewSize = new Size(ViewModel.WindowWidth * scaleFactor, ViewModel.WindowHeight * scaleFactor);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private void SetTitleBar()
        {
            // Now this app doesn't have a title bar.
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CustomTitleBarLayoutMetricsChanged;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(UserLayout);

            //
            // Set title bar and system menu buttons color.
            //
            ApplicationViewTitleBar appViewTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            appViewTitleBar.ButtonForegroundColor = Colors.Black;
            appViewTitleBar.ButtonBackgroundColor = Colors.Transparent;
            appViewTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private bool TryGoBack(Frame frame)
        {
            if (!frame.CanGoBack) return false;

            frame.GoBack();

            return true;
        }

        public void CustomTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (!(sender is CoreApplicationViewTitleBar)) return;

            CoreApplicationViewTitleBar titleBar = sender as CoreApplicationViewTitleBar;
            // Nothing to do right now...
        }

        private async void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize();

            ContentFrame.Navigate(typeof(ViewSelectionPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void RootPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.WindowWidth = e.NewSize.Width;
            ViewModel.WindowHeight = e.NewSize.Height;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TryGoBack(ContentFrame);
        }
    }
}
