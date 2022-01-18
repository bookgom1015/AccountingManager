using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using AccountingManager.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs
{
    public sealed partial class AddYearDialog : ContentDialog
    {
        public AddYearDialog(AddYearDialogControls inControls)
        {
            this.InitializeComponent();

            inControls.InputYear.VerticalAlignment = VerticalAlignment.Center;

            YearPanel.Children.Add(inControls.InputYear);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
