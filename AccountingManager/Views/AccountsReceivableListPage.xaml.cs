using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.Core.Helpers;
using AccountingManager.Core.Infrastructures;
using AccountingManager.Core.Models;
using AccountingManager.Dialogs;
using AccountingManager.Dialogs.Controls;
using AccountingManager.Helpers;
using AccountingManager.Helpers.NavParams;
using AccountingManager.ViewModels;

namespace AccountingManager.Views {
    public sealed partial class AccountsReceivableListPage : Page {
        private AccountsReceivableListViewModel ViewModel => DataContext as AccountsReceivableListViewModel;

        public AccountsReceivableListPage() {
            InitializeComponent();

            Application.Current.EnteredBackground += OnEnteredBackground;

            ViewModel.ColumnDirIndicators.Add(DataTypeIndicator);
            ViewModel.ColumnDirIndicators.Add(ClientNameIndicator);
            ViewModel.ColumnDirIndicators.Add(DateIndicator);
            ViewModel.ColumnDirIndicators.Add(SumIndicator);
        }

        private void OnEnteredBackground(Object sender, EnteredBackgroundEventArgs e) {
            Deferral deferral = e.GetDeferral();

            ViewModel.SaveSettings();

            deferral.Complete();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            AccountsReceivableListNavParams navParams = e.Parameter as AccountsReceivableListNavParams;
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

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (!ViewModel.DataIsSelected) {
                    await Logger.MessageBox("경고", "수정할 자료를 선택해주십시오");
                    return;
                }

                DepositDateControls controls = new DepositDateControls { AccountingData = ViewModel.SelectedData };
                DepositDateDialog dialog = new DepositDateDialog(controls);
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

        private async void SearchButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                await SearchData();
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
                    Result result = ViewModel.GetData(out data, (DateTime)ViewModel.BeginDate, (DateTime)ViewModel.EndDate, ViewModel.LookingForClietnName, true);
                    if (!result.Status) {
                        await Logger.MessageBox("오류", result.ErrMsg);
                        return;
                    }
                }
                else {
                    if (ViewModel.SelectedYear == null) return;
                    Result result = ViewModel.GetData(out data, ViewModel.SelectedYear, ViewModel.SelectedMonth, null, true);
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
            
            TotalSales.Text = string.Format("{0:#,##0}", ViewModel.AccountingDataList.Where(a => a.DataType == false).Sum(a => a.Sum));
            TotalPurchases.Text = string.Format("{0:#,##0}", ViewModel.AccountingDataList.Where(a => a.DataType == true).Sum(a => a.Sum));
            
            DetailAccountsReceivableListNavParams navParams = new DetailAccountsReceivableListNavParams {
                DataTypeColWidthBinding = BindingHelper.CreateBinding(ViewModel, "DataTypeColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                ClientNameColWidthBinding = BindingHelper.CreateBinding(ViewModel, "ClientNameColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                DateColWidthBinding = BindingHelper.CreateBinding(ViewModel, "DateColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                SumColWidthBinding = BindingHelper.CreateBinding(ViewModel, "SumColumnWidth", BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged),
                AccountingData = ViewModel.AccountingDataList,
                SelectedAccountingDataChanged = this.SelectedAccountingDataChanged
            };
            DetailListFrame.Navigate(typeof(DetailAccountsReceivableListPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void ShowDateNavs() {
            if (NavFrame.CurrentSourcePageType == typeof(MonthlyNavPage)) {
                MonthlyNavParams navParams = new MonthlyNavParams {
                    DbManager = ViewModel.NavParams.DbManager,
                    Receivable = true,
                    SelectedYear = ViewModel.SelectedYear,
                    SelectedMonthChanged = this.SelectedMonthChanged
                };
                NavFrame.Navigate(typeof(MonthlyNavPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
                if (NavFrame.BackStackDepth > 0) NavFrame.BackStack.RemoveAt(NavFrame.BackStackDepth - 1);
            }
            else {
                YearlyNavParams navParams = new YearlyNavParams {
                    DbManager = ViewModel.NavParams.DbManager,
                    Receivable = true,
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

            await ShowDataList(true, true);
        }
    }
}
