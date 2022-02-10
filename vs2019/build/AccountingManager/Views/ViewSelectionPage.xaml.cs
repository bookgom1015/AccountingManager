using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Helpers;
using AccountingManager.ViewModels;

namespace AccountingManager.Views {
    public sealed partial class ViewSelectionPage : Page {
        private ViewSelectionViewModel ViewModel => DataContext as ViewSelectionViewModel;

        public ViewSelectionPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            ViewSelectionPageParams navParams = e.Parameter as ViewSelectionPageParams;
            if (navParams != null)
            {
                ViewModel.SqlManager = navParams.SqlManager;
                ViewModel.DatabaseName = navParams.DatabaseName;
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataViewButton_Click(object sender, RoutedEventArgs e) {
            AccountingDataListPageParams navParams = new AccountingDataListPageParams(ViewModel.SqlManager, ViewModel.DatabaseName);

            this.Frame.Navigate(typeof(AccountingDataListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void AccountsReceivableViewButton_Click(object sender, RoutedEventArgs e) { }

        private void SalesRankViewButton_Click(object sender, RoutedEventArgs e) { }
    }
}
