using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Core.Helpers;
using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Dialogs;
using AccountingManager.Renew.Dialogs.Controls;
using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.Helpers.NavParams;
using AccountingManager.Renew.ViewModels;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AccountingManager.Renew.Views {
    public sealed partial class AccountingDataListPage : Page {
        private AccountingDataListViewModel ViewModel => DataContext as AccountingDataListViewModel;

        public AccountingDataListPage() {
            InitializeComponent();

            Application.Current.EnteredBackground += OnEnteredBackground;

            ViewModel.ColumnDirIndicators.Add(DataTypeIndicator);
            ViewModel.ColumnDirIndicators.Add(ClientNameIndicator);
            ViewModel.ColumnDirIndicators.Add(DateIndicator);
            ViewModel.ColumnDirIndicators.Add(SteelWeightIndicator);
            ViewModel.ColumnDirIndicators.Add(SupplyPriceIndicator);
            ViewModel.ColumnDirIndicators.Add(TaxAmountIndicator);
            ViewModel.ColumnDirIndicators.Add(SumIndicator);
            ViewModel.ColumnDirIndicators.Add(DepositConfirmedIndicator);
        }

        private void OnEnteredBackground(Object sender, EnteredBackgroundEventArgs e) {
            Deferral deferral = e.GetDeferral();

            ViewModel.SaveSettings();

            deferral.Complete();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            AccountingDataListNavParams navParams = e.Parameter as AccountingDataListNavParams;
            if (navParams != null) {
                ViewModel.NavParams = navParams;
            }
            else {
                await Logger.MessageBox("오류", "올바르지 않은 인자가 전달되었습니다");
                CoreApplication.Exit();
            }

            ViewModel.LoadSettings();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            ViewModel.SaveSettings();

            base.OnNavigatedFrom(e);
        }

        private void NavFrame_Loaded(object sender, RoutedEventArgs e) {
            ShowDateNavs();
        }

        private void DatePickerGrid_Loaded(object sender, RoutedEventArgs e) {
            DateTime now = DateTime.Now;

            BeginDate.Date = new DateTime(now.Year, now.Month, 1);
            EndDate.Date = now;
        }

        private async void InputTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                using (var locker = new DoubleClickPreventer(sender)) {
                    await SearchData();
                }
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                await SearchData();
            }
        }

        private async void ExportButton_Click(object sender, RoutedEventArgs e){
            using (var btnLocker = new DoubleClickPreventer(sender)){
                if (ViewModel.AccountingDataList.Count <= 0) {
                    await Logger.MessageBox("경고", "저장할 자료가 없습니다");
                    return;
                }
                var savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as.
                savePicker.FileTypeChoices.Add("엑셀 통합 파일(.xlsx)", new List<string>() { ".xlsx" });
                savePicker.FileTypeChoices.Add("텍스트 파일(.txt)", new List<string>() { ".txt" });
                // Default file name if the user does not type one in or select a file to replace.
                savePicker.SuggestedFileName = "새 파일";

                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file == null) {
                    await Logger.MessageBox("오류", "저장을 실패했습니다");
                    return;
                }
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // Arrange the content.
                using (ExcelPackage package = new ExcelPackage()) {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet");
                    //
                    // Data type
                    //
                    worksheet.Column(1).Width = 10.0;

                    var dataTypeCell = worksheet.Cells[1, 1];
                    dataTypeCell.Value = "매입/매출";
                    dataTypeCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    
                    var dataTypeCells = worksheet.Cells["A2:A65535"];
                    dataTypeCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //
                    // Client name
                    //
                    worksheet.Column(2).Width = 18.0;

                    var clientNameCell = worksheet.Cells[1, 2];
                    clientNameCell.Value = "거래처";
                    clientNameCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var clientNameCells = worksheet.Cells["B2:B65535"];
                    clientNameCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    clientNameCells.Style.Indent = 1;
                    //
                    // Date
                    //
                    worksheet.Column(3).Width = 12.0;

                    var dateCell = worksheet.Cells[1, 3];
                    dateCell.Value = "날짜";
                    dateCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var dateCells = worksheet.Cells["C2:C65535"];
                    dateCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //
                    // Steel weight
                    //
                    var steelWeightCell = worksheet.Cells[1, 4];
                    steelWeightCell.Value = "중량(t)";
                    steelWeightCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var steelWeightCells = worksheet.Cells["D2:D65535"];
                    steelWeightCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    steelWeightCells.Style.Numberformat.Format = "#,##0.0";
                    steelWeightCells.Style.Indent = 1;
                    //
                    // Supply price
                    //
                    worksheet.Column(5).Width = 18.0;

                    var supplyPriceCell = worksheet.Cells[1, 5];
                    supplyPriceCell.Value = "공급가격";
                    supplyPriceCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var supplyPriceCells = worksheet.Cells["E2:E65535"];
                    supplyPriceCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    supplyPriceCells.Style.Numberformat.Format = "#,##0";
                    supplyPriceCells.Style.Indent = 1;
                    //
                    // Tax amount
                    //
                    worksheet.Column(6).Width = 18.0;

                    var taxAmountCell = worksheet.Cells[1, 6];
                    taxAmountCell.Value = "세액";
                    taxAmountCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var taxAmountCells = worksheet.Cells["F2:F65535"];
                    taxAmountCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    taxAmountCells.Style.Numberformat.Format = "#,##0";
                    taxAmountCells.Style.Indent = 1;
                    //
                    // Sum
                    //
                    worksheet.Column(7).Width = 18.0;

                    var sumCell = worksheet.Cells[1, 7];
                    sumCell.Value = "합계";
                    sumCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var sumCells = worksheet.Cells["G2:G65535"];
                    sumCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sumCells.Style.Numberformat.Format = "#,##0";
                    sumCells.Style.Indent = 1;
                    //
                    // Deposit confirmed
                    //
                    worksheet.Column(8).Width = 10.0;

                    var depositConfirmedCell = worksheet.Cells[1, 8];
                    depositConfirmedCell.Value = "입금확인";
                    depositConfirmedCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var depositConfirmedCells = worksheet.Cells["H2:H65535"];
                    depositConfirmedCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //
                    // Deposit date
                    //
                    worksheet.Column(9).Width = 12.0;

                    var depositDate = worksheet.Cells[1, 9];
                    depositDate.Value = "확인날짜";
                    depositDate.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var depositDateCells = worksheet.Cells["I2:I65535"];
                    depositDateCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //
                    // Insert each accounting data into each cell.
                    //
                    int row = 2;
                    foreach(AccountingData data in ViewModel.AccountingDataList) {
                        worksheet.Cells[row, 1].Value = data.DataType ? "매입" : "매출";
                        worksheet.Cells[row, 2].Value = data.ClientName;
                        worksheet.Cells[row, 3].Value = data.DateFormatting;
                        worksheet.Cells[row, 4].Value = data.SteelWeight;
                        worksheet.Cells[row, 5].Value = data.SupplyPrice;
                        worksheet.Cells[row, 6].Value = data.TaxAmount;
                        worksheet.Cells[row, 7].Value = data.Sum;
                        worksheet.Cells[row, 8].Value = data.DepositConfirmed ? "확인됨" : "";
                        worksheet.Cells[row, 9].Value = data.DepositDate;
                        ++row;
                    }
                    // write to file.
                    await FileIO.WriteBytesAsync(file, package.GetAsByteArray());
                }
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                    await Logger.MessageBox("알림", "저장을 완료했습니다");
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                AddEditControls controls = new AddEditControls();
                AddEditDialog dialog = new AddEditDialog(controls) { Title = "자료 추가", PrimaryButtonText = "추가", SecondaryButtonText = "취소" };
                ContentDialogResult dialogResult = await dialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    Result addResult = ViewModel.AddData(controls.AccountingData);
                    if (!addResult.Status) {
                        await Logger.MessageBox("오류", addResult.ErrMsg);
                        return;
                    }

                    await ShowDataList(true, true);
                    ShowDateNavs();
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (!ViewModel.DataIsSelected) {
                    await Logger.MessageBox("경고", "수정할 자료를 선택해주십시오");
                    return;
                }

                AddEditControls controls = new AddEditControls { AccountingData = ViewModel.SelectedData };
                AddEditDialog dialog = new AddEditDialog(controls) { Title = "자료 수정", PrimaryButtonText = "수정", SecondaryButtonText = "취소" };
                ContentDialogResult dialogResult = await dialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    Result updateResult = ViewModel.EditData(controls.AccountingData);
                    if (!updateResult.Status) {
                        await Logger.MessageBox("오류", updateResult.ErrMsg);
                        return;
                    }

                    ViewModel.DataEditted = true;

                    await ShowDataList(true, true);
                    ShowDateNavs();
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (!ViewModel.DataIsSelected) {
                    await Logger.MessageBox("경고", "삭제할 자료를 선택해주십시오");
                    return;
                }

                DeleteDialog dialog = new DeleteDialog();
                ContentDialogResult dialogResult = await dialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    Result deleteResult = ViewModel.RemoveData(ViewModel.SelectedData);
                    if (!deleteResult.Status) {
                        await Logger.MessageBox("오류", deleteResult.ErrMsg);
                        return;
                    }

                    ViewModel.DataEditted = true;

                    await ShowDataList(true, true);
                    ShowDateNavs();
                }
            }
        }

        private async void DataTypeColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareType) {
                    DataTypeIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareTypeReverse;
                }
                else {
                    DataTypeIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareType;
                }

                ViewModel.ConfigureColumnDirIndicators(DataTypeIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void ClientNameColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareName) {
                    ClientNameIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareNameReverse;
                }
                else {
                    ClientNameIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareName;
                }

                ViewModel.ConfigureColumnDirIndicators(ClientNameIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void DateColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareDate) {
                    DateIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareDateReverse;
                }
                else {
                    DateIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareDate;
                }

                ViewModel.ConfigureColumnDirIndicators(DateIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void SteelWeightColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareWeight) {
                    SteelWeightIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareWeightReverse;
                }
                else {
                    SteelWeightIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareWeight;
                }

                ViewModel.ConfigureColumnDirIndicators(SteelWeightIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void SupplyPriceColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.ComparePrice) {
                    SupplyPriceIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.ComparePriceReverse;
                }
                else {
                    SupplyPriceIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.ComparePrice;
                }

                ViewModel.ConfigureColumnDirIndicators(SupplyPriceIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void TaxAmountColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareTax) {
                    TaxAmountIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareTaxReverse;
                }
                else {
                    TaxAmountIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareTax;
                }

                ViewModel.ConfigureColumnDirIndicators(TaxAmountIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void SumColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareSum) {
                    SumIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareSumReverse;
                }
                else {
                    SumIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareSum;
                }

                ViewModel.ConfigureColumnDirIndicators(SumIndicator);
                await ShowDataList(false, true);
            }
        }

        private async void DepositConfirmedColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareConfirm) {
                    DepositConfirmedIndiRotation.Angle = 180;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareConfirmReverse;
                }
                else {
                    DepositConfirmedIndiRotation.Angle = 0;
                    ViewModel.CurrentComparision = AccountingDataComparisions.CompareConfirm;
                }

                ViewModel.ConfigureColumnDirIndicators(DepositConfirmedIndicator);
                await ShowDataList(false, true);
            }
        }

        private void SelectedAccountingDataChanged(AccountingData data) {
            ViewModel.SelectedData = data;
        }

        private async void SelectedYearChanged(int year) {
            ViewModel.SelectedYear = year;
            ViewModel.SelectedMonth = null;
            ViewModel.BeginDate = null;
            ViewModel.EndDate = null;
            ViewModel.LookingForClietnName = null;

            await ShowDataList(true, true);
        }

        private async void SelectedMonthChanged(int month) {
            ViewModel.SelectedMonth = month;
            ViewModel.BeginDate = null;
            ViewModel.EndDate = null;
            ViewModel.LookingForClietnName = null;

            await ShowDataList(true, true);
        }

        private async Task ShowDataList(bool needToUpdate = false, bool needToSort = false) {
            if (needToUpdate) {
                IEnumerable<AccountingData> data;
                if (ViewModel.IsForSearching) {                    
                    Result result = ViewModel.GetData(out data, (DateTime)ViewModel.BeginDate, (DateTime)ViewModel.EndDate, ViewModel.LookingForClietnName, false);
                    if (!result.Status) {
                        await Logger.MessageBox("오류", result.ErrMsg);
                        return;
                    }
                }
                else {
                    if (ViewModel.SelectedYear == null) return;
                    Result result = ViewModel.GetData(out data, ViewModel.SelectedYear, ViewModel.SelectedMonth, null, false);
                    if (!result.Status) {
                        await Logger.MessageBox("오류", result.ErrMsg);
                        return;
                    }
                }
                ViewModel.AccountingDataList = data.ToList();
            }

            if (!ViewModel.DataEditted && ViewModel.IsForSearching && ViewModel.AccountingDataList.Count == 0) {
                ViewModel.BeginDate = null;
                ViewModel.EndDate = null;
                await Logger.MessageBox("알림", "검색된 결과가 없습니다");
                return;
            }
            ViewModel.DataEditted = false;

            if (needToSort) ViewModel.SortAccountingData();

            ViewModel.SelectedData = null;

            TotalSales.Text = string.Format("{0:#,##0}", ViewModel.AccountingDataList.Where(a => a.DataType == false).Sum(a => a.SupplyPrice + a.TaxAmount));
            TotalPurchases.Text = string.Format("{0:#,##0}", ViewModel.AccountingDataList.Where(a => a.DataType == true).Sum(a => a.SupplyPrice + a.TaxAmount));
            TotalWeight.Text = string.Format("{0:#,##0.##}", ViewModel.AccountingDataList.Where(a => a.DataType == false).Sum(a => a.SteelWeight));

            DetailAccountingDataListNavParams navParams = new DetailAccountingDataListNavParams {           
                DataTypeColWidthBinding = BindingHelper.CreateBinding(ViewModel, "DataTypeColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                ClientNameColWidthBinding = BindingHelper.CreateBinding(ViewModel, "ClientNameColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                DateColWidthBinding = BindingHelper.CreateBinding(ViewModel, "DateColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                SteelWeightColWidthBinding = BindingHelper.CreateBinding(ViewModel, "SteelWeightColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                SupplyPriceColWidthBinding = BindingHelper.CreateBinding(ViewModel, "SupplyPriceColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                TaxAmountColWidthBinding = BindingHelper.CreateBinding(ViewModel, "TaxAmountColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                SumColWidthBinding = BindingHelper.CreateBinding(ViewModel, "SumColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                DepositConfirmedColWidthBinding = BindingHelper.CreateBinding(ViewModel, "DepositConfirmColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                AccountingData = ViewModel.AccountingDataList,
                SelectedAccountingDataChanged = this.SelectedAccountingDataChanged
            };
            DetailListFrame.Navigate(typeof(DetailAccountingDataListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }        

        private void ShowDateNavs() {
            if (NavFrame.CurrentSourcePageType == typeof(MonthlyNavPage)) {
                MonthlyNavParams navParams = new MonthlyNavParams {
                    DbManager = ViewModel.NavParams.DbManager,
                    Receivable = false,
                    SelectedYear = ViewModel.SelectedYear,
                    SelectedMonthChanged = this.SelectedMonthChanged
                };
                NavFrame.Navigate(typeof(MonthlyNavPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
                if (NavFrame.BackStackDepth > 0) NavFrame.BackStack.RemoveAt(NavFrame.BackStackDepth - 1);
            }
            else {
                YearlyNavParams navParams = new YearlyNavParams {
                    DbManager = ViewModel.NavParams.DbManager,
                    Receivable = false,
                    SelectedYearChanged = this.SelectedYearChanged,
                    SelectedMonthChanged = this.SelectedMonthChanged
                };
                NavFrame.Navigate(typeof(YearlyNavPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
                if (NavFrame.BackStackDepth > 0) NavFrame.BackStack.RemoveAt(NavFrame.BackStackDepth - 1);
            }
        }        

        private async Task SearchData() {
            if (InputTextBox.Text.Length == 0) {
                await Logger.MessageBox("경고", "검색할 거래처를 입력해주십시오");
                return;
            }

            ViewModel.BeginDate = new DateTime(BeginDate.Date.Year, BeginDate.Date.Month, BeginDate.Date.Day);
            ViewModel.EndDate = new DateTime(EndDate.Date.Year, EndDate.Date.Month, EndDate.Date.Day);
            ViewModel.LookingForClietnName = InputTextBox.Text;

            if (ViewModel.BeginDate >= ViewModel.EndDate) {
                await Logger.MessageBox("경고", "시작 날짜가 끝 날짜와 같거나 보다 큽니다");
                return;
            }

            await ShowDataList(true);
        }
    }
}
