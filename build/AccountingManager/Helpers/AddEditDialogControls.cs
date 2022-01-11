using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Helpers
{
    public class AddEditDialogControls
    {
        public AddEditDialogControls()
        {
            mInputClientName = new TextBox();

            mInputSteelWeight = new TextBox();
            mInputSteelWeight.BeforeTextChanging += TextBox_BeforeTextChanging;

            mInputSupplyPrice = new TextBox();
            mInputSupplyPrice.BeforeTextChanging += TextBox_BeforeTextChanging;

            mInputTaxAmount = new TextBox();
            mInputTaxAmount.BeforeTextChanging += TextBox_BeforeTextChanging;

            DateTime localDate = DateTime.Now;

            //
            // Generate ComboBox for entering a month.
            //
            mInputMonth = new ComboBox();
            for (int i = 1; i <= 12; ++i)
                InputMonth.Items.Add(i);

            // Set SelectedIndex to the current month.
            string monthText = localDate.ToString("MM");
            int monthIdx;
            int.TryParse(monthText, out monthIdx);            
            mInputMonth.SelectedIndex = monthIdx - 1;
            mInputMonth.SelectionChanged += ComboBox_SelectionChanged;

            //
            // Generate ...
            //
            mMonthList = new List<List<int>>();

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
            mInputDay = new ComboBox();
            {
                List<int> dayList = MonthList[monthIdx - 1];
                InputDay.ItemsSource = dayList;
            }

            // Set SelectedIndex to the current day.
            string dayText = localDate.ToString("dd");
            int dayIdx;
            int.TryParse(dayText, out dayIdx);            
            InputDay.SelectedIndex = dayIdx - 1;

            mInputDataType = new ComboBox();
            mInputDataType.Items.Add("매입");
            mInputDataType.Items.Add("매출");
            mInputDataType.SelectedIndex = 0;
            
            mInputDepositConfirm = new CheckBox();
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

        private TextBox mInputClientName;
        public TextBox InputClientName { get => mInputClientName; }

        private TextBox mInputSteelWeight;
        public TextBox InputSteelWeight { get => mInputSteelWeight; }

        private TextBox mInputSupplyPrice;
        public TextBox InputSupplyPrice { get => mInputSupplyPrice; }

        private TextBox mInputTaxAmount;
        public TextBox InputTaxAmount { get => mInputTaxAmount; }

        private ComboBox mInputMonth;
        public ComboBox InputMonth { get => mInputMonth; }

        private ComboBox mInputDay;
        public ComboBox InputDay { get => mInputDay; }

        private ComboBox mInputDataType;
        public ComboBox InputDataType { get => mInputDataType; }

        private CheckBox mInputDepositConfirm;
        public CheckBox InputDepositConfirm { get => mInputDepositConfirm; }

        private List<List<int>> mMonthList;
        public List<List<int>> MonthList { get => mMonthList; }
    }
}
