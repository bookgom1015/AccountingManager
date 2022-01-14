
namespace AccountingManager.Helpers
{
    public class AccountingDataListPageParams
    {
        public AccountingDataListPageParams(MariaManager inSqlManager)
        {
            mSqlManager = inSqlManager;
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager { get => mSqlManager; }
    }
}
