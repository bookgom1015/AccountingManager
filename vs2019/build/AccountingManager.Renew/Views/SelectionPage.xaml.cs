using System;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Dialogs;
using AccountingManager.Renew.Dialogs.Controls;
using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.Helpers.NavParams;
using AccountingManager.Renew.ViewModels;

namespace AccountingManager.Renew.Views {
    public sealed partial class SelectionPage : Page {
        private SelectionViewModel ViewModel => DataContext as SelectionViewModel;

        public SelectionPage() {
            InitializeComponent();

            Windows.ApplicationModel.Core.CoreApplication.EnteredBackground += OnEnteredBackground;
        }

        private void OnEnteredBackground(Object sender, EnteredBackgroundEventArgs e) {
            Deferral deferral = e.GetDeferral();

            ViewModel.SaveSettings();
            ViewModel.DisconnectFromDb();

            deferral.Complete();
        }

        protected override async  void OnNavigatedTo(NavigationEventArgs e) {
            ViewModel.LoadSettings();

            if (ViewModel.CanConnect) {
                Result result = ViewModel.ConnectToDb();
                if (!result.Status) {
                    MessageDialog msgDialog = new MessageDialog() { Title = "로그인 실패", Message = result.ErrMsg };
                    await msgDialog.ShowAsync();
                }
            }

            while (!ViewModel.IsConnected) {
                LoginControls controls = new LoginControls();
                LoginDialog loginDialog = new LoginDialog(controls);

                ContentDialogResult dialogResult = await loginDialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    ViewModel.UserName = controls.UserName;
                    ViewModel.Password = controls.Password;

                    Result connResult = ViewModel.ConnectToDb();
                    if (!connResult.Status) {
                        MessageDialog msgDialog = new MessageDialog() { Title = "로그인 실패", Message = connResult.ErrMsg };
                        await msgDialog.ShowAsync();
                    }
                }
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataViewButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            AccountingDataListNavParams navParams = new AccountingDataListNavParams { DbManager = ViewModel.DbManager };
            this.Frame.Navigate(typeof(AccountingDataListPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void AccountsReceivableViewButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) { }

        private void SalesRankViewButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) { }
    }
}
