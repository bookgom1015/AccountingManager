using System;
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
            
            string year = localDate.ToString("yy");

            // Set SelectedIndex to the current year.
            int yearIdx;
            int.TryParse(year, out yearIdx);
            InputYear.SelectedIndex = yearIdx;

            //
            // Generate ComboBox for entering a month.
            //
            InputMonth = new ComboBox();
            for (int i = 1; i <= 12; ++i)
                InputMonth.Items.Add(i);
            
            string month = localDate.ToString("MM");

            // Set SelectedIndex to the current month.
            int monthIdx;
            int.TryParse(month, out monthIdx);            
            InputMonth.SelectedIndex = monthIdx - 1;

            //
            // Generate ComboBox for entering a day.
            //
            InputDay = new ComboBox();
            int endDay;
            if (monthIdx < 8)
            {
                if (monthIdx % 2 == 0)
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
                if (monthIdx % 2 == 0)
                {
                    endDay = 31;
                }
                else
                {
                    endDay = 30;
                }
            }
            
            for (int i = 1; i <= endDay; ++i)
            {
                InputDay.Items.Add(i);
            }
            
            string day = localDate.ToString("dd");

            // Set SelectedIndex to the current day.
            int dayIdx;
            int.TryParse(day, out dayIdx);            
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
    }
}
