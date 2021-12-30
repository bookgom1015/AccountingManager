using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Core.Models
{
    public class DateData
    {
        public DateData()
        {
            Year = default;
            Months = new List<string>();
        }

        public DateData(string year, List<string> months)
        {
            Year = year;
            Months = months;
        }

        private string mYear;
        public string Year
        {
            get => mYear;
            set => mYear = value;
        }

        private List<string> mMonths;
        public List<string> Months
        {
            get => mMonths;
            set => mMonths = value;
        }
    }
}
