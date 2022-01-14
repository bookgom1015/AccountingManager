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

            await initTask;

            //
            // After intialization.
            //
            Task<bool> connectionTask = ViewModel.SqlManager.ConnectToDBAsync("bookgom.synology.me", 4885, "kbg", "@mDB901901@");

            SetWindowSize();
            SetTitleBar();

            bool connectionResult = await connectionTask;
            if (!connectionResult)
            {
                await Logger.ShowAlertDialog("DB 연결 실패");
                return;
            }

            bool useResult = await ViewModel.SqlManager.UseDatabaseAsync("Test");
            if (!useResult) await Logger.ShowAlertDialog("DB 참조 실패");
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewModel.CleanUp();

            if (ViewModel.SqlManager != null)
                ViewModel.SqlManager.DisconnnectFromDB();
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
            if (frame == null || !frame.CanGoBack) return false;

            frame.GoBack();

            return true;
        }

        public void CustomTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            CoreApplicationViewTitleBar titleBar = sender as CoreApplicationViewTitleBar;
            if (titleBar == null) return;

            // Nothing to do right now...
        }

        private async void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize();

            ViewSelectionPageParams navParams = new ViewSelectionPageParams(ViewModel.SqlManager);

            ContentFrame.Navigate(typeof(ViewSelectionPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
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
