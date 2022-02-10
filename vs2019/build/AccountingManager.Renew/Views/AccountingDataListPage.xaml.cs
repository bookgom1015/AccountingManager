using System;
using System.Linq;
using System.Collections.Generic;
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

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Core.Helpers;
using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Dialogs;
using AccountingManager.Renew.Dialogs.Controls;
using AccountingManager.Renew.Helpers;
using AccountingManager.Renew.Helpers.NavParams;
using AccountingManager.Renew.ViewModels;

namespace AccountingManager.Renew.Views {
    public sealed partial class AccountingDataListPage : Page {
        private AccountingDataListViewModel ViewModel => DataContext as AccountingDataListViewModel;

        public AccountingDataListPage() {
            InitializeComponent();

            Application.Current.EnteredBackground += OnEnteredBackground;
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
                await MessageBox("오류", "올바르지 않은 인자가 전달되었습니다");
                CoreApplication.Exit();
            }

            ViewModel.LoadSettings();

            DateTime now = DateTime.Now;

            BeginDate.Date = new DateTime(now.Year, now.Month, 1);
            EndDate.Date = now;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            ViewModel.SaveSettings();

            base.OnNavigatedFrom(e);
        }

        private void NavFrame_Loaded(object sender, RoutedEventArgs e) {
            ShowDateNavs();
        }

        private async void InputTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) await SearchData();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                await SearchData();
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
                        await MessageBox("오류", addResult.ErrMsg);
                        return;
                    }

                    await ShowDataList(true);
                    ShowDateNavs();
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (!ViewModel.DataIsSelected) {
                    await MessageBox("경고", "수정할 자료를 선택해주십시오");
                    return;
                }

                AddEditControls controls = new AddEditControls { AccountingData = ViewModel.SelectedData };
                AddEditDialog dialog = new AddEditDialog(controls) { Title = "자료 수정", PrimaryButtonText = "수정", SecondaryButtonText = "취소" };
                ContentDialogResult dialogResult = await dialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    Result updateResult = ViewModel.EditData(controls.AccountingData);
                    if (!updateResult.Status) {
                        await MessageBox("오류", updateResult.ErrMsg);
                        return;
                    }

                    await ShowDataList(true);
                    ShowDateNavs();

                    ViewModel.SelectedData = null;
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (!ViewModel.DataIsSelected) {
                    await MessageBox("경고", "삭제할 자료를 선택해주십시오");
                    return;
                }

                DeleteDialog dialog = new DeleteDialog();
                ContentDialogResult dialogResult = await dialog.ShowAsync();

                if (dialogResult == ContentDialogResult.Primary) {
                    Result deleteResult = ViewModel.RemoveData(ViewModel.SelectedData);
                    if (!deleteResult.Status) {
                        await MessageBox("오류", deleteResult.ErrMsg);
                        return;
                    }

                    await ShowDataList(true);
                    ShowDateNavs();

                    ViewModel.SelectedData = null;
                }
            }
        }

        private async void DataTypeColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareType) ViewModel.CurrentComparision = AccountingDataComparisions.CompareTypeReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareTypeReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareType;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareType;

                await ShowDataList(false, true);
            }
        }

        private async void ClientNameColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareName) ViewModel.CurrentComparision = AccountingDataComparisions.CompareNameReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareNameReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareName;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareName;

                await ShowDataList(false, true);
            }
        }

        private async void DateColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareDate) ViewModel.CurrentComparision = AccountingDataComparisions.CompareDateReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareDateReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareDate;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareDate;

                await ShowDataList(false, true);
            }
        }

        private async void SteelWeightColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareWeight) ViewModel.CurrentComparision = AccountingDataComparisions.CompareWeightReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareWeightReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareWeight;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareWeight;

                await ShowDataList(false, true);
            }
        }

        private async void SupplyPriceColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.ComparePrice) ViewModel.CurrentComparision = AccountingDataComparisions.ComparePriceReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.ComparePriceReverse) ViewModel.CurrentComparision = AccountingDataComparisions.ComparePrice;
                else ViewModel.CurrentComparision = AccountingDataComparisions.ComparePrice;

                await ShowDataList(false, true);
            }
        }

        private async void TaxAmountColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareTax) ViewModel.CurrentComparision = AccountingDataComparisions.CompareTaxReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareTaxReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareTax;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareTax;

                await ShowDataList(false, true);
            }
        }

        private async void SumColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareSum) ViewModel.CurrentComparision = AccountingDataComparisions.CompareSumReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareSumReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareSum;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareSum;

                await ShowDataList(false, true);
            }
        }

        private async void DepositConfirmedColumnButton_Click(object sender, RoutedEventArgs e) {
            using (var btnLocker = new DoubleClickPreventer(sender)) {
                if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareConfirm) ViewModel.CurrentComparision = AccountingDataComparisions.CompareConfirmReverse;
                else if (ViewModel.CurrentComparision == AccountingDataComparisions.CompareConfirmReverse) ViewModel.CurrentComparision = AccountingDataComparisions.CompareConfirm;
                else ViewModel.CurrentComparision = AccountingDataComparisions.CompareConfirm;

                await ShowDataList(false, true);
            }
        }

        private async Task MessageBox(string title, string msg) {
            MessageDialog dialog = new MessageDialog() { Title = title, Message = msg };
            await dialog.ShowAsync();
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

            await ShowDataList(true);
        }

        private async void SelectedMonthChanged(int month) {
            ViewModel.SelectedMonth = month;
            ViewModel.BeginDate = null;
            ViewModel.EndDate = null;
            ViewModel.LookingForClietnName = null;

            await ShowDataList(true);
        }

        private async Task ShowDataList(bool needToUpdate = false, bool needToSort = false) {
            if (needToUpdate) {
                IEnumerable<AccountingData> data;
                if (ViewModel.IsForSearching) {                    
                    Result result = ViewModel.GetData(out data, (DateTime)ViewModel.BeginDate, (DateTime)ViewModel.EndDate, ViewModel.LookingForClietnName);
                    if (!result.Status) {
                        await MessageBox("오류", result.ErrMsg);
                        return;
                    }
                }
                else {
                    if (ViewModel.SelectedYear == null) return;
                    Result result = ViewModel.NavParams.DbManager.GetData(out data, ViewModel.SelectedYear, ViewModel.SelectedMonth);
                    if (!result.Status) {
                        await MessageBox("오류", result.ErrMsg);
                        return;
                    }
                }
                ViewModel.AccountingDataList = data.ToList();
            }

            if (ViewModel.AccountingDataList == null) return;
            if (needToSort) ViewModel.SortAccountingData();

            TotalSum.Text = ViewModel.AccountingDataList.Sum(a => a.SupplyPrice + a.TaxAmount).ToString();
            TotalWeight.Text = ViewModel.AccountingDataList.Sum(a => a.SteelWeight).ToString();

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
                    SelectedYear = ViewModel.SelectedYear,
                    SelectedMonthChanged = this.SelectedMonthChanged
                };
                NavFrame.Navigate(typeof(MonthlyNavPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
                if (NavFrame.BackStackDepth > 0) NavFrame.BackStack.RemoveAt(NavFrame.BackStackDepth - 1);
            }
            else {
                YearlyNavParams navParams = new YearlyNavParams {
                    DbManager = ViewModel.NavParams.DbManager,
                    SelectedYear = ViewModel.SelectedYear,
                    SelectedYearChanged = this.SelectedYearChanged,
                    SelectedMonthChanged = this.SelectedMonthChanged
                };
                NavFrame.Navigate(typeof(YearlyNavPage), navParams, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
                if (NavFrame.BackStackDepth > 0) NavFrame.BackStack.RemoveAt(NavFrame.BackStackDepth - 1);
            }
        }        

        private async Task SearchData() {
            if (InputTextBox.Text.Length == 0) {
                await MessageBox("경고", "검색할 거래처를 입력해주십시오");
                return;
            }

            ViewModel.BeginDate = new DateTime(BeginDate.Date.Year, BeginDate.Date.Month, BeginDate.Date.Day);
            ViewModel.EndDate = new DateTime(EndDate.Date.Year, EndDate.Date.Month, EndDate.Date.Day);
            ViewModel.LookingForClietnName = InputTextBox.Text;

            if (ViewModel.BeginDate >= ViewModel.EndDate) {
                await MessageBox("경고", "시작 날짜가 끝 날짜와 같거나 보다 큽니다");
                return;
            }

            await ShowDataList(true);
        }
    }
}
