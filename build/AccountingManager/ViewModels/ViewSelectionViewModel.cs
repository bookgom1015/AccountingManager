using System;

using Prism.Windows.Mvvm;

using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class ViewSelectionViewModel : ViewModelBase
    {
        public ViewSelectionViewModel()
        {
            mSqlManager = new MariaManager();
        }

        private MariaManager mSqlManager;
        public MariaManager SqlManager
        {
            get => mSqlManager;
        }

        private bool mConnected = false;
        public bool Connected
        {
            get => mConnected;
            set => SetProperty(ref mConnected, value);
        }
    }
}
