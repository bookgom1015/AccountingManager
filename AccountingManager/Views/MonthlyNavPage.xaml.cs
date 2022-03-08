using System;
using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AccountingManager.Core.Infrastructures;
using AccountingManager.Dialogs;
using AccountingManager.Helpers;
using AccountingManager.Helpers.NavParams;
using AccountingManager.ViewModels;

namespace AccountingManager.Views {
    public sealed partial class MonthlyNavPage : Page {
        private MonthlyNavViewModel ViewModel => DataContext as MonthlyNavViewModel;

        public MonthlyNavPage() {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            MonthlyNavParams navParams = e.Parameter as MonthlyNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;

                NavTitle.Text = string.Format("{0}년", navParams.SelectedYear.ToString());

                IEnumerable<int> months;
                Result result = navParams.DbManager.GetDates(out months, navParams.SelectedYear, null, navParams.Receivable);
                if (result.Status) {
                    foreach (int month in months)
                        MonthListBox.Items.Add(month);
                }
                else {
                    MessageDialog dialog = new MessageDialog { Title = "오류", Message = result.ErrMsg };
                    await dialog.ShowAsync();
                }
            }

            base.OnNavigatedTo(e);
        }

        private void MonthListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MonthListBox.SelectedItem is int) {
                int month = (int)MonthListBox.SelectedItem;

                ViewModel.NavParams.SelectedMonthChanged(month);
            }
        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            FrameHelper.TryGoBack(this.Frame);
        }
    }
}
