using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core.Preview;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

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

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CleanUp;
        }

        private async Task Initialize()
        {
            Task initTask = Logger.StaticInit();

            ViewModel.Initialize();

            SetWindowSize();
            SetTitleBarColor();

            await initTask;
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewModel.CleanUp();
        }

        private void SetWindowSize()
        {
            // For compatibility with Windows content size options.
            double scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            ApplicationView.PreferredLaunchViewSize = new Size(ViewModel.WindowWidth * scaleFactor, ViewModel.WindowHeight * scaleFactor);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private void SetTitleBarColor()
        {
            // Now this app doesn't have a title bar.
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            //
            // Set title bar and system menu buttons color.
            //
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonForegroundColor = Colors.Gray;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;

            titleBar.ButtonPressedForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = Colors.Transparent;

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

                Grid.SetColumn(typeTextBlock,   ViewModel.DataTypeTextBlockColumn);
                Grid.SetColumn(nameTextBlock,   ViewModel.ClientNameTextBlockColumn);
                Grid.SetColumn(dateTextBlock,   ViewModel.DateTextBlockColumn);
                Grid.SetColumn(weightTextBlock, ViewModel.SteelWeightTextBlockColumn);
                Grid.SetColumn(priceTextBlock,  ViewModel.SupplyPriceTextBlockColumn);
                Grid.SetColumn(taxTextBlock,    ViewModel.TaxAmountTextBlockColumn);
                Grid.SetColumn(sumTextBlock,    ViewModel.SumTextBlockColumn);
                Grid.SetColumn(checkBox,        ViewModel.DepositConfirmBlockColumn);

                AccountingDataListBox.Items.Add(grid);
            }            
        }

        private void SortAccountingDataListBox(bool reverseOrder = false)
        {
            List<AccountingData> sorted = ViewModel.AccountingDataList.ToList();
            sorted.Sort(ViewModel.AccountingDataComparision);

            GenerateAccountingDataListBox(sorted, reverseOrder);
        }

        private async Task ShowAlertDialog(string inMsg)
        {
            AlertDialog alertDialog = new AlertDialog(inMsg);
            await alertDialog.ShowAsync();

            alertDialog = null;
        }

        private async Task GenerateYearComboBox()
        {
            YearComboBox.Items.Clear();

            List<string> tableNameList = new List<string>();

            bool gettingNamesResult = await ViewModel.SqlManager.GetTableNamesLikeAsync("Test", "year%", tableNameList);
            if (gettingNamesResult)
            {
                foreach (string tableName in tableNameList)
                {
                    bool emptyResult = await ViewModel.SqlManager.TableNotEmptyAsync(tableName);
                    if (emptyResult)
                    {
                        string yearText = tableName.Substring(5, 4);

                        YearComboBox.Items.Add(yearText);
                    }
                }
            }
        }

        private async Task GenerateAccountingDataListView(string inYearText)
        {
            // First, clear data in the list.
            ViewModel.AccountingDataList.Clear();

            string tableName = "year_" + inYearText;

            // Load accounting data.
            bool gettingResult = await ViewModel.SqlManager.GetDataAsync(tableName, ViewModel.AccountingDataList);
            if (!gettingResult)
            {
                await ShowAlertDialog("DB 데이터 불러오기 실패");
                return;
            }
        }

        private async Task NavigateDateNavFrame(string inYearText)
        {
            string tableName = "year_" + inYearText;
            List<string> dateList = new List<string>();

            bool gettingResult = await ViewModel.SqlManager.GetDistinctDateDataAsync(tableName, dateList);
            if (!gettingResult)
            {
                await ShowAlertDialog("DB 자료 불러오기 실패");
                return;
            }

            SortedSet<int> monthSet = new SortedSet<int>();
            foreach (string date in dateList)
            {
                string monthText = date.Substring(5, 2);

                int month;
                int.TryParse(monthText, out month);

                monthSet.Add(month);
            }
            
            DateNavPageParams navParams = new DateNavPageParams();
            navParams.YearText = inYearText;
            navParams.MonthSet = monthSet;
            navParams.DateLitView_SelectionChanged = DateLitView_SelectionChanged;

            DateNavFrame.Navigate(typeof(DateNavPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private async void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Initialize();

            bool connecitonResult = await ViewModel.SqlManager.ConnectToDBAsync("192.168.0.4", 4885, "kbg", "@mDB901901@");
            if (!connecitonResult)
            {
                await ShowAlertDialog("DB 연결 실패");
                return;
            }

            bool useResult = await ViewModel.SqlManager.UseDatabaseAsync("Test");
            if (!useResult)
            {
                await ShowAlertDialog("DB 참조 실패");
                return;
            }

            await GenerateYearComboBox();
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

        private async Task AddDataToDB(string inYearText, int inId, string inClientName, string inDate, int inWeight, int inPrice, int inTax, bool inDataType, bool inConfirm)
        {
            //
            // If there is no table that is nominated by user, Create new one.
            // And insert data there.
            //
            string tableName = "year_" + inYearText.ToString();

            bool containResult = await ViewModel.SqlManager.DatabaseContainsAsync("Test", tableName);
            if (!containResult)
            {
                bool creationResult = await ViewModel.SqlManager.CreateTableAsync(tableName);
                if (!creationResult)
                {
                    await ShowAlertDialog("테이블 생성 실패");
                    return;
                }
            }

            bool insertionResult = await ViewModel.SqlManager.InsertDataAsync(tableName, inId, inClientName, inDate, inWeight, inPrice, inTax, inDataType, inConfirm);
            if (!insertionResult)
                await ShowAlertDialog("자료 생성 실패");
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

            // Handled when the add button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                string clientName = controls.InputName.Text;

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

                string tableName = "year_" + year;
                int id = await ViewModel.SqlManager.GetLastIdInTableAsync(tableName) + 1;

                if (ViewModel.SelectedYear == year)
                {
                    Task additionTask = AddDataToDB(year.ToString(), id, clientName, date, weight, price, tax, dataType, confirm);

                    ViewModel.AddAccountingData(id, clientName, date, weight, price, tax, dataType, confirm);
                
                    SortAccountingDataListBox();

                    await additionTask;
                }
                else
                {
                    await AddDataToDB(year.ToString(), id, clientName, date, weight, price, tax, dataType, confirm);
                    await GenerateYearComboBox();
                }
            }
        }

        private async Task EditDataInDBAsync(string inYearText, AccountingData inData, AccountingData.ChangedData inChangedData)
        {
            string tableName = "year_" + inYearText;

            bool updateResult = await ViewModel.SqlManager.UpdateDataAsync(tableName, inData, inChangedData);
            if (!updateResult) await ShowAlertDialog("자료 수정 실패");
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //
            // Get the id of the accounting data.
            //
            Grid grid = AccountingDataListBox.SelectedItem as Grid;
            if (grid == null)
            {
                await ShowAlertDialog("수정할 자료를 선택해주십시오.");
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

            // Handled when the edit button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                //
                // Configure date after extract year, month, and day.
                //
                year = (int)controls.InputYear.SelectedItem;
                month = (int)controls.InputMonth.SelectedItem;
                day = (int)controls.InputDay.SelectedItem;

                string date = year + "/" + string.Format("{0:D2}", month) + "/" + string.Format("{0:D2}", day);

                //
                // Parse the text in TextBox to an integer.
                //
                int steelWeight;
                int.TryParse(controls.InputWeight.Text, out steelWeight);

                int supplyPrice;
                int.TryParse(controls.InputPrice.Text, out supplyPrice);

                int taxAmount;
                int.TryParse(controls.InputTax.Text, out taxAmount);

                bool dataType = controls.InputType.SelectedIndex == 0 ? true : false;
                bool confirmed = (bool)controls.InputConfirm.IsChecked;

                //
                // Specify the changed data.
                //
                AccountingData.ChangedData changedData = AccountingData.ChangedData.ENone;

                if (data.ClientName     != controls.InputName.Text) changedData |= AccountingData.ChangedData.EClientName;
                if (data.Date           != date)                    changedData |= AccountingData.ChangedData.EDate;
                if (data.SteelWeight    != steelWeight)             changedData |= AccountingData.ChangedData.ESteelWeight;
                if (data.SupplyPrice    != supplyPrice)             changedData |= AccountingData.ChangedData.ESupplyPrice;
                if (data.TaxAmount      != taxAmount)               changedData |= AccountingData.ChangedData.ETaxAmount;
                if (data.DataType       != dataType)                changedData |= AccountingData.ChangedData.EDataType;
                if (data.DepositConfirm != confirmed)               changedData |= AccountingData.ChangedData.EDepositConfirm;

                //
                // Insert the changed data.
                //
                data.ClientName     = controls.InputName.Text;
                data.Date           = date;
                data.SteelWeight    = steelWeight;
                data.SupplyPrice    = supplyPrice;
                data.TaxAmount      = taxAmount;
                data.DataType       = dataType;
                data.DepositConfirm = confirmed;

                // Asynchronously edit data in db.
                Task editTask = EditDataInDBAsync(year.ToString(), data, changedData);

                SortAccountingDataListBox();

                await editTask;
            }
        }

        private async Task DeleteDataInDBAsync(Grid inGrid, int inId)
        {
            foreach (UIElement element in inGrid.Children)
            {
                TextBlock textBlock = element as TextBlock;
                if (textBlock == null) continue;

                if (Grid.GetColumn(textBlock) == ViewModel.DateTextBlockColumn)
                {
                    string yearText = textBlock.Text.Substring(0, 4);

                    int year;
                    int.TryParse(yearText, out year);

                    //
                    // Notify the DB that table has been deleted.
                    //
                    string tableName = "year_" + year;

                    bool deletionResult = await ViewModel.SqlManager.DeleteDataAsync(tableName, inId);
                    if (!deletionResult) await ShowAlertDialog("DB 내 자료 삭제 실패");

                    break;
                }
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
                await ShowAlertDialog("삭제할 자료를 선택해주십시오.");
                return;
            }

            int id;
            int.TryParse(grid.Name, out id);

            // Asynchronously delete data using id.
            Task deletionTask = DeleteDataInDBAsync(grid, id);

            // Remove the accounting data using the id.
            if (!ViewModel.RemoveAccountingData(id))
            {
                await ShowAlertDialog("클라이언트 내 자료 삭제 실패");

                return;
            }

            SortAccountingDataListBox();

            await deletionTask;
        }

        public void DateLitView_SelectionChanged(string inSelection)
        {
            InputText.Text = inSelection;
        }

        private async void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox)) return;
            ComboBox listBox = sender as ComboBox;

            if (listBox.SelectedIndex == -1) return;

            string yearText = listBox.SelectedItem as string;
            string tableName = "year_" + yearText;

            int year;
            int.TryParse(yearText, out year);

            ViewModel.SelectedYear = year;

            Task genTask = GenerateAccountingDataListView(yearText);
            Task navTask = NavigateDateNavFrame(yearText);

            await genTask;

            // Sort the list after loading accounting data from DB.
            SortAccountingDataListBox();

            await navTask;
        }
    }
}
