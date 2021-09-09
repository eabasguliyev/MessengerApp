using System.ComponentModel;
using System.Runtime.CompilerServices;
using Messenger.Client.Annotations;

namespace Messenger.Client.MVVM.ViewModels
{
    public class ObservableObject:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}