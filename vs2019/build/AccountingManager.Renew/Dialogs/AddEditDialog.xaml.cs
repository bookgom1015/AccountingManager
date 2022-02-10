using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using AccountingManager.Renew.Core.Models;
using AccountingManager.Renew.Dialogs.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Renew.Dialogs {
    public sealed partial class AddEditDialog : ContentDialog {
        public AddEditDialog(AddEditControls controls) {
            this.InitializeComponent();

            this.controls = controls;

            InputDate.MinYear = DateTimeOffset.Now.AddYears(-100);
            InputDate.MaxYear = DateTimeOffset.Now.AddYears(100);

            InputDepositDate.MinYear = DateTimeOffset.Now.AddYears(-100);
            InputDepositDate.MaxYear = DateTimeOffset.Now.AddYears(100);

            if (controls.AccountingData != null) {
                AccountingData data = controls.AccountingData;

                InputDate.Date = new DateTime(data.Year, data.Month, data.Day);
                InputClientName.Text = data.ClientName;
                InputSteelWeight.Text = data.SteelWeight.ToString();
                InputSupplyPrice.Text = data.SupplyPrice.ToString();
                InputTaxAmount.Text = data.TaxAmount.ToString();
                InputDepositConfirmed.IsChecked = data.DepositConfirmed;
                InputDataType.IsChecked = data.DataType;
                InputDepositDate.Date = data.DepositConfirmed ? Convert.ToDateTime(data.DepositDate) : DateTime.Now;
            }
            else {
                InputDate.Date = DateTimeOffset.Now;
                InputDepositDate.Date = DateTimeOffset.Now;
            }
        }

        private void CancelReturn(ContentDialogButtonClickEventArgs args, TextBox textBox, string msg) {
            args.Cancel = true;
            textBox.PlaceholderText = msg;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (InputClientName.Text.Length == 0) {
                CancelReturn(args, InputClientName, "* 거래처를 입력해주십시오");
                return;
            }
            if (InputSteelWeight.Text.Length == 0) {
                CancelReturn(args, InputSteelWeight, "* 중량을 입력해주십시오");
                return;
            }
            if (InputSupplyPrice.Text.Length == 0) {
                CancelReturn(args, InputSupplyPrice, "* 공급가를 입력해주십시오");
                return;
            }
            if (InputTaxAmount.Text.Length == 0) {
                CancelReturn(args, InputTaxAmount, "* 세액을 입력해주십시오");
                return;
            }

            DateTime date = new DateTime(InputDate.Date.Year, InputDate.Date.Month, InputDate.Date.Day);
            
            float steelWeight = float.Parse(InputSteelWeight.Text);
            uint supplyPrice = Convert.ToUInt32(InputSupplyPrice.Text);
            uint taxAmount = Convert.ToUInt32(InputTaxAmount.Text);

            bool dataType = (bool)InputDataType.IsChecked;

            bool depositConfirmed = (bool)InputDepositConfirmed.IsChecked;
            string depositDate = depositConfirmed ? new DateTime(InputDepositDate.Date.Year, InputDepositDate.Date.Month, InputDepositDate.Date.Day).ToString("yyyy/MM/dd") : "";

            AccountingData data = new AccountingData {
                ClientName = InputClientName.Text,
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

            controls.AccountingData = data;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        private void OnlyRealNumberTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args) {
            if (args.NewText.Length == 0) return;

            float temp;
            args.Cancel = !float.TryParse(args.NewText, out temp);
        }

        private void OnlyDigitTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args) {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void IncreaseAmount(TextBox inTextBox, int inAmount) {
            int price;
            int.TryParse(inTextBox.Text, out price);

            price += inAmount;

            inTextBox.Text = price.ToString();
        }

        private void IncSupplyPriceOneMillion_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputSupplyPrice, 1000000);
        }

        private void IncSupplyPriceHundredThousand_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputSupplyPrice, 100000);
        }

        private void IncSupplyPriceTenThousand_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputSupplyPrice, 10000);
        }

        private void IncTaxAmountOneMillion_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputTaxAmount, 1000000);
        }

        private void IncTaxAmountHundredenThousand_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputTaxAmount, 100000);
        }

        private void IncTaxAmountTenThousand_Click(object sender, RoutedEventArgs e) {
            IncreaseAmount(InputTaxAmount, 10000);
        }

        private void InputDataType_Click(object sender, RoutedEventArgs e) {
            if (InputDataType.IsChecked == true) InputDataType.Content = "매입";
            else InputDataType.Content = "매출";
        }

        private AddEditControls controls;
    }
}
