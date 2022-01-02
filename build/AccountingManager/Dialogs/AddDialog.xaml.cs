using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using AccountingManager.Helpers;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs
{
    public sealed partial class AddDialog : ContentDialog
    {
        public AddDialog(AccountingDataControls controls)
        {
            this.InitializeComponent();

            Panel.Children.Add(controls.InputName);
            Panel.Children.Add(controls.InputPrice);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
