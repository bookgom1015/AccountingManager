using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingManager.Core.Infrastructures {
    public class GlobalSettings {
        public static string Address { get => "stdaewon.synology.me"; }
        public static short Port { get => 5252; }
        public static string Uid { get => "dw_user"; }
        public static string Pwd { get => "@dbUSER901901@"; }
        public static string DatabaseName { get => "accounting_manager"; }
        public static string DataTableName {
#if DEBUG
            get => "test_accounting_data";
#else
            get => "accounting_data";
#endif
        }
        public static string AccountTableName { get => "users"; }
    }
}
