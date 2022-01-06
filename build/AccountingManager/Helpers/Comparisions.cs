using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AccountingManager.Core.Models;

namespace AccountingManager.Helpers
{
    public class Comparisions
    {
        public static int CompareType(AccountingData a, AccountingData b)
        {
            if (a.DataType == b.DataType)
                return 0;
            else if (a.DataType && !b.DataType)
                return 1;
            else
                return -1;
        }

        public static int CompareName(AccountingData a, AccountingData b)
        {
            return string.Compare(a.ClientName, b.ClientName);
        }

        public static int CompareDate(AccountingData a, AccountingData b)
        {
            return string.Compare(a.Date, b.Date);
        }

        public static int CompareWeight(AccountingData a, AccountingData b)
        {
            if (a.SteelWeight > b.SteelWeight)
                return 1;
            else if (a.SteelWeight == b.SteelWeight)
                return 0;
            else
                return -1;
        }

        public static int ComparePrice(AccountingData a, AccountingData b)
        {
            if (a.SupplyPrice > b.SupplyPrice)
                return 1;
            else if (a.SupplyPrice == b.SupplyPrice)
                return 0;
            else
                return -1;
        }

        public static int CompareTax(AccountingData a, AccountingData b)
        {
            if (a.TaxAmount > b.TaxAmount)
                return 1;
            else if (a.TaxAmount == b.TaxAmount)
                return 0;
            else
                return -1;
        }

        public static int CompareSum(AccountingData a, AccountingData b)
        {
            int sumA = a.SupplyPrice + a.TaxAmount;
            int sumB = b.SupplyPrice + b.TaxAmount;
            if (sumA > sumB)
                return 1;
            else if (sumA == sumB)
                return 0;
            else
                return -1;
        }

        public static int CompareConfirm(AccountingData a, AccountingData b)
        {
            if (a.DepositConfirm == b.DepositConfirm)
                return 0;
            else if (a.DepositConfirm && !b.DepositConfirm)
                return 1;
            else
                return -1;
        }
    }
}
