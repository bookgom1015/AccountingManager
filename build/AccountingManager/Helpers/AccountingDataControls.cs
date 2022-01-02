using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Helpers
{
    public class AccountingDataControls
    {
        public AccountingDataControls()
        {
            InputName = new TextBox();

            InputPrice = new TextBox();
            InputPrice.BeforeTextChanging += TextBox_BeforeTextChanging;

            InputTax = new TextBox();
            InputTax.BeforeTextChanging += TextBox_BeforeTextChanging;

            InputYear = new ComboBox();
            for (int i = 2000; i < 2100; ++i)
                InputYear.Items.Add(i);

            InputType = new CheckBox();
            InputCheck = new CheckBox();
        }

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

        private CheckBox mInputType;
        public CheckBox InputType
        {
            get => mInputType;
            set => mInputType = value;
        }

        private CheckBox mInputCheck;
        public CheckBox InputCheck
        {
            get => mInputCheck;
            set => mInputCheck = value;
        }
    }
}
