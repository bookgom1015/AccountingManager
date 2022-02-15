using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AccountingManager.Renew.Helpers {
    public class DoubleClickPreventer : IDisposable {
        public DoubleClickPreventer(object sender) {
            control = sender as Control;
            if (control != null) control.IsEnabled = false;
        }

        public void Dispose() {
            if (control != null) control.IsEnabled = true;
        }

        private readonly Control control;
    }
}
