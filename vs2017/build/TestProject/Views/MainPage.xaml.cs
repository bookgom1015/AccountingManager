using System;

using TestProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace TestProject.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SubPage), InputName.Text);
        }
    }
}
