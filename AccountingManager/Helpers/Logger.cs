using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AccountingManager.Dialogs;

namespace AccountingManager.Helpers {
    public static class Logger {
        public static async Task MessageBox(string title, string msg) {
            MessageDialog dialog = new MessageDialog() { Title = title, Message = msg };
            await dialog.ShowAsync();
        }
    }
}
