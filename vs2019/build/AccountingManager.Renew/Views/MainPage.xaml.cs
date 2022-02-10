using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.ViewModels;

namespace AccountingManager.Renew.Views {
    public sealed partial class MainPage : Page {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage() {
            InitializeComponent();

            SetTitleBar();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) {
            FrameHelper.TryGoBack(ContentFrame);
        }

        private void RootPage_Loaded(object sender, RoutedEventArgs e) {
            ContentFrame.Navigate(typeof(SelectionPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        public void CustomTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
            CoreApplicationViewTitleBar titleBar = sender as CoreApplicationViewTitleBar;
            if (titleBar == null) return;

            // Nothing to do right now...
        }

        private void SetTitleBar() {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.LayoutMetricsChanged += CustomTitleBarLayoutMetricsChanged;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Window.Current.SetTitleBar(UserLayout);

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
