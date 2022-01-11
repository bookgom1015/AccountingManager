using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManager.Helpers
{
    class MonthlyNavPageParams
    {
        private KeyValuePair<int, List<int>> mPair;
        public KeyValuePair<int, List<int>> Pair
        {
            get => mPair;
            set => mPair = value;
        }

        public Action<string> DataLitView_SelectionChanged { get; set; }
    }
}
