using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

using AccountingManager.Core.Models;
using AccountingManager.Dialogs;
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

            DateNavFrame.Navigate(typeof(YearlyNavPage));
        }

        private Binding CreateBinding(object source, string path, BindingMode mode, UpdateSourceTrigger trigger)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.OneWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            return binding;
        }

        private void GenerateDateListBox(List<AccountingData> dataList, bool reverse = false)
        {
            if (reverse) dataList.Reverse();

            foreach (AccountingData data in dataList)
            {
                ColumnDefinition typeCol = new ColumnDefinition();
                Binding typeColWidthBinding = CreateBinding(ViewModel, "TypeColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(typeCol, ColumnDefinition.WidthProperty, typeColWidthBinding);

                ColumnDefinition nameCol = new ColumnDefinition();
                Binding nameColWidthBinding = CreateBinding(ViewModel, "NameColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(nameCol, ColumnDefinition.WidthProperty, nameColWidthBinding);

                ColumnDefinition dateCol = new ColumnDefinition();
                Binding dateColWidthBinding = CreateBinding(ViewModel, "DateColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(dateCol, ColumnDefinition.WidthProperty, dateColWidthBinding);

                ColumnDefinition priceCol = new ColumnDefinition();
                Binding priceColWidthBinding = CreateBinding(ViewModel, "PriceColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(priceCol, ColumnDefinition.WidthProperty, priceColWidthBinding);

                ColumnDefinition taxCol = new ColumnDefinition();
                Binding taxColWidthBinding = CreateBinding(ViewModel, "TaxColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(taxCol, ColumnDefinition.WidthProperty, taxColWidthBinding);

                ColumnDefinition sumCol = new ColumnDefinition();
                Binding sumColWidthBinding = CreateBinding(ViewModel, "SumColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(sumCol, ColumnDefinition.WidthProperty, sumColWidthBinding);

                ColumnDefinition checkCol = new ColumnDefinition();
                Binding checkColWidthBinding = CreateBinding(ViewModel, "CheckColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(checkCol, ColumnDefinition.WidthProperty, checkColWidthBinding);

                Grid grid = new Grid();
                grid.Background = new SolidColorBrush(Colors.Transparent);
                grid.ColumnDefinitions.Add(typeCol);
                grid.ColumnDefinitions.Add(nameCol);
                grid.ColumnDefinitions.Add(dateCol);
                grid.ColumnDefinitions.Add(priceCol);
                grid.ColumnDefinitions.Add(taxCol);
                grid.ColumnDefinitions.Add(sumCol);
                grid.ColumnDefinitions.Add(checkCol);

                Thickness textBlockMargin = new Thickness(15, 0, 0, 0);

                TextBlock typeTextBlock = new TextBlock();
                typeTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                typeTextBlock.VerticalAlignment = VerticalAlignment.Center;
                typeTextBlock.Text = data.DataType ? "매출" : "매입";

                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                nameTextBlock.Margin = textBlockMargin;
                nameTextBlock.Text = data.ClientName;

                TextBlock dateTextBlock = new TextBlock();
                dateTextBlock.VerticalAlignment = VerticalAlignment.Center;
                dateTextBlock.Margin = textBlockMargin;
                dateTextBlock.Text = data.Date;

                TextBlock priceTextBlock = new TextBlock();
                priceTextBlock.VerticalAlignment = VerticalAlignment.Center;
                priceTextBlock.Margin = textBlockMargin;
                priceTextBlock.Text = string.Format("{0:#,###}", data.SupplyPrice);

                TextBlock taxTextBlock = new TextBlock();
                taxTextBlock.VerticalAlignment = VerticalAlignment.Center;
                taxTextBlock.Margin = textBlockMargin;
                taxTextBlock.Text = string.Format("{0:#,###}", data.TaxAmount);

                TextBlock sumTextBlock = new TextBlock();
                sumTextBlock.VerticalAlignment = VerticalAlignment.Center;
                sumTextBlock.Margin = textBlockMargin;
                sumTextBlock.Text = string.Format("{0:#,###}", data.SupplyPrice + data.TaxAmount);

                CheckBox checkBox = new CheckBox();
                checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.MinWidth = ViewModel.DefaultMinColumnWidth;
                checkBox.IsChecked = data.DepositConfirm;
                checkBox.IsEnabled = false;
                checkBox.Content = data.DepositConfirm ? "확인됨" : "";

                grid.Children.Add(typeTextBlock);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(dateTextBlock);
                grid.Children.Add(priceTextBlock);
                grid.Children.Add(taxTextBlock);
                grid.Children.Add(sumTextBlock);
                grid.Children.Add(checkBox);

                Grid.SetColumn(typeTextBlock, 0);
                Grid.SetColumn(nameTextBlock, 1);
                Grid.SetColumn(dateTextBlock, 2);
                Grid.SetColumn(priceTextBlock, 3);
                Grid.SetColumn(taxTextBlock, 4);
                Grid.SetColumn(sumTextBlock, 5);
                Grid.SetColumn(checkBox, 6);

                AccountingDataListBox.Items.Add(grid);
            }            
        }

        private void AccountingDataListBox_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateDateListBox(ViewModel.AccountingDataList);
        }

        private void SortAccountingDataListBox(Comparison<AccountingData> comparison)
        {
            ViewModel.ColumnOrder = !ViewModel.ColumnOrder;

            AccountingDataListBox.Items.Clear();

            List<AccountingData> sorted = ViewModel.AccountingDataList;
            sorted.Sort(comparison);

            GenerateDateListBox(sorted, ViewModel.ColumnOrder);
        }

        private int CompareType(AccountingData a, AccountingData b)
        {
            if (a.DataType == b.DataType)
                return 0;
            else if (a.DataType && !b.DataType)
                return 1;
            else
                return -1;
        }

        private void TypeColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareType);
        }

        private int CompareName(AccountingData a, AccountingData b)
        {
            return string.Compare(a.ClientName, b.ClientName);
        }

        private void NameColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareName);
        }

        private int CompareDate(AccountingData a, AccountingData b)
        {
            return string.Compare(a.Date, b.Date);
        }

        private void DateColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareDate);
        }

        private int ComparePrice(AccountingData a, AccountingData b)
        {
            if (a.SupplyPrice > b.SupplyPrice)
                return 1;
            else if (a.SupplyPrice == b.SupplyPrice)
                return 0;
            else
                return -1;
        }

        private void PriceColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(ComparePrice);
        }

        private int CompareTax(AccountingData a, AccountingData b)
        {
            if (a.TaxAmount > b.TaxAmount)
                return 1;
            else if (a.TaxAmount == b.TaxAmount)
                return 0;
            else
                return -1;
        }

        private void TaxColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareTax);
        }

        private int CompareSum(AccountingData a, AccountingData b)
        {
            int sumA = a.SupplyPrice + a.TaxAmount;
            int sumB = b.SupplyPrice + b.TaxAmount;
            if (sumA > sumB)
                return 1;
            else if (sumA == sumB)
                return 0;
            else
                return -1;
        }

        private void SumColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareSum);
        }

        private int CompareCheck(AccountingData a, AccountingData b)
        {
            if (a.DepositConfirm == b.DepositConfirm)
                return 0;
            else if (a.DepositConfirm && !b.DepositConfirm)
                return 1;
            else
                return -1;
        }

        private void CheckColumnButton_Click(object sender, RoutedEventArgs e)
        {
            SortAccountingDataListBox(CompareCheck);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AccountingDataControls controls = new AccountingDataControls();

            AddDialog dialog = new AddDialog(controls);

             ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                InputText.Text = controls.InputName.Text;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
