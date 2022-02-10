using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AccountingManager.Renew.Core.Abstract;

namespace AccountingManager.Renew.Helpers.NavParams {
    public class YearlyNavParams {
        public IDbManager DbManager { get; set; }
        public int? SelectedYear { get; set; }
        public Action<int> SelectedYearChanged { get; set; }
        public Action<int> SelectedMonthChanged { get; set; }
    }
}
