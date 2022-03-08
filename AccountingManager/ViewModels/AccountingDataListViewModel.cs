using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Models;
using AccountingManager.Core.Helpers;
using AccountingManager.Core.Infrastructures;
using AccountingManager.Helpers.NavParams;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AccountingManager.ViewModels {
    public class AccountingDataListViewModel : ViewModelBase {
        public AccountingDataListViewModel() {
            AccountingDataList = new List<AccountingData>();

            CurrentComparision = AccountingDataComparisions.CompareDate;

            ColumnDirIndicators = new List<BitmapIcon>();
        }

        public void LoadSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object dataTypeWidthObj = localSettings.Values["DataTypeColumnWidth"];
            DataTypeColumnWidth = new GridLength(dataTypeWidthObj == null ? DefaultColumnWidth : (double)dataTypeWidthObj);

            Object clientNameWidthObj = localSettings.Values["ClientNameColumnWidth"];
            ClientNameColumnWidth = new GridLength(clientNameWidthObj == null ? DefaultColumnWidth : (double)clientNameWidthObj);

            Object dateWidthObj = localSettings.Values["DateColumnWidth"];
            DateColumnWidth = new GridLength(dateWidthObj == null ? DefaultColumnWidth : (double)dateWidthObj);

            Object steelWeightWidthObj = localSettings.Values["SteelWeightColumnWidth"];
            SteelWeightColumnWidth = new GridLength(steelWeightWidthObj == null ? DefaultColumnWidth : (double)steelWeightWidthObj);

            Object supplyPriceWidthObj = localSettings.Values["SupplyPriceColumnWidth"];
            SupplyPriceColumnWidth = new GridLength(supplyPriceWidthObj == null ? DefaultColumnWidth : (double)supplyPriceWidthObj);

            Object taxAmountWidthObj = localSettings.Values["TaxAmountColumnWidth"];
            TaxAmountColumnWidth = new GridLength(taxAmountWidthObj == null ? DefaultColumnWidth : (double)taxAmountWidthObj);

            Object sumWidthObj = localSettings.Values["SumColumnWidth"];
            SumColumnWidth = new GridLength(sumWidthObj == null ? DefaultColumnWidth : (double)sumWidthObj);

            Object depositConfirmWidthObj = localSettings.Values["DepositConfirmColumnWidth"];
            DepositConfirmColumnWidth = new GridLength(depositConfirmWidthObj == null ? DefaultColumnWidth : (double)depositConfirmWidthObj);
        }

        public void SaveSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["DataTypeColumnWidth"] = DataTypeColumnWidth.Value;
            localSettings.Values["ClientNameColumnWidth"] = ClientNameColumnWidth.Value;
            localSettings.Values["DateColumnWidth"] = DateColumnWidth.Value;
            localSettings.Values["SteelWeightColumnWidth"] = SteelWeightColumnWidth.Value;
            localSettings.Values["SupplyPriceColumnWidth"] = SupplyPriceColumnWidth.Value;
            localSettings.Values["TaxAmountColumnWidth"] = TaxAmountColumnWidth.Value;
            localSettings.Values["SumColumnWidth"] = SumColumnWidth.Value;
            localSettings.Values["DepositConfirmColumnWidth"] = DepositConfirmColumnWidth.Value;
        }

        public Result GetData(out IEnumerable<AccountingData> data, int? year = null, int? month = null, int? day = null, bool receivable = false) {
            return NavParams.DbManager.GetData(out data, year, month, day, receivable);
        }

        public Result GetData(out IEnumerable<AccountingData> data, DateTime begin, DateTime end, string clientName, bool receivable = false) {
            return NavParams.DbManager.GetData(out data, begin, end, clientName, receivable);
        }

        public Result AddData(AccountingData data) {
            return NavParams.DbManager.Add(data);
        }

        public Result EditData(AccountingData data) {
            AccountingDataQueryKeys queryKeys = AccountingDataQueryKeys.ENone;

            if (SelectedData.ClientName != data.ClientName) {
                queryKeys |= AccountingDataQueryKeys.EClientName;
                SelectedData.ClientName = data.ClientName;
            }
            if (SelectedData.Date != data.Date) {
                queryKeys |= AccountingDataQueryKeys.EDate;
                SelectedData.Date = data.Date;
            }
            if (SelectedData.SteelWeight != data.SteelWeight) {
                queryKeys |= AccountingDataQueryKeys.ESteelWeight;
                SelectedData.SteelWeight = data.SteelWeight;
            }
            if (SelectedData.SupplyPrice != data.SupplyPrice) {
                queryKeys |= AccountingDataQueryKeys.ESupplyPrice;
                SelectedData.SupplyPrice = data.SupplyPrice;
            }
            if (SelectedData.TaxAmount != data.TaxAmount) {
                queryKeys |= AccountingDataQueryKeys.ETaxAmount;
                SelectedData.TaxAmount = data.TaxAmount;
            }
            if (SelectedData.DataType != data.DataType) {
                queryKeys |= AccountingDataQueryKeys.EDataType;
                SelectedData.DataType = data.DataType;
            }
            if (SelectedData.DepositConfirmed != data.DepositConfirmed) {
                queryKeys |= AccountingDataQueryKeys.EDepositConfirmed;
                SelectedData.DepositConfirmed = data.DepositConfirmed;
            }
            if (SelectedData.DepositDate != data.DepositDate) {
                queryKeys |= AccountingDataQueryKeys.EDepositDate;
                SelectedData.DepositDate = data.DepositDate;
            }

            return NavParams.DbManager.Update(SelectedData, queryKeys);
        }

        public Result RemoveData(AccountingData data) {
            return NavParams.DbManager.Delete(data);
        }

        public void SortAccountingData() {
            AccountingDataList.Sort(CurrentComparision);
        }

        public void ConfigureColumnDirIndicators(BitmapIcon visibleIndicator) {
            foreach (var indicator in ColumnDirIndicators)
                indicator.Visibility = indicator == visibleIndicator ? Visibility.Visible : Visibility.Collapsed;
        }

        public async Task<bool> ExportData() {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as.
            savePicker.FileTypeChoices.Add("엑셀 통합 파일(.xlsx)", new List<string>() { ".xlsx" });
            savePicker.FileTypeChoices.Add("텍스트 파일(.txt)", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace.
            savePicker.SuggestedFileName = "새 파일";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file == null) return true;
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
                foreach (AccountingData data in AccountingDataList) {
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
            if (status != FileUpdateStatus.Complete) return false;

            return true;
        }

        public AccountingDataListNavParams NavParams { get; set; }

        private double DefaultColumnWidth { get => 110; }
        public double DefaultMinColumnWidth { get => 100; }

        private GridLength dataTypeColumnWidth;
        public GridLength DataTypeColumnWidth {
            get => dataTypeColumnWidth;
            set => SetProperty(ref dataTypeColumnWidth, value);
        }

        private GridLength clientNameColumnWidth;
        public GridLength ClientNameColumnWidth {
            get => clientNameColumnWidth;
            set => SetProperty(ref clientNameColumnWidth, value);
        }

        private GridLength dateColumnWidth;
        public GridLength DateColumnWidth {
            get => dateColumnWidth;
            set => SetProperty(ref dateColumnWidth, value);
        }

        private GridLength steelWeightColumnWidth;
        public GridLength SteelWeightColumnWidth {
            get => steelWeightColumnWidth;
            set => SetProperty(ref steelWeightColumnWidth, value);
        }

        private GridLength supplyPriceColumnWidth;
        public GridLength SupplyPriceColumnWidth {
            get => supplyPriceColumnWidth;
            set => SetProperty(ref supplyPriceColumnWidth, value);
        }

        private GridLength taxAmountColumnWidth;
        public GridLength TaxAmountColumnWidth {
            get => taxAmountColumnWidth;
            set => SetProperty(ref taxAmountColumnWidth, value);
        }

        private GridLength sumColumnWidth;
        public GridLength SumColumnWidth {
            get => sumColumnWidth;
            set => SetProperty(ref sumColumnWidth, value);
        }

        private GridLength depositConfirmColumnWidth;
        public GridLength DepositConfirmColumnWidth {
            get => depositConfirmColumnWidth;
            set => SetProperty(ref depositConfirmColumnWidth, value);
        }

        public AccountingData SelectedData { get; set; }
        public bool DataIsSelected { get => SelectedData != null; }

        public int? SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }

        public string LookingForClietnName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsForSearching { get => BeginDate != null && EndDate != null; }

        public List<AccountingData> AccountingDataList { get; set; }
        public Comparison<AccountingData> CurrentComparision { get; set; }

        public List<BitmapIcon> ColumnDirIndicators { get; }

        public bool DataEditted { get; set; }
    }
}
