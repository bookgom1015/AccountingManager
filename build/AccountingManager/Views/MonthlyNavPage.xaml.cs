using System;
using System.Collections.Generic;

using AccountingManager.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AccountingManager.Views
{
    public sealed partial class MonthlyNavPage : Page
    {
        private MonthlyNavViewModel ViewModel => DataContext as MonthlyNavViewModel;

        public MonthlyNavPage()
        {
            InitializeComponent();
        }

        private bool TryGoBack(Frame frame)
        {
            if (!frame.CanGoBack) return false;

            frame.GoBack();

            return true;
        }

        public void GenerateDateList(KeyValuePair<int, List<int>> pair)
        {
            string year = pair.Key.ToString();

            TextBlock yearTextBlock = new TextBlock();
            yearTextBlock.Margin = new Thickness(10, 0, 0, 0);
            yearTextBlock.FontSize = 22;
            yearTextBlock.Text = year;

            DateListBox.Items.Add(yearTextBlock);

            foreach (int month in pair.Value)
            {
                TextBlock dateTextBlock = new TextBlock();
                dateTextBlock.Margin = new Thickness(25, 0, 0, 0);
                dateTextBlock.FontSize = 18;
                dateTextBlock.Text = year + "/" + month.ToString("D2");

                DateListBox.Items.Add(dateTextBlock);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is KeyValuePair<int, List<int>>)
                GenerateDateList((KeyValuePair<int, List<int>>)e.Parameter);

            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TryGoBack(this.Frame);
        }
    }
}
