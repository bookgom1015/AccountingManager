using System;
using System.Threading.Tasks;

using AccountingManager.ViewModels;

using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Helpers;

namespace AccountingManager.Views
{
    public sealed partial class ViewSelectionPage : Page
    {
        private ViewSelectionViewModel ViewModel => DataContext as ViewSelectionViewModel;

        public ViewSelectionPage()
        {
            InitializeComponent();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CleanUp;
        }

        private async void Initialize()
        {
            bool connectionResult = await ViewModel.SqlManager.ConnectToDBAsync("192.168.0.4", 4885, "kbg", "@mDB901901@");
            if (!connectionResult)
            {
                await Logger.ShowAlertDialog("DB 연결 실패");
                return;
            }

            bool useResult = await ViewModel.SqlManager.UseDatabaseAsync("Test");
            if (!useResult)
            {
                await Logger.ShowAlertDialog("DB 참조 실패");
                return;
            }

            ViewModel.Connected = true;
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewModel.SqlManager.DisconnnectFromDB();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();

            base.OnNavigatedTo(e);
        }

        private void AccountingDataViewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AccountingDataListPage), ViewModel.SqlManager, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void AccountsReceivableViewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SalesRankViewButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
