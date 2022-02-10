using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Renew.Helpers {
    public class DoubleClickPreventer : IDisposable {
        public DoubleClickPreventer(object sender) {
            button = sender as Button;
            if (button != null) button.IsEnabled = false;
        }

        public void Dispose() {
            if (button != null) button.IsEnabled = true;
        }

        private readonly Button button;
    }
}
