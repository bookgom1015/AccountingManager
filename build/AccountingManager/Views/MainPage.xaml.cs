using System;
using System.Collections.Generic;
using System.Linq;

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

        private void GenerateAccountingDataListBox(List<AccountingData> dataList, bool reverse = false)
        {
            AccountingDataListBox.Items.Clear();

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

                ColumnDefinition weightCol = new ColumnDefinition();
                Binding weightColWidthBinding = CreateBinding(ViewModel, "WeightColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
                BindingOperations.SetBinding(weightCol, ColumnDefinition.WidthProperty, weightColWidthBinding);

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
                checkBox.HorizontalAlignment = HorizontalAlignment.Center;
                checkBox.VerticalAlignment = VerticalAlignment.Center;
                checkBox.MinWidth = ViewModel.DefaultMinColumnWidth;
                checkBox.IsChecked = data.DepositConfirm;
                checkBox.IsEnabled = false;
                checkBox.Content = data.DepositConfirm ? "확인됨" : "";

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

        private void SortAccountingDataListBox(bool reverseOrder = false)
        {
            List<AccountingData> sorted = ViewModel.AccountingDataList.ToList();
            sorted.Sort(ViewModel.AccountingDataComparision);

            GenerateAccountingDataListBox(sorted, reverseOrder);

            sorted = null;
        }

        private void AccountingDataListBox_Loaded(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();

            for (int i = 0; i < 5; ++i)
            {
                int weight = rand.Next(0, 99999);
                int price = rand.Next(0, 100) * weight;
                int tax = (int)(price * 0.026);
                int sum = price + tax;
                bool isEven = i % 2 == 0;
                
                ViewModel.AddAccountingData("Client Name " + i.ToString(), "2021/12/21", weight, price, tax, !isEven, isEven);
            }

            rand = null;

            SortAccountingDataListBox();
        }

        private void TypeColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareType)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareType;
                ViewModel.Orders.TypeOrder = false;
            }
            else
            {
                ViewModel.Orders.TypeOrder = !ViewModel.Orders.TypeOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.TypeOrder);
        }

        private void NameColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareName)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareName;
                ViewModel.Orders.NameOrder = false;
            }
            else
            {
                ViewModel.Orders.NameOrder = !ViewModel.Orders.NameOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.NameOrder);
        }

        private void DateColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareDate)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareDate;
                ViewModel.Orders.DateOrder = false;
            }
            else
            {
                ViewModel.Orders.DateOrder = !ViewModel.Orders.DateOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.DateOrder);
        }

        private void WeightColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareWeight)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareWeight;
                ViewModel.Orders.WeightOrder = false;
            }
            else
            {
                ViewModel.Orders.WeightOrder = !ViewModel.Orders.WeightOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.WeightOrder);
        }

        private void PriceColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.ComparePrice)
            {
                ViewModel.AccountingDataComparision = Comparisions.ComparePrice;
                ViewModel.Orders.PriceOrder = false;
            }
            else
            {
                ViewModel.Orders.PriceOrder = !ViewModel.Orders.PriceOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.PriceOrder);
        }

        private void TaxColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareTax)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareTax;
                ViewModel.Orders.TaxOrder = false;
            }
            else
            {
                ViewModel.Orders.TaxOrder = !ViewModel.Orders.TaxOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.TaxOrder);
        }

        private void SumColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareSum)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareSum;
                ViewModel.Orders.SumOrder = false;
            }
            else
            {
                ViewModel.Orders.SumOrder = !ViewModel.Orders.SumOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.SumOrder);
        }

        private void ConfirmColumnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AccountingDataComparision != Comparisions.CompareConfirm)
            {
                ViewModel.AccountingDataComparision = Comparisions.CompareConfirm;
                ViewModel.Orders.ConfirmOrder = false;
            }
            else
            {
                ViewModel.Orders.ConfirmOrder = !ViewModel.Orders.ConfirmOrder;
            }

            SortAccountingDataListBox(ViewModel.Orders.ConfirmOrder);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Generate controls used by ContentDialog.
            AccountingDataControls controls = new AccountingDataControls();

            AddDialog dialog = new AddDialog(controls);
            dialog.Title = "매출입 입력";
            dialog.PrimaryButtonText = "추가";
            dialog.SecondaryButtonText = "취소";

            ContentDialogResult result = await dialog.ShowAsync();
            dialog = null;

            // Handled when the add button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                string name = controls.InputName.Text;

                int year = (int)controls.InputYear.SelectedItem;
                int month = (int)controls.InputMonth.SelectedItem;
                int day = (int)controls.InputDay.SelectedItem;

                string date = year + "/" + string.Format("{0:D2}", month) + "/" + string.Format("{0:D2}", day);

                int weight;
                int.TryParse(controls.InputWeight.Text, out weight);

                int price;
                int.TryParse(controls.InputPrice.Text, out price);

                int tax;
                int.TryParse(controls.InputTax.Text, out tax);

                bool dataType = controls.InputType.SelectedIndex == 0 ? true : false;
                bool confirm = (bool)controls.InputConfirm.IsChecked;

                ViewModel.AddAccountingData(name, date, weight, price, tax, dataType, confirm);

                SortAccountingDataListBox();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //
            // Get the id of the accounting data.
            //
            Grid grid = AccountingDataListBox.SelectedItem as Grid;
            if (grid == null)
            {
                AlertDialog alertDialog = new AlertDialog("수정할 자료를 선택해주십시오.");
                await alertDialog.ShowAsync();

                alertDialog = null;

                return;
            }

            int id;
            int.TryParse(grid.Name, out id);

            // Get the data corresponding to the id.
            // (TODO) If failed, write log
            AccountingData data;
            if (!ViewModel.FindAccountingData(id, out data)) return;

            //
            // Configure variables for date.
            //
            int year;
            int.TryParse(data.Date.Substring(0, 4), out year);

            int month;
            int.TryParse(data.Date.Substring(5, 2), out month);

            int day;
            int.TryParse(data.Date.Substring(8, 2), out day);

            //
            // Generate the controls that is diplayed in the ContentDialog.
            //
            AccountingDataControls controls = new AccountingDataControls();
            controls.InputName.Text = data.ClientName;
            controls.InputDay.SelectedIndex = day - 1;
            controls.InputMonth.SelectedIndex = month - 1;
            controls.InputYear.SelectedIndex = year - 2000;
            controls.InputWeight.Text = data.SteelWeight.ToString();
            controls.InputPrice.Text = data.SupplyPrice.ToString();
            controls.InputTax.Text = data.TaxAmount.ToString();
            controls.InputType.SelectedIndex = data.DataType ? 0 : 1;
            controls.InputConfirm.IsChecked = data.DepositConfirm;

            //
            // Pop up ContentDialog.
            //
            AddDialog dialog = new AddDialog(controls);
            dialog.Title = "매출입 수정";
            dialog.PrimaryButtonText = "수정";
            dialog.SecondaryButtonText = "취소";

            ContentDialogResult result = await dialog.ShowAsync();
            dialog = null;

            // Handled when the edit button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                data.ClientName = controls.InputName.Text;

                year = (int)controls.InputYear.SelectedItem;
                month = (int)controls.InputMonth.SelectedItem;
                day = (int)controls.InputDay.SelectedItem;

                string date = year + "/" + string.Format("{0:D2}", month) + "/" + string.Format("{0:D2}", day);
                data.Date = date;

                int weight;
                int.TryParse(controls.InputWeight.Text, out weight);
                data.SteelWeight = weight;

                int price;
                int.TryParse(controls.InputPrice.Text, out price);
                data.SupplyPrice = price;

                int tax;
                int.TryParse(controls.InputTax.Text, out tax);
                data.TaxAmount = tax;

                data.DataType = controls.InputType.SelectedIndex == 0 ? true : false;
                data.DepositConfirm = (bool)controls.InputConfirm.IsChecked;

                SortAccountingDataListBox();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //
            // Get the id of the accounting data.
            //
            Grid grid = AccountingDataListBox.SelectedItem as Grid;
            if (grid == null)
            {
                AlertDialog alertDialog = new AlertDialog("삭제할 자료를 선택해주십시오.");
                await alertDialog.ShowAsync();

                alertDialog = null;

                return;
            }

            int id;
            int.TryParse(grid.Name, out id);

            // Remove the accounting data using the id.
            ViewModel.RemoveAccountingData(id);

            SortAccountingDataListBox();
        }
        
        private async void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            string errMsg;

            if (!ViewModel.SqlManager.ConnectToDB("192.168.0.4", 4885, "kbg", "@mDB901901@", out errMsg))
            {
                AlertDialog alertDialog = new AlertDialog("서버 연결을 실패했습니다. \r\n" + errMsg);
                await alertDialog.ShowAsync();

                alertDialog = null;
                return;
            }

            if (!ViewModel.SqlManager.UseDatabase("Test", out errMsg))
            {
                AlertDialog alertDialog = new AlertDialog("데이터베이스 참조를 실패했습니다. \r\n" + errMsg);
                await alertDialog.ShowAsync();

                alertDialog = null;
                return;
            }

            DateNavFrame.Navigate(typeof(YearlyNavPage));
        }
    }
}
