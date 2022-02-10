using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using AccountingManager.Renew.Core.Models;

namespace AccountingManager.Renew.Core.Helpers {
    public class AccountingDataComparisions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareType(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) {
                return 1;
            }
            else if (dateA < dateB) {
                return -1;
            }
            else {
                int nameCompResult = string.Compare(a.ClientName, b.ClientName);

                if (nameCompResult == 0) {
                    if (a.SteelWeight == b.SteelWeight) return 0;
                    else if (a.SteelWeight > b.SteelWeight) return 1;
                    else return -1;
                }
                else {
                    return nameCompResult;
                }
            }
        }

        public static int CompareType(AccountingData a, AccountingData b) {
            if (a.DataType == b.DataType) return InlineCompareType(a, b);
            else if (a.DataType && !b.DataType) return 1;
            else return -1;
        }

        public static int CompareTypeReverse(AccountingData a, AccountingData b) {
            if (a.DataType == b.DataType) return InlineCompareType(a, b);
            else if (a.DataType && !b.DataType) return -1;
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareName(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) {
                return 1;
            }
            else if (dateA < dateB) {
                return -1;
            }
            else {
                if (a.SteelWeight == b.SteelWeight) return 0;
                else if (a.SteelWeight > b.SteelWeight) return 1;
                else return -1;
            }
        }

        public static int CompareName(AccountingData a, AccountingData b) {
            int nameCompResult = string.Compare(a.ClientName, b.ClientName);

            if (nameCompResult == 0) return InlineCompareName(a, b);
            else return nameCompResult;
        }

        public static int CompareNameReverse(AccountingData a, AccountingData b) {
            int nameCompResult = string.Compare(b.ClientName, a.ClientName);

            if (nameCompResult == 0) return InlineCompareName(a, b);
            else return nameCompResult;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareDate(AccountingData a, AccountingData b) {
            int nameCompResult = string.Compare(a.ClientName, b.ClientName);

            if (nameCompResult == 0) {
                if (a.SteelWeight == b.SteelWeight) return 0;
                else if (a.SteelWeight > b.SteelWeight) return 1;
                else return -1;
            }
            else return nameCompResult;
        }

        public static int CompareDate(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return 1;
            else if (dateA < dateB) return -1;
            else return InlineCompareDate(a, b);
        }

        public static int CompareDateReverse(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return -1;
            else if (dateA < dateB) return 1;
            else return InlineCompareDate(a, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareWeight(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return -1;
            else if (dateA < dateB) return 1;
            else return string.Compare(a.ClientName, b.ClientName);
        }

        public static int CompareWeight(AccountingData a, AccountingData b) {
            if (a.SteelWeight == b.SteelWeight) return InlineCompareWeight(a, b);
            else if (a.SteelWeight > b.SteelWeight) return 1;
            else return -1;
        }

        public static int CompareWeightReverse(AccountingData a, AccountingData b) {
            if (a.SteelWeight == b.SteelWeight) return InlineCompareWeight(a, b);
            else if (a.SteelWeight > b.SteelWeight) return -1;
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineComparePrice(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return -1;
            else if (dateA < dateB) return 1;
            else return string.Compare(a.ClientName, b.ClientName);
        }

        public static int ComparePrice(AccountingData a, AccountingData b) {
            if (a.SupplyPrice == b.SupplyPrice) return InlineComparePrice(a, b);
            else if (a.SupplyPrice > b.SupplyPrice) return 1;
            else return -1;
        }

        public static int ComparePriceReverse(AccountingData a, AccountingData b) {
            if (a.SupplyPrice == b.SupplyPrice) return InlineComparePrice(a, b);
            else if (a.SupplyPrice > b.SupplyPrice) return -1;
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareTax(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return -1;
            else if (dateA < dateB) return 1;
            else return string.Compare(a.ClientName, b.ClientName);;
        }

        public static int CompareTax(AccountingData a, AccountingData b) {
            if (a.TaxAmount == b.TaxAmount) return InlineCompareTax(a, b);
            else if (a.TaxAmount > b.TaxAmount) return 1;
            else return -1;
        }

        public static int CompareTaxReverse(AccountingData a, AccountingData b) {
            if (a.TaxAmount == b.TaxAmount) return InlineCompareTax(a, b);
            else if (a.TaxAmount > b.TaxAmount) return -1;
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareSum(AccountingData a, AccountingData b) {
            DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
            DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

            if (dateA > dateB) return -1;
            else if (dateA < dateB) return 1;
            else return string.Compare(a.ClientName, b.ClientName); ;
        }

        public static int CompareSum(AccountingData a, AccountingData b) {
            uint sumA = a.SupplyPrice + a.TaxAmount;
            uint sumB = b.SupplyPrice + b.TaxAmount;

            if (sumA == sumB) return InlineCompareSum(a, b);
            else if (sumA > sumB) return 1;
            else return -1;
        }

        public static int CompareSumReverse(AccountingData a, AccountingData b) {
            uint sumA = a.SupplyPrice + a.TaxAmount;
            uint sumB = b.SupplyPrice + b.TaxAmount;

            if (sumA == sumB) return InlineCompareSum(a, b);
            else if (sumA > sumB) return -1;
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int InlineCompareConfirm(AccountingData a, AccountingData b) {
            int depositDateCompResult = string.Compare(a.DepositDate, b.DepositDate);

            if (depositDateCompResult == 0) {
                int nameCompResult = string.Compare(a.ClientName, b.ClientName);

                if (nameCompResult == 0) {
                    DateTime dateA = new DateTime(a.Year, a.Month, a.Day);
                    DateTime dateB = new DateTime(b.Year, b.Month, b.Day);

                    if (dateA > dateB) return 1;
                    else if (dateA < dateB) return 1;
                    else return 0;
                }
                else {
                    return nameCompResult;
                }
            }
            else {
                return depositDateCompResult;
            }
        }

        public static int CompareConfirm(AccountingData a, AccountingData b) {
            if (a.DepositConfirmed == b.DepositConfirmed) return InlineCompareConfirm(a, b);
            else if (a.DepositConfirmed && !b.DepositConfirmed) return 1;
            else return -1;
        }

        public static int CompareConfirmReverse(AccountingData a, AccountingData b) {
            if (a.DepositConfirmed == b.DepositConfirmed) return InlineCompareConfirm(a, b);
            else if (a.DepositConfirmed && !b.DepositConfirmed) return -1;
            else return 1;
        }
    }
}
