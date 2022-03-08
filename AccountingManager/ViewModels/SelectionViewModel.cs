using System;

using Windows.Storage;

using Prism.Windows.Mvvm;

using AccountingManager.Core.Abstract;
using AccountingManager.Core.Concrete;
using AccountingManager.Core.Infrastructures;

namespace AccountingManager.ViewModels {
    public class SelectionViewModel : ViewModelBase {
        public SelectionViewModel() {
            dbManager = new MariaDbManager();
        }

        public void LoadSettings() {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            Object userNmaeObj = localSettings.Values["UserName"];
            UserName = userNmaeObj == null ? null : userNmaeObj as string;

            Object pwdObj = localSettings.Values["Password"];
            Password = pwdObj == null ? null : pwdObj as string;
        }

        public void SaveSettings() {
            ApplicationDataContainer localSettinds = ApplicationData.Current.LocalSettings;

            if (UserName != null) localSettinds.Values["UserName"] = UserName;
            if (Password != null) localSettinds.Values["Password"] = Password;
        }

        public Result ConnectToDb() {
            Result result = dbManager.Connect(GlobalSettings.Address, GlobalSettings.Port, UserName, Password);
            if (!result.Status) {
                UserName = null;
                Password = null;
                return result;
            }

            result = dbManager.Use(GlobalSettings.DatabaseName);
            if (!result.Status) return result;

            return new Result { Status = true };
        }

        public void DisconnectFromDb() {
            dbManager.Disconnect();
        }

        private IDbManager dbManager;
        public IDbManager DbManager { get => dbManager; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public bool CanConnect { get => !(UserName == null || Password == null); }
        public bool IsConnected { get => dbManager.IsConnected; }

        private bool loggined = false;
        public bool Loggined {
            get => loggined;
            set => SetProperty(ref loggined, value);
        }
    }
}
