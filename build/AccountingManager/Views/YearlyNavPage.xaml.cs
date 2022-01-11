using System;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

using AccountingManager.ViewModels;
using AccountingManager.Helpers;

namespace AccountingManager.Views
{
    public sealed partial class YearlyNavPage : Page
    {
        private YearlyNavViewModel ViewModel => DataContext as YearlyNavViewModel;

        public YearlyNavPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is YearlyNavPageParams)
            {
                YearlyNavPageParams navParams = e.Parameter as YearlyNavPageParams;
                ViewModel.DateMap = navParams.DateMap;

                ViewModel.DataLitView_SelectionChanged = navParams.DataLitView_SelectionChanged;
            }            

            base.OnNavigatedTo(e);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null) return;

            if (!(listBox.SelectedItem is KeyValuePair<int, List<int>>)) return;
            KeyValuePair<int, List<int>> pair = (KeyValuePair<int, List<int>>)listBox.SelectedItem;

            MonthlyNavPageParams navParams = new MonthlyNavPageParams();
            navParams.Pair = pair;
            navParams.DataLitView_SelectionChanged = ViewModel.DataLitView_SelectionChanged;

            this.Frame.Navigate(typeof(MonthlyNavPage), navParams, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
