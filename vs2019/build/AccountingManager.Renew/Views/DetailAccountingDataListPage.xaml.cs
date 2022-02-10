using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.Helpers.NavParams;
using AccountingManager.Renew.ViewModels;

namespace AccountingManager.Renew.Views {
    public sealed partial class DetailAccountingDataListPage : Page {
        private DetailAccountingDataListViewModel ViewModel => DataContext as DetailAccountingDataListViewModel;

        public DetailAccountingDataListPage() {
            InitializeComponent();
        }

        private void GenerateList() {
            foreach (AccountingData data in ViewModel.NavParams.AccountingData) {
                ColumnDefinition typeCol = new ColumnDefinition();                
                BindingOperations.SetBinding(typeCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.DataTypeColWidthBinding);

                ColumnDefinition nameCol = new ColumnDefinition();
                BindingOperations.SetBinding(nameCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.ClientNameColWidthBinding);

                ColumnDefinition dateCol = new ColumnDefinition();
                BindingOperations.SetBinding(dateCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.DateColWidthBinding);

                ColumnDefinition weightCol = new ColumnDefinition();
                BindingOperations.SetBinding(weightCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.SteelWeightColWidthBinding);

                ColumnDefinition priceCol = new ColumnDefinition();
                BindingOperations.SetBinding(priceCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.SupplyPriceColWidthBinding);

                ColumnDefinition taxCol = new ColumnDefinition();
                BindingOperations.SetBinding(taxCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.TaxAmountColWidthBinding);

                ColumnDefinition sumCol = new ColumnDefinition();
                BindingOperations.SetBinding(sumCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.SumColWidthBinding);

                ColumnDefinition checkCol = new ColumnDefinition();
                BindingOperations.SetBinding(checkCol, ColumnDefinition.WidthProperty, ViewModel.NavParams.DepositConfirmedColWidthBinding);

                Grid grid = new Grid();
                grid.Name = data.Uid.ToString();
                grid.ColumnDefinitions.Add(typeCol);
                grid.ColumnDefinitions.Add(nameCol);
                grid.ColumnDefinitions.Add(dateCol);
                grid.ColumnDefinitions.Add(weightCol);
                grid.ColumnDefinitions.Add(priceCol);
                grid.ColumnDefinitions.Add(taxCol);
                grid.ColumnDefinitions.Add(sumCol);
                grid.ColumnDefinitions.Add(checkCol);
                grid.Background = new SolidColorBrush(Colors.Transparent);

                Thickness textBlockMargin = new Thickness(15, 0, 0, 0);
                Thickness integerTextBlockMargin = new Thickness(0, 0, 15, 0);

                TextBlock typeTextBlock = new TextBlock();
                typeTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                typeTextBlock.VerticalAlignment = VerticalAlignment.Center;
                typeTextBlock.Text = data.DataType ? "매입" : "매출";

                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                nameTextBlock.Margin = textBlockMargin;
                nameTextBlock.Text = data.ClientName;

                TextBlock dateTextBlock = new TextBlock();
                dateTextBlock.VerticalAlignment = VerticalAlignment.Center;
                dateTextBlock.Margin = textBlockMargin;
                dateTextBlock.Text = new DateTime(data.Year, data.Month, data.Day).ToString("yyyy/MM/dd");

                TextBlock weightTextBlock = new TextBlock();
                weightTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                weightTextBlock.VerticalAlignment = VerticalAlignment.Center;
                weightTextBlock.Margin = integerTextBlockMargin;
                weightTextBlock.Text = string.Format("{0:#,##0.##}", data.SteelWeight);

                TextBlock priceTextBlock = new TextBlock();
                priceTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                priceTextBlock.VerticalAlignment = VerticalAlignment.Center;
                priceTextBlock.Margin = integerTextBlockMargin;
                priceTextBlock.Text = string.Format("{0:#,##0}", data.SupplyPrice);

                TextBlock taxTextBlock = new TextBlock();
                taxTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                taxTextBlock.VerticalAlignment = VerticalAlignment.Center;
                taxTextBlock.Margin = integerTextBlockMargin;
                taxTextBlock.Text = string.Format("{0:#,##0}", data.TaxAmount);

                TextBlock sumTextBlock = new TextBlock();
                sumTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                sumTextBlock.VerticalAlignment = VerticalAlignment.Center;
                sumTextBlock.Margin = integerTextBlockMargin;
                sumTextBlock.Text = string.Format("{0:#,##0}", data.SupplyPrice + data.TaxAmount);

                CheckBox checkBox = new CheckBox();
                checkBox.MinWidth = 0;
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                checkBox.Margin = new Thickness(15, 0, 0, 0);
                checkBox.Padding = new Thickness(10, 0, 0, 0);
                checkBox.IsChecked = data.DepositConfirmed;
                checkBox.IsEnabled = false;
                checkBox.Content = data.DepositConfirmed ? data.DepositDate : "";

                grid.Children.Add(typeTextBlock);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(dateTextBlock);
                grid.Children.Add(weightTextBlock);
                grid.Children.Add(priceTextBlock);
                grid.Children.Add(taxTextBlock);
                grid.Children.Add(sumTextBlock);
                grid.Children.Add(checkBox);

                Grid.SetColumn(typeTextBlock, 0);
                Grid.SetColumn(nameTextBlock, 1);
                Grid.SetColumn(dateTextBlock, 2);
                Grid.SetColumn(weightTextBlock, 3);
                Grid.SetColumn(priceTextBlock, 4);
                Grid.SetColumn(taxTextBlock, 5);
                Grid.SetColumn(sumTextBlock, 6);
                Grid.SetColumn(checkBox, 7);

                AccountingDataListBox.Items.Add(grid);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            DetailAccountingDataListNavParams navParams = e.Parameter as DetailAccountingDataListNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;

                GenerateList();
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ListBox listBox = sender as ListBox;
            Grid grid = listBox.SelectedItem as Grid;

            string idText = grid.Name;

            uint uid;
            uint.TryParse(idText, out uid);

            TextBlock dataTypeTextBlock = grid.Children[0] as TextBlock;
            bool dataType = dataTypeTextBlock.Text == "매입" ? true : false;

            TextBlock clientNameTextBlock = grid.Children[1] as TextBlock;
            string clientName = clientNameTextBlock.Text;

            TextBlock dateTextBlock = grid.Children[2] as TextBlock;
            DateTime date = Convert.ToDateTime(dateTextBlock.Text);

            TextBlock steelWeightTextBlock = grid.Children[3] as TextBlock;
            string steelWeightText = steelWeightTextBlock.Text.Replace(",", "");
            uint steelWeight;
            uint.TryParse(steelWeightText, out steelWeight);

            TextBlock supplyPriceTextBlock = grid.Children[4] as TextBlock;
            string supplyPriceText = supplyPriceTextBlock.Text.Replace(",", "");
            uint supplyPrice;
            uint.TryParse(supplyPriceText, out supplyPrice);

            TextBlock taxAmountTextBlock = grid.Children[5] as TextBlock;
            string taxAmountText = taxAmountTextBlock.Text.Replace(",", "");
            uint taxAmount;
            uint.TryParse(taxAmountText, out taxAmount);

            CheckBox checkBox = grid.Children[7] as CheckBox;
            bool depositConfirmed = (bool)checkBox.IsChecked;
            string depositDate = checkBox.Content as string;

            AccountingData data = new AccountingData {
                Uid = uid,
                ClientName = clientName,
                Year = date.Year,
                Month = date.Month,
                Day = date.Day,
                SteelWeight = steelWeight,
                SupplyPrice = supplyPrice,
                TaxAmount = taxAmount,
                DataType = dataType,
                DepositConfirmed = depositConfirmed,
                DepositDate = depositDate
            };

            ViewModel.NavParams.SelectedAccountingDataChanged(data);
        }
    }
}
