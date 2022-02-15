using System;
using System.Collections.Generic;
using System.Text;

using AccountingManager.Renew.Core.Infrastructures;
using AccountingManager.Renew.Core.Models;

namespace AccountingManager.Renew.Core.Abstract {
    public interface IDbManager {
        Result Connect(string address, Int16 port, string uid, string pwd);
        void Disconnect();
        Result Use(string database);
        Result Add(AccountingData data);
        Result Update(AccountingData data, AccountingDataQueryKeys keys);
        Result Delete(AccountingData data);
        Result GetDates(out IEnumerable<int> dates, int? year = null, int? month = null, bool receivable = false);
        Result GetData(out IEnumerable<AccountingData> data, int? year = null, int? month = null, int? day = null, bool receivable = false);
        Result GetData(out IEnumerable<AccountingData> data, DateTime begin, DateTime end, string clientName, bool receivable = false);

        bool IsConnected { get; }
    }
}
