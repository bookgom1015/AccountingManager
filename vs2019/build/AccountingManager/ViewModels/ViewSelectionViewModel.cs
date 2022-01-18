using Prism.Windows.Mvvm;

using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class ViewSelectionViewModel : ViewModelBase
    {
        public ViewSelectionViewModel() { }

        public MariaDbManager SqlManager { get; set; }
        public string DatabaseName { get; set; }
    }
}
