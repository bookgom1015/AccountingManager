using System;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Dialogs;
using AccountingManager.Renew.ViewModels;
using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.Helpers.NavParams;

namespace AccountingManager.Renew.Views {
    public sealed partial class YearlyNavPage : Page {
        private YearlyNavViewModel ViewModel => DataContext as YearlyNavViewModel;

        public YearlyNavPage() {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            YearlyNavParams navParams = e.Parameter as YearlyNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;

                IEnumerable<int> years;
                Result result = ViewModel.GetDates(out years, null, null, navParams.Receivable);
                if (result.Status) {
                    foreach (int year in years)
                        YearListBox.Items.Add(year);
                }
                else {
                    MessageDialog dialog = new MessageDialog { Title = "오류", Message = result.ErrMsg };
                    await dialog.ShowAsync();
                }
            }

            base.OnNavigatedTo(e);
        }

        private void YearListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (YearListBox.SelectedItem is int) {
                int year = (int)YearListBox.SelectedItem;

                var navParams = ViewModel.NavParams;
                navParams.SelectedYearChanged(year);

                MonthlyNavParams newNavParams = new MonthlyNavParams {
                    DbManager = navParams.DbManager,
                    Receivable = ViewModel.NavParams.Receivable,
                    SelectedYear = year,
                    SelectedMonthChanged = navParams.SelectedMonthChanged
                };
                this.Frame.Navigate(typeof(MonthlyNavPage), newNavParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
        }
        
        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            FrameHelper.TryGoBack(this.Frame);
        }
    }
}
