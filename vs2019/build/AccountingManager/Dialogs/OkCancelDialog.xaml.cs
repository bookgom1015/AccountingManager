using Windows.UI.Xaml.Controls;

namespace AccountingManager.Dialogs
{
    public sealed partial class OkCancelDialog : ContentDialog
    {
        public OkCancelDialog(string text, int fontSize = 0)
        {
            this.InitializeComponent();

            NoticeBoard.Text = text;
            if (fontSize != 0) NoticeBoard.FontSize = fontSize;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {}

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {}
    }
}
