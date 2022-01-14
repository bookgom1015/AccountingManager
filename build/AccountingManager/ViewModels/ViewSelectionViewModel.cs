using Prism.Windows.Mvvm;

using AccountingManager.Helpers;

namespace AccountingManager.ViewModels
{
    public class ViewSelectionViewModel : ViewModelBase
    {
        public ViewSelectionViewModel() {}

        public MariaManager SqlManager { get; set; }
    }
}
