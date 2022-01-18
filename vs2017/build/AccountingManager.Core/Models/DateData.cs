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
            Months = new List<int>();
        }

        public DateData(int year, List<int> months)
        {
            Year = year;
            Months = months;
        }

        private int mYear;
        public int Year
        {
            get => mYear;
            set => mYear = value;
        }

        private List<int> mMonths;
        public List<int> Months
        {
            get => mMonths;
            set => mMonths = value;
        }
    }
}
