
namespace AccountingManager.Helpers
{
    public class ViewSelectionPageParams
    {
        public ViewSelectionPageParams(MariaManager inSqlManager)
        {
            mSqlManager = inSqlManager;
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager { get => mSqlManager; }
    }
}
