namespace AccountingManager.Helpers
{
    public class AccountingDataListPageParams
    {
        public AccountingDataListPageParams(MariaDbManager inSqlManager, string inDatabaseName)
        {
            mSqlManager = inSqlManager;
            mDatabaseName = inDatabaseName;
        }

        private MariaDbManager mSqlManager;
        public MariaDbManager SqlManager { get => mSqlManager; }

        private string mDatabaseName;
        public string DatabaseName { get => mDatabaseName; }
    }
}
