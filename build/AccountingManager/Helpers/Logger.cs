using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

using AccountingManager.Dialogs;

namespace AccountingManager.Helpers
{
    public class Logger
    {


        public static async Task StaticInit()
        {
            DateTime localDate = DateTime.Now;

            string date = localDate.ToString("yyyy_MM_dd");

            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile logFile = await folder.CreateFileAsync(date + ".log", CreationCollisionOption.OpenIfExists);

            BasicProperties basicProperties = await logFile.GetBasicPropertiesAsync();

            mStream = await logFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.AllowReadersAndWriters);
            mStream.Seek(basicProperties.Size);
        }

        public static async Task Log(string inMsg)
        {
            DateTime localDate = DateTime.Now;

            string time = localDate.ToString("HH:mm:ss");
            string textToWrite = "[" + time + "]" + inMsg;

            IBuffer buffer = Encoding.ASCII.GetBytes(textToWrite).AsBuffer();
            
            if (mStream.CanWrite) await mStream.WriteAsync(buffer);
        }

        public static async Task Logln(string inMsg)
        {
            DateTime localDate = DateTime.Now;

            string time = localDate.ToString("HH:mm:ss");
            string textToWrite = "[" + time + "]" + inMsg + "\r\n";

            IBuffer buffer = Encoding.ASCII.GetBytes(textToWrite).AsBuffer();

            if (mStream.CanWrite) await mStream.WriteAsync(buffer);
        }

        public static async Task ShowAlertDialog(string inMsg)
        {
            AlertDialog alertDialog = new AlertDialog(inMsg);
            await alertDialog.ShowAsync();
        }

        private static IRandomAccessStream mStream = null;
    }
}
