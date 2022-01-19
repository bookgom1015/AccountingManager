using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using AccountingManager.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs
{
    public sealed partial class AddEditDataDialog : ContentDialog
    {
        public AddEditDataDialog(AddEditDialogControls controls)
        {
            this.InitializeComponent();

            // Diable scaling.
            Windows.UI.ViewManagement.ApplicationViewScaling.TrySetDisableLayoutScaling(true);

            SolidColorBrush red = new SolidColorBrush(Colors.Red);

            controls.InputDataType.MinWidth = 0;
            controls.InputDataType.Margin = new Thickness(0);
            controls.InputDataType.VerticalAlignment = VerticalAlignment.Center;

            controls.InputClientName.PlaceholderForeground = red;
            controls.InputSteelWeight.PlaceholderForeground = red;
            controls.InputSupplyPrice.PlaceholderForeground = red;
            controls.InputTaxAmount.PlaceholderForeground = red;

            controls.InputDepositConfirm.MinWidth = 0;
            controls.InputDepositConfirm.Margin = new Thickness(0);
            controls.InputDepositConfirm.VerticalAlignment = VerticalAlignment.Center;

            controls.InputMonth.Margin = new Thickness(0, 0, 5, 0);

            mInputName = controls.InputClientName;
            mInputWeight = controls.InputSteelWeight;
            mInputPrice = controls.InputSupplyPrice;
            mInputTax = controls.InputTaxAmount;

            NamePanel.Children.Add(controls.InputClientName);
            WeightPanel.Children.Add(controls.InputSteelWeight);
            PricePanel.Children.Add(controls.InputSupplyPrice);
            TaxPanel.Children.Add(controls.InputTaxAmount);
            MonthPanel.Children.Add(controls.InputMonth);
            DayPanel.Children.Add(controls.InputDay);
            TypePanel.Children.Add(controls.InputDataType);
            DepositPanel.Children.Add(controls.InputDepositConfirm);
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
