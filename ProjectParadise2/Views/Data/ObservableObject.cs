using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjectParadise2.Views
{
    /// <summary>
    /// A base class that implements <see cref="INotifyPropertyChanged"/>, enabling property change notifications.
    /// </summary>
    class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event to notify listeners that a property has changed.
        /// </summary>
        /// <param name="name">The name of the property that changed. The default is the caller's member name.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            // Trigger the PropertyChanged event if there are subscribers
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}