using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AccountingManager.Helpers {
    public class BindingHelper {
        public static Binding CreateBinding(object source, string path, BindingMode mode, UpdateSourceTrigger trigger) {
            Binding binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = mode;
            binding.UpdateSourceTrigger = trigger;

            return binding;
        }
    }
}
