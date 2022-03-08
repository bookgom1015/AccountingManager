using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

using AccountingManager.Dialogs.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AccountingManager.Dialogs {
    public sealed partial class LoginDialog : ContentDialog {
        public LoginDialog(LoginControls controls) {
            this.InitializeComponent();

            this.controls = controls;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            if (UserName.Text.Length == 0) {
                args.Cancel = true;

                UserNameStatus.Text = "* 아이디를 입력해주십시오";
            }

            if (Password.Password.Length == 0) {
                args.Cancel = true;

                PasswordStatus.Text = "* 비밀번호를 입력해주십시오";
            }

            if (UserName.Text.Length != 0 && Password.Password.Length != 0) {
                controls.UserName = UserName.Text;
                controls.Password = Password.Password;
            }
        }

        private void Input_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                
            }
        }

        private LoginControls controls;
    }
}
