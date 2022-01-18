using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Core.Models;
using AccountingManager.Dialogs;
using AccountingManager.Helpers;
using AccountingManager.ViewModels;

namespace AccountingManager.Views
{
    public sealed partial class AccountingDataListPage : Page
    {
        private AccountingDataListViewModel ViewModel => DataContext as AccountingDataListViewModel;

        public AccountingDataListPage()
        {
            InitializeComponent();

            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += CleanUp;
        }

        private void CleanUp(Object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            ViewModel.CleanUp();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Initialization must be called first
            ViewModel.Initialize();

            //
            // After intialization.
            //
            AccountingDataListPageParams navParams = e.Parameter as AccountingDataListPageParams;
            if (navParams != null)
            {
                ViewModel.SqlManager = navParams.SqlManager;
                ViewModel.DatabaseName = navParams.DatabaseName;

                NavigateToYearlyNavPage();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.CleanUp();

            base.OnNavigatedFrom(e);
        }

        private async void NavigateToYearlyNavPage()
        {
            List<String> tableNameList = new List<string>();

            bool gettingNamesResult = await ViewModel.SqlManager.GetTableNamesLikeAsync(ViewModel.DatabaseName, "year%", tableNameList);
            if (gettingNamesResult)
            {
                List<int> yearList = new List<int>();

                foreach (string tableName in tableNameList)
                {
                    string yearText = tableName.Substring(5, 4);

                    int year;
                    int.TryParse(yearText, out year);

                    yearList.Add(year);
                }

                YearlyNavPageParams navParams = new YearlyNavPageParams(yearList, this.YearList_SelectionChanged);

                YearlyNavFrame.Navigate(typeof(YearlyNavPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
        }

        private async Task NavigateToAccountingDetailDataListPage([CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            ViewModel.SelectedData.Id = -1;

            try
            {
                ViewModel.AccountingDataList.Sort(ViewModel.CurrentComparision);
            }
            catch (ArgumentNullException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentNullException: " + e.Message + ";");
                return;
            }
            catch (ArgumentException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentException: " + e.Message + ";");
                return;
            }

            DetailAccountingDataListPageParams navParams = new DetailAccountingDataListPageParams(ViewModel, this.AccountingDataList_SelecitonChanged);

            DetailListFrame.Navigate(typeof(DetailAccountingDataListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void NavigateToAccountingDetailDataListPageWithNoParams()
        {
            InputTextBox.Text = "";
            ViewModel.YearSelected = false;
            ViewModel.SelectedData.Id = -1;

            DetailListFrame.Navigate(typeof(DetailAccountingDataListPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private async Task NavigateToAccountingDetailDataListPageUsingYear([CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            InputTextBox.Text = "";
            ViewModel.SelectedData.Id = -1;

            string talbeName = "year_" + ViewModel.SelectedYearText;
            ViewModel.AccountingDataList.Clear();

            bool gettingResult = await ViewModel.SqlManager.GetAccountingDataAsync(talbeName, AccountingData.QueryKeys.ENone, null, ViewModel.AccountingDataList);
            if (!gettingResult)
            {
                await Logger.ShowAlertDialog("DB 데이터 불러오기 실패");
                return;
            }

            try
            {
                ViewModel.AccountingDataList.Sort(ViewModel.CurrentComparision);
            }
            catch (ArgumentNullException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentNullException: " + e.Message + ";");
                return;
            }
            catch (ArgumentException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentException: " + e.Message + ";");
                return;
            }

            DetailAccountingDataListPageParams navParams = new DetailAccountingDataListPageParams(ViewModel, this.AccountingDataList_SelecitonChanged);

            DetailListFrame.Navigate(typeof(DetailAccountingDataListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private async Task NavigateToAccountingDetailDataListPageUsingDataList(List<AccountingData> inDataList, [CallerMemberName] string inMemberName = "", [CallerFilePath] string inFilePath = "", [CallerLineNumber] int inLineNumber = 0)
        {
            try
            {
                inDataList.Sort(ViewModel.CurrentComparision);
            }
            catch (ArgumentNullException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentNullException: " + e.Message + ";");
                return;
            }
            catch (ArgumentException e)
            {
                await Logger.Logln("File Path: " + inFilePath + "; Line Number: " + inLineNumber + "; ArgumentException: " + e.Message + ";");
                return;
            }

            ViewModel.AccountingDataList = null;
            ViewModel.AccountingDataList = inDataList;

            DetailAccountingDataListPageParams navParams = new DetailAccountingDataListPageParams(ViewModel, this.AccountingDataList_SelecitonChanged);

            DetailListFrame.Navigate(typeof(DetailAccountingDataListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        public async void YearList_SelectionChanged(string inYearText)
        {
            ViewModel.SelectedYearText = inYearText;

            await NavigateToAccountingDetailDataListPageUsingYear();

            ViewModel.YearSelected = true;
        }

        public void AccountingDataList_SelecitonChanged(int inId, string inClientName, string inDate, int inSteelWeight, int inSupplyPrice, int inTaxAmount, bool inDataType, bool inDepositConfirm)
        {
            ViewModel.SelectedData.Id = inId;
            ViewModel.SelectedData.ClientName = inClientName;
            ViewModel.SelectedData.Date = inDate;
            ViewModel.SelectedData.SteelWeight = inSteelWeight;
            ViewModel.SelectedData.SupplyPrice = inSupplyPrice;
            ViewModel.SelectedData.TaxAmount = inTaxAmount;
            ViewModel.SelectedData.DataType = inDataType;
            ViewModel.SelectedData.DepositConfirm = inDepositConfirm;
        }

        public async Task AddDataToDB(int inId, string inClientName, string inDate, int inSteelWeight, int inSupplyPrice, int inTaxAmount, bool inDataType, bool inDepositConfirm)
        {
            string tableName = "year_" + ViewModel.SelectedYearText;

            bool insertionResult = await ViewModel.SqlManager.InsertDataAsync(tableName, inId, inClientName, inDate, inSteelWeight, inSupplyPrice, inTaxAmount, inDataType, inDepositConfirm);
            if (!insertionResult) await Logger.ShowAlertDialog("자료 생성 실패");
        }

        public async Task EditDataInDB(AccountingData.QueryKeys inQueryKeys)
        {
            string tableName = "year_" + ViewModel.SelectedYearText;

            bool updateResult = await ViewModel.SqlManager.UpdateDataAsync(tableName, ViewModel.SelectedData, inQueryKeys);
            if (!updateResult) await Logger.ShowAlertDialog("자료 수정 실패");
        }

        public async Task DeleteDataInDB()
        {
            string tableName = "year_" + ViewModel.SelectedYearText;

            bool deletionResult = await ViewModel.SqlManager.DeleteDataAsync(tableName, ViewModel.SelectedData.Id);
            if (!deletionResult) await Logger.ShowAlertDialog("자료 삭제 실패");
        }

        private async void DataTypeColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareType) ViewModel.CurrentComparision = Comparisions.CompareTypeReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareTypeReverse) ViewModel.CurrentComparision = Comparisions.CompareType;
            else ViewModel.CurrentComparision = Comparisions.CompareType;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void ClientNameColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareName) ViewModel.CurrentComparision = Comparisions.CompareNameReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareNameReverse) ViewModel.CurrentComparision = Comparisions.CompareName;
            else ViewModel.CurrentComparision = Comparisions.CompareName;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void DateColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareDate) ViewModel.CurrentComparision = Comparisions.CompareDateReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareDateReverse) ViewModel.CurrentComparision = Comparisions.CompareDate;
            else ViewModel.CurrentComparision = Comparisions.CompareDate;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void SteelWeightColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareWeight) ViewModel.CurrentComparision = Comparisions.CompareWeightReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareWeightReverse) ViewModel.CurrentComparision = Comparisions.CompareWeight;
            else ViewModel.CurrentComparision = Comparisions.CompareWeight;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void SupplyPriceColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.ComparePrice) ViewModel.CurrentComparision = Comparisions.ComparePriceReverse;
            else if (ViewModel.CurrentComparision == Comparisions.ComparePriceReverse) ViewModel.CurrentComparision = Comparisions.ComparePrice;
            else ViewModel.CurrentComparision = Comparisions.ComparePrice;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void TaxAmountColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareTax) ViewModel.CurrentComparision = Comparisions.CompareTaxReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareTaxReverse) ViewModel.CurrentComparision = Comparisions.CompareTax;
            else ViewModel.CurrentComparision = Comparisions.CompareTax;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void SumColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareSum) ViewModel.CurrentComparision = Comparisions.CompareSumReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareSumReverse) ViewModel.CurrentComparision = Comparisions.CompareSum;
            else ViewModel.CurrentComparision = Comparisions.CompareSum;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void DepositConfirmColumnButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.CurrentComparision == Comparisions.CompareConfirm) ViewModel.CurrentComparision = Comparisions.CompareConfirmReverse;
            else if (ViewModel.CurrentComparision == Comparisions.CompareConfirmReverse) ViewModel.CurrentComparision = Comparisions.CompareConfirm;
            else ViewModel.CurrentComparision = Comparisions.CompareConfirm;

            await NavigateToAccountingDetailDataListPage();
        }

        private async void AddYearButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Generate controls used by AddYearDialog.
            AddYearDialogControls controls = new AddYearDialogControls();

            AddYearDialog dialog = new AddYearDialog(controls);
            dialog.Title = "연도 입력";
            dialog.PrimaryButtonText = "추가";
            dialog.SecondaryButtonText = "취소";

            ContentDialogResult result = await dialog.ShowAsync();

            // Handled when the add button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                int year = (int)controls.InputYear.SelectedItem;
                string yearText = year.ToString();

                string tableName = "year_" + yearText;

                bool exists = await ViewModel.SqlManager.DatabaseContainsAsync(ViewModel.DatabaseName, tableName);
                if (exists)
                {
                    await Logger.ShowAlertDialog("중복된 연도입니다.");
                    return;
                }

                Task creationTask = ViewModel.SqlManager.CreateTableAsync(tableName);

                NavigateToAccountingDetailDataListPageWithNoParams();

                await creationTask;

                NavigateToYearlyNavPage();
            }
        }

        private async void AddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Generate controls used by AddEditDataDialog.
            AddEditDialogControls controls = new AddEditDialogControls();

            AddEditDataDialog dialog = new AddEditDataDialog(controls);
            dialog.Title = "매출입 입력";
            dialog.PrimaryButtonText = "입력";
            dialog.SecondaryButtonText = "취소";

            ContentDialogResult result = await dialog.ShowAsync();

            // Handled when the add button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                string clientName = controls.InputClientName.Text;

                int month = (int)controls.InputMonth.SelectedItem;
                int day = (int)controls.InputDay.SelectedItem;

                string date = string.Format("{0:D2}", month) + "/" + string.Format("{0:D2}", day);

                int steelWeight;
                int.TryParse(controls.InputSteelWeight.Text, out steelWeight);

                int supplyPrice;
                int.TryParse(controls.InputSupplyPrice.Text, out supplyPrice);

                int taxAmount;
                int.TryParse(controls.InputTaxAmount.Text, out taxAmount);

                bool dataType = controls.InputDataType.SelectedIndex == 0 ? true : false;
                bool depositConfirm = (bool)controls.InputDepositConfirm.IsChecked;

                string tableName = "year_" + ViewModel.SelectedYearText;

                int nextId = 0;

                bool exists = await ViewModel.SqlManager.TableContainsAsync(tableName, AccountingData.QueryKeys.ENone, null);
                if (exists) nextId = await ViewModel.SqlManager.GetLastIdInTableAsync(tableName) + 1;

                await AddDataToDB(nextId, clientName, date, steelWeight, supplyPrice, taxAmount, dataType, depositConfirm);

                await NavigateToAccountingDetailDataListPageUsingYear();
            }
        }

        private async void EditButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.SelectedData.Id == -1)
            {
                await Logger.ShowAlertDialog("수정할 자료를 선택해주십시오.");
                return;
            }

            string monthText = ViewModel.SelectedData.Date.Substring(0, 2);
            string dayText = ViewModel.SelectedData.Date.Substring(3, 2);

            int month;
            int.TryParse(monthText, out month);

            int day;
            int.TryParse(dayText, out day);

            //
            // Generate the controls that is diplayed in the ContentDialog.
            //
            AddEditDialogControls controls = new AddEditDialogControls();
            controls.InputClientName.Text = ViewModel.SelectedData.ClientName;
            controls.InputDay.SelectedIndex = day - 1;
            controls.InputMonth.SelectedIndex = month - 1;
            controls.InputSteelWeight.Text = ViewModel.SelectedData.SteelWeight.ToString();
            controls.InputSupplyPrice.Text = ViewModel.SelectedData.SupplyPrice.ToString();
            controls.InputTaxAmount.Text = ViewModel.SelectedData.TaxAmount.ToString();
            controls.InputDataType.SelectedIndex = ViewModel.SelectedData.DataType ? 0 : 1;
            controls.InputDepositConfirm.IsChecked = ViewModel.SelectedData.DepositConfirm;

            //
            // Pop up ContentDialog.
            //
            AddEditDataDialog dialog = new AddEditDataDialog(controls);
            dialog.Title = "매출입 수정";
            dialog.PrimaryButtonText = "수정";
            dialog.SecondaryButtonText = "취소";

            ContentDialogResult result = await dialog.ShowAsync();

            // Handled when the edit button is clicked.
            if (result == ContentDialogResult.Primary)
            {
                //
                // Configure date after extract month, and day.
                //
                month = (int)controls.InputMonth.SelectedItem;
                day = (int)controls.InputDay.SelectedItem;

                string date = string.Format("{0:D2}", month) + "/" + string.Format("{0:D2}", day);

                //
                // Parse the text in TextBox to an integer.
                //
                int steelWeight;
                int.TryParse(controls.InputSteelWeight.Text, out steelWeight);

                int supplyPrice;
                int.TryParse(controls.InputSupplyPrice.Text, out supplyPrice);

                int taxAmount;
                int.TryParse(controls.InputTaxAmount.Text, out taxAmount);

                bool dataType = controls.InputDataType.SelectedIndex == 0 ? true : false;
                bool confirmed = (bool)controls.InputDepositConfirm.IsChecked;

                //
                // Specify the changed data.
                //
                AccountingData.QueryKeys queryKeys = AccountingData.QueryKeys.ENone;

                if (ViewModel.SelectedData.ClientName != controls.InputClientName.Text)
                {
                    queryKeys |= AccountingData.QueryKeys.EClientName;
                    ViewModel.SelectedData.ClientName = controls.InputClientName.Text;
                }
                if (ViewModel.SelectedData.Date != date)
                {
                    queryKeys |= AccountingData.QueryKeys.EDate;
                    ViewModel.SelectedData.Date = date;
                }
                if (ViewModel.SelectedData.SteelWeight != steelWeight)
                {
                    queryKeys |= AccountingData.QueryKeys.ESteelWeight;
                    ViewModel.SelectedData.SteelWeight = steelWeight;
                }
                if (ViewModel.SelectedData.SupplyPrice != supplyPrice)
                {
                    queryKeys |= AccountingData.QueryKeys.ESupplyPrice;
                    ViewModel.SelectedData.SupplyPrice = supplyPrice;
                }
                if (ViewModel.SelectedData.TaxAmount != taxAmount)
                {
                    queryKeys |= AccountingData.QueryKeys.ETaxAmount;
                    ViewModel.SelectedData.TaxAmount = taxAmount;
                }
                if (ViewModel.SelectedData.DataType != dataType)
                {
                    queryKeys |= AccountingData.QueryKeys.EDataType;
                    ViewModel.SelectedData.DataType = dataType;
                }
                if (ViewModel.SelectedData.DepositConfirm != confirmed)
                {
                    queryKeys |= AccountingData.QueryKeys.EDepositConfirm;
                    ViewModel.SelectedData.DepositConfirm = confirmed;
                }

                await EditDataInDB(queryKeys);

                await NavigateToAccountingDetailDataListPageUsingYear();
            }
        }

        private async void DeleteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.SelectedData.Id == -1)
            {
                await Logger.ShowAlertDialog("삭제할 자료를 선택해주십시오.");
                return;
            }

            await DeleteDataInDB();

            await NavigateToAccountingDetailDataListPageUsingYear();
        }

        private async void SearchButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (InputTextBox.Text.Length < 1)
            {
                await Logger.ShowAlertDialog("검색어를 입력해주십시오.");
                return;
            }

            string tableName = "year_" + ViewModel.SelectedYearText;

            AccountingData.QueryValues values = new AccountingData.QueryValues();
            values.ClientName = InputTextBox.Text;

            bool contains = await ViewModel.SqlManager.TableContainsAsync(tableName, AccountingData.QueryKeys.EClientName, values);
            if (!contains)
            {
                await Logger.ShowAlertDialog("검색된 결과가 없습니다.");
                return;
            }

            List<AccountingData> dataList = new List<AccountingData>();
            bool gettingResult = await ViewModel.SqlManager.GetAccountingDataAsync(tableName, AccountingData.QueryKeys.EClientName, values, dataList);
            if (!gettingResult)
            {
                await Logger.ShowAlertDialog("DB 데이터 불러오기 실패");
                return;
            }

            await NavigateToAccountingDetailDataListPageUsingDataList(dataList);
        }
    }
}
