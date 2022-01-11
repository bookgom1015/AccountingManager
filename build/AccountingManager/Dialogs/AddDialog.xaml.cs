using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using AccountingManager.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs
{
    public sealed partial class AddDialog : ContentDialog
    {
        public AddDialog(AccountingDataControls controls)
        {
            this.InitializeComponent();

            SolidColorBrush red = new SolidColorBrush(Colors.Red);

            controls.InputType.MinWidth = 0;
            controls.InputType.Margin = new Thickness(0);
            controls.InputType.VerticalAlignment = VerticalAlignment.Center;

            controls.InputName.PlaceholderForeground = red;
            controls.InputWeight.PlaceholderForeground = red;
            controls.InputPrice.PlaceholderForeground = red;
            controls.InputTax.PlaceholderForeground = red;

            controls.InputConfirm.MinWidth = 0;
            controls.InputConfirm.Margin = new Thickness(0);
            controls.InputConfirm.VerticalAlignment = VerticalAlignment.Center;

            controls.InputYear.Margin = new Thickness(0, 0, 5, 0);
            controls.InputMonth.Margin = new Thickness(0, 0, 5, 0);

            mInputName = controls.InputName;
            mInputWeight = controls.InputWeight;
            mInputPrice = controls.InputPrice;
            mInputTax = controls.InputTax;

            NamePanel.Children.Add(controls.InputName);
            WeightPanel.Children.Add(controls.InputWeight);
            PricePanel.Children.Add(controls.InputPrice);
            TaxPanel.Children.Add(controls.InputTax);
            DatePanel.Children.Add(controls.InputYear);
            DatePanel.Children.Add(controls.InputMonth);
            DatePanel.Children.Add(controls.InputDay);
            TypePanel.Children.Add(controls.InputType);
            DepositPanel.Children.Add(controls.InputConfirm);
        }

        private void CancelReturn(ContentDialogButtonClickEventArgs args, TextBox textBox, string msg)
        {
            args.Cancel = true;
            textBox.PlaceholderText = msg;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (mInputName.Text.Length == 0) CancelReturn(args, mInputName, "* 거래처명을 입력해주십시오.");
            else if (mInputWeight.Text.Length == 0) CancelReturn(args, mInputWeight, "* 철강무게를 입력해주십시오.");
            else if (mInputPrice.Text.Length == 0) CancelReturn(args, mInputPrice, "* 공급가격을 입력해주십시오.");
            else if (mInputTax.Text.Length == 0) CancelReturn(args, mInputTax, "* 세액을 입력해주십시오.");
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void IncreaseAmount(TextBox inTextBox, int inAmount)
        {
            int price;
            int.TryParse(inTextBox.Text, out price);

            price += inAmount;

            inTextBox.Text = price.ToString();
        }

        private void PriceHudred_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputPrice, 1000000);
        }

        private void PriceTen_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputPrice, 100000);
        }

        private void PriceOne_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputPrice, 10000);
        }

        private void TaxHundred_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputTax, 1000000);
        }

        private void TaxTen_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputTax, 100000);
        }

        private void TaxOne_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(mInputTax, 10000);
        }

        private TextBox mInputName;
        private TextBox mInputWeight;
        private TextBox mInputPrice;
        private TextBox mInputTax;
    }
}
