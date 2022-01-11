using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AccountingManager.ViewModels;
using AccountingManager.Helpers;

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
            yearTextBlock.Margin = new Thickness(25, 0, 0, 0);
            yearTextBlock.FontSize = 22;
            yearTextBlock.Text = year;

            DateListBox.Items.Add(yearTextBlock);

            foreach (int month in pair.Value)
            {
                TextBlock dateTextBlock = new TextBlock();
                dateTextBlock.Margin = new Thickness(40, 0, 0, 0);
                dateTextBlock.FontSize = 18;
                dateTextBlock.Text = year + "/" + month.ToString("D2");

                DateListBox.Items.Add(dateTextBlock);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MonthlyNavPageParams)
            {
                MonthlyNavPageParams navParams = e.Parameter as MonthlyNavPageParams;

                ViewModel.DataLitView_SelectionChanged = navParams.DataLitView_SelectionChanged;
                GenerateDateList(navParams.Pair);
            }

            base.OnNavigatedTo(e);
        }

        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            TryGoBack(this.Frame);
        }

        private void DateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.DataLitView_SelectionChanged != null)
            {
                ListBox listBox = sender as ListBox;
                if (listBox == null) return;

                if (!(listBox.SelectedItem is TextBlock)) return;
                TextBlock textBlock = listBox.SelectedItem as TextBlock;

                ViewModel.DataLitView_SelectionChanged(textBlock.Text);
            }
        }
    }
}
