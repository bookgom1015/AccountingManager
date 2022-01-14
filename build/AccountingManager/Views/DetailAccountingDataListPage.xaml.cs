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
using AccountingManager.ViewModels;

namespace AccountingManager.Views
{
    public sealed partial class DetailAccountingDataListPage : Page
    {
        private DetailAccountingDataListViewModel ViewModel => DataContext as DetailAccountingDataListViewModel;

        public DetailAccountingDataListPage()
        {
            InitializeComponent();
        }

        private Binding CreateBinding(object source, string path, BindingMode mode, UpdateSourceTrigger trigger)
        {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = mode;
            binding.UpdateSourceTrigger = trigger;

            return binding;
        }

        private void GenerateAccountingDataListBox(DetailAccountingDataListPageParams inNavParams)
        {            
            if (inNavParams.Reversed) inNavParams.AccountingDataList.Reverse();
            
            foreach (AccountingData data in inNavParams.AccountingDataList)
            {
                ColumnDefinition typeCol = new ColumnDefinition();
                Binding typeColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "DataTypeColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(typeCol, ColumnDefinition.WidthProperty, typeColWidthBinding);
            
                ColumnDefinition nameCol = new ColumnDefinition();
                Binding nameColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "ClientNameColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(nameCol, ColumnDefinition.WidthProperty, nameColWidthBinding);
            
                ColumnDefinition dateCol = new ColumnDefinition();
                Binding dateColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "DateColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(dateCol, ColumnDefinition.WidthProperty, dateColWidthBinding);
            
                ColumnDefinition weightCol = new ColumnDefinition();
                Binding weightColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "SteelWeightColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(weightCol, ColumnDefinition.WidthProperty, weightColWidthBinding);
            
                ColumnDefinition priceCol = new ColumnDefinition();
                Binding priceColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "SupplyPriceColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(priceCol, ColumnDefinition.WidthProperty, priceColWidthBinding);
            
                ColumnDefinition taxCol = new ColumnDefinition();
                Binding taxColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "TaxAmountColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(taxCol, ColumnDefinition.WidthProperty, taxColWidthBinding);
            
                ColumnDefinition sumCol = new ColumnDefinition();
                Binding sumColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "SumColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(sumCol, ColumnDefinition.WidthProperty, sumColWidthBinding);
            
                ColumnDefinition checkCol = new ColumnDefinition();
                Binding checkColWidthBinding = CreateBinding(inNavParams.ParentViewModel, "DepositConfirmColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(checkCol, ColumnDefinition.WidthProperty, checkColWidthBinding);
            
                Grid grid = new Grid();
                grid.Name = data.Id.ToString();
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
                dateTextBlock.Text = data.Date;
            
                TextBlock weightTextBlock = new TextBlock();
                weightTextBlock.VerticalAlignment = VerticalAlignment.Center;
                weightTextBlock.Margin = textBlockMargin;
                weightTextBlock.Text = string.Format("{0:#,###}", data.SteelWeight);
            
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
                checkBox.MinWidth = 0;
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.VerticalContentAlignment = VerticalAlignment.Center;
                checkBox.Margin = new Thickness(15, 0, 0, 0);
                checkBox.Padding = new Thickness(0);
                checkBox.IsChecked = data.DepositConfirm;
                checkBox.IsEnabled = false;
                checkBox.Content = data.DepositConfirm ? "  확인됨" : "";
            
                grid.Children.Add(typeTextBlock);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(dateTextBlock);
                grid.Children.Add(weightTextBlock);
                grid.Children.Add(priceTextBlock);
                grid.Children.Add(taxTextBlock);
                grid.Children.Add(sumTextBlock);
                grid.Children.Add(checkBox);
            
                Grid.SetColumn(typeTextBlock,   0);
                Grid.SetColumn(nameTextBlock,   1);
                Grid.SetColumn(dateTextBlock,   2);
                Grid.SetColumn(weightTextBlock, 3);
                Grid.SetColumn(priceTextBlock,  4);
                Grid.SetColumn(taxTextBlock,    5);
                Grid.SetColumn(sumTextBlock,    6);
                Grid.SetColumn(checkBox,        7);
            
                AccountingDataListBox.Items.Add(grid);
            }            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DetailAccountingDataListPageParams navParams = e.Parameter as DetailAccountingDataListPageParams;
            if (navParams != null)
            {
                ViewModel.AccountingDataList_SelectionChagned = navParams.AccountingDataList_SelectionChagned;

                GenerateAccountingDataListBox(navParams);
            }

            base.OnNavigatedTo(e);
        }

        private void AccountingDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            Grid grid = listBox.SelectedItem as Grid;

            string idText = grid.Name;

            int id;
            int.TryParse(idText, out id);

            TextBlock dataTypeTextBlock = grid.Children[0] as TextBlock;
            bool dataType = dataTypeTextBlock.Text == "매입" ? true : false;

            TextBlock clientNameTextBlock = grid.Children[1] as TextBlock;
            string clientName = clientNameTextBlock.Text;

            TextBlock dateTextBlock = grid.Children[2] as TextBlock;
            string date = dateTextBlock.Text;

            TextBlock steelWeightTextBlock = grid.Children[3] as TextBlock;
            string steelWeightText = steelWeightTextBlock.Text.Replace(",", "");
            int steelWeight;
            int.TryParse(steelWeightText, out steelWeight);

            TextBlock supplyPriceTextBlock = grid.Children[4] as TextBlock;
            string supplyPriceText = supplyPriceTextBlock.Text.Replace(",", "");
            int supplyPrice;
            int.TryParse(supplyPriceText, out supplyPrice);

            TextBlock taxAmountTextBlock = grid.Children[5] as TextBlock;
            string taxAmountText = taxAmountTextBlock.Text.Replace(",", "");
            int taxAmount;
            int.TryParse(taxAmountText, out taxAmount);

            CheckBox checkBox = grid.Children[7] as CheckBox;
            bool depositConfirm = (bool)checkBox.IsChecked;

            ViewModel.AccountingDataList_SelectionChagned(id, clientName, date, steelWeight, supplyPrice, taxAmount, dataType, depositConfirm);
        }
    }
}
