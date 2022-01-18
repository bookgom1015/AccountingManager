using System;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Helpers
{
    public class AddYearDialogControls
    {
        public AddYearDialogControls()
        {
            mInputYear = new ComboBox();

            for (int i = 2000; i < 2100; ++i)
                mInputYear.Items.Add(i);

            DateTime localDate = DateTime.Now;

            // Set SelectedIndex to the current month.
            string yearText = localDate.ToString("yyyy");

            int year;
            int.TryParse(yearText, out year);

            mInputYear.SelectedIndex = year - 2000;
        }

        private ComboBox mInputYear;
        public ComboBox InputYear { get => mInputYear; }
    }
}
