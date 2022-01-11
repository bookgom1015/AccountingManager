using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Helpers
{
    public class AccountingDataControls
    {
        public AccountingDataControls()
        {
            InputName = new TextBox();

            InputWeight = new TextBox();
            InputWeight.BeforeTextChanging += TextBox_BeforeTextChanging;

            InputPrice = new TextBox();
            InputPrice.BeforeTextChanging += TextBox_BeforeTextChanging;

            InputTax = new TextBox();
            InputTax.BeforeTextChanging += TextBox_BeforeTextChanging;

            //
            // Generate ComboBox for entering an year.
            //
            InputYear = new ComboBox();
            for (int i = 2000; i < 2100; ++i)
                InputYear.Items.Add(i);

            DateTime localDate = DateTime.Now;

            // Set SelectedIndex to the current year.
            string year = localDate.ToString("yy");
            int yearIdx;
            int.TryParse(year, out yearIdx);
            InputYear.SelectedIndex = yearIdx;

            //
            // Generate ComboBox for entering a month.
            //
            InputMonth = new ComboBox();
            for (int i = 1; i <= 12; ++i)
                InputMonth.Items.Add(i);

            // Set SelectedIndex to the current month.
            string monthText = localDate.ToString("MM");
            int monthIdx;
            int.TryParse(monthText, out monthIdx);            
            InputMonth.SelectedIndex = monthIdx - 1;
            InputMonth.SelectionChanged += ComboBox_SelectionChanged;

            //
            // Generate ...
            //
            MonthList = new List<List<int>>();

            for (int month = 1; month <= 12; ++month)
            {
                int endDay;

                if (month == 2)
                {
                    endDay = 28;
                }
                else if (month < 8)
                {
                    if (month % 2 == 0)
                    {
                        endDay = 30;
                    }
                    else
                    {
                        endDay = 31;
                    }
                }
                else
                {
                    if (month % 2 == 0)
                    {
                        endDay = 31;
                    }
                    else
                    {
                        endDay = 30;
                    }
                }

                List<int> dayList = new List<int>();

                for (int i = 1; i <= endDay; ++i)
                    dayList.Add(i);

                MonthList.Add(dayList);
            }

            //
            // Generate ComboBox for entering a day.
            //
            InputDay = new ComboBox();
            {
                List<int> dayList = MonthList[monthIdx - 1];
                InputDay.ItemsSource = dayList;
            }

            // Set SelectedIndex to the current day.
            string dayText = localDate.ToString("dd");
            int dayIdx;
            int.TryParse(dayText, out dayIdx);            
            InputDay.SelectedIndex = dayIdx - 1;

            InputType = new ComboBox();
            InputType.Items.Add("매입");
            InputType.Items.Add("매출");
            InputType.SelectedIndex = 0;
            
            InputConfirm = new CheckBox();
        }

        //* Only input digits.
        private void TextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox)) return;

            ComboBox comboBox = sender as ComboBox;
            int monthIndex = comboBox.SelectedIndex;

            List<int> dayList = MonthList[monthIndex];
            int prevIndex = InputDay.SelectedIndex;
            int count = dayList.Count;

            InputDay.ItemsSource = dayList;
            InputDay.SelectedIndex = prevIndex >= count ? count -1 : prevIndex;

        }

        private TextBox mInputName;
        public TextBox InputName
        {
            get => mInputName;
            set => mInputName = value;
        }

        private TextBox mInputWeight;
        public TextBox InputWeight
        {
            get => mInputWeight;
            set => mInputWeight = value;
        }

        private TextBox mInputPrice;
        public TextBox InputPrice
        {
            get => mInputPrice;
            set => mInputPrice = value;
        }

        private TextBox mInputTax;
        public TextBox InputTax
        {
            get => mInputTax;
            set => mInputTax = value;
        }

        private ComboBox mInputYear;
        public ComboBox InputYear
        {
            get => mInputYear;
            set => mInputYear = value;
        }

        private ComboBox mInputMonth;
        public ComboBox InputMonth
        {
            get => mInputMonth;
            set => mInputMonth = value;
        }

        private ComboBox mInputDay;
        public ComboBox InputDay
        {
            get => mInputDay;
            set => mInputDay = value;
        }

        private ComboBox mInputType;
        public ComboBox InputType
        {
            get => mInputType;
            set => mInputType = value;
        }

        private CheckBox mInputConfirm;
        public CheckBox InputConfirm
        {
            get => mInputConfirm;
            set => mInputConfirm = value;
        }

        private List<List<int>> mMonthList;
        public List<List<int>> MonthList
        {
            get => mMonthList;
            set => mMonthList = value;
        }
    }
}
