using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

using AccountingManager.Core.Models;
using AccountingManager.Helpers;
using AccountingManager.Helpers.NavParams;
using AccountingManager.ViewModels;

namespace AccountingManager.Views {
    public sealed partial class DetailAccountingDataListPage : Page {
        private DetailAccountingDataListViewModel ViewModel => DataContext as DetailAccountingDataListViewModel;

        public DetailAccountingDataListPage() {
            InitializeComponent();
        }        

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            DetailAccountingDataListNavParams navParams = e.Parameter as DetailAccountingDataListNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;

                BindingOperations.SetBinding(DummyDataTypeColWidth, ColumnDefinition.WidthProperty, navParams.DataTypeColWidthBinding);
                BindingOperations.SetBinding(DummyClientNameColWidth, ColumnDefinition.WidthProperty, navParams.ClientNameColWidthBinding);
                BindingOperations.SetBinding(DummyDateColWidth, ColumnDefinition.WidthProperty, navParams.DateColWidthBinding);
                BindingOperations.SetBinding(DummySteelWeightColWidth, ColumnDefinition.WidthProperty, navParams.SteelWeightColWidthBinding);
                BindingOperations.SetBinding(DummySupplyPriceColWidth, ColumnDefinition.WidthProperty, navParams.SupplyPriceColWidthBinding);
                BindingOperations.SetBinding(DummyTaxAmountColWidth, ColumnDefinition.WidthProperty, navParams.TaxAmountColWidthBinding);
                BindingOperations.SetBinding(DummySumColWidth, ColumnDefinition.WidthProperty, navParams.SumColWidthBinding);
                BindingOperations.SetBinding(DummyDepositConfirmedColWidth, ColumnDefinition.WidthProperty, navParams.DepositConfirmedColWidthBinding);
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AccountingData data = AccountingDataListBox.SelectedItem as AccountingData;
            if (data != null) ViewModel.NavParams.SelectedAccountingDataChanged(data);
        }
    }
}
