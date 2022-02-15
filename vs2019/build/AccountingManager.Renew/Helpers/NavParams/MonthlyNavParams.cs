using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AccountingManager.Renew.Core.Abstract;

namespace AccountingManager.Renew.Helpers.NavParams {
    public class MonthlyNavParams {
        public IDbManager DbManager { get; set; }
        public bool Receivable { get; set; }
        public int? SelectedYear { get; set; }
        public Action<int> SelectedMonthChanged { get; set; }
    }
}
