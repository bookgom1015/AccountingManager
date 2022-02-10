using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace AccountingManager.Renew.Helpers {
    public class FrameHelper {
        public static bool TryGoBack(Frame frame) {
            if (frame == null || !frame.CanGoBack) return false;

            frame.GoBack();

            return true;
        }
    }
}
