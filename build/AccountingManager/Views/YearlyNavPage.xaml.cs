using System;
using System.Collections.Generic;

using AccountingManager.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AccountingManager.Views
{
    public sealed partial class YearlyNavPage : Page
    {
        private YearlyNavViewModel ViewModel => DataContext as YearlyNavViewModel;

        public YearlyNavPage()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null) return;

            if (!(listBox.SelectedItem is KeyValuePair<int, List<int>>)) return;
            KeyValuePair<int, List<int>> pair = (KeyValuePair<int, List<int>>)listBox.SelectedItem;

            this.Frame.Navigate(typeof(MonthlyNavPage), pair, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
