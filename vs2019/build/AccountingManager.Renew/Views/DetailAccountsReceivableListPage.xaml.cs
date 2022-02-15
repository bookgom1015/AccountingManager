using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Helpers.NavParams;
using AccountingManager.Renew.ViewModels;

namespace AccountingManager.Renew.Views {
    public sealed partial class DetailAccountsReceivableListPage : Page {
        private DetailAccountsReceivableListViewModel ViewModel => DataContext as DetailAccountsReceivableListViewModel;

        public DetailAccountsReceivableListPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            DetailAccountsReceivableListNavParams navParams = e.Parameter as DetailAccountsReceivableListNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;

                BindingOperations.SetBinding(DummyDataTypeColWidth, ColumnDefinition.WidthProperty, navParams.DataTypeColWidthBinding);
                BindingOperations.SetBinding(DummyClientNameColWidth, ColumnDefinition.WidthProperty, navParams.ClientNameColWidthBinding);
                BindingOperations.SetBinding(DummyDateColWidth, ColumnDefinition.WidthProperty, navParams.DateColWidthBinding);
                BindingOperations.SetBinding(DummySumColWidth, ColumnDefinition.WidthProperty, navParams.SumColWidthBinding);
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AccountingData data = AccountingDataListBox.SelectedItem as AccountingData;
            if (data != null) ViewModel.NavParams.SelectedAccountingDataChanged(data);
        }
    }
}
