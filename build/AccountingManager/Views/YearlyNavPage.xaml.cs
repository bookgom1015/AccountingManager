using System;
using System.Collections.Generic;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            YearlyNavPageParams navParams = e.Parameter as YearlyNavPageParams;
            if (navParams != null)
            {
                ViewModel.YearList_SelectionChanged = navParams.YearList_SelectionChanged;

                if (navParams.YearList != null)
                {
                    foreach (int year in navParams.YearList)
                        YearListBox.Items.Add(year.ToString());
                }
            }

            base.OnNavigatedTo(e);
        }

        private void YearListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null) return;

            if (ViewModel.YearList_SelectionChanged != null)
                ViewModel.YearList_SelectionChanged(listBox.SelectedItem as string);
        }
    }
}
