﻿using Windows.UI.Xaml.Controls;

namespace AccountingManager.Dialogs {
    public sealed partial class AlertDialog : ContentDialog {
        public AlertDialog(string text, int fontSize = 0) {
            this.InitializeComponent();

            NoticeBoard.Text = text;
            if (fontSize != 0) NoticeBoard.FontSize = fontSize;
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {}
    }
}
