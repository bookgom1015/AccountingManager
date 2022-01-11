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

                ViewModel.YearList_SelectionChanged = navParams.YearList_SelectionChanged;

                foreach (int year in navParams.YearList)
                    YearListBox.Items.Add(year.ToString());
            }

            base.OnNavigatedTo(e);
        }

        private void YearListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListBox)) return;
            ListBox listBox = sender as ListBox;

            ViewModel.YearList_SelectionChanged(listBox.SelectedItem as string);
        }
    }
}
