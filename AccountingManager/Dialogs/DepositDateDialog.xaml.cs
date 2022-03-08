using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using AccountingManager.Core.Models;
using AccountingManager.Dialogs.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs {
    public sealed partial class DepositDateDialog : ContentDialog {
        public DepositDateDialog(DepositDateControls controls) {
            this.InitializeComponent();

            this.controls = controls;
            DepositDate.Date = DateTime.Now;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            AccountingData original = controls.AccountingData;

            AccountingData data = new AccountingData {
                Uid = original.Uid,
                ClientName = original.ClientName,
                Date = original.Date,
                SteelWeight = original.SteelWeight,
                SupplyPrice = original.SupplyPrice,
                TaxAmount = original.TaxAmount,
                DataType = original.DataType,
                DepositConfirmed = true,
                DepositDate = DepositDate.Date.ToString("yyyy-MM-dd")
            };

            controls.AccountingData = data;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) { }

        DepositDateControls controls;
    }
}
