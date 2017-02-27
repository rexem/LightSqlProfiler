using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LightSqlProfiler.Gui
{
    /// <summary>
    /// Allows using OnPropertyChanged() mechanism to notify GUI of any changes to an object
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires PropertyChanged event for a property (if not supplied, caller name is used).
        /// </summary>
        /// <param name="name">Property name (takes default caller name if non provided)</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
