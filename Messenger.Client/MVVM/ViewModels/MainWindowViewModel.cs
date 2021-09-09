using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using Prism.Commands;

namespace Messenger.Client.MVVM.ViewModels
{
    public class MainWindowViewModel:ObservableObject, IMainWindowViewModel
    {
        private readonly IRegisterViewModel _registerViewModel;
        private ObservableObject _currentViewModel;
        public MainWindowViewModel(IRegisterViewModel registerViewModel)
        {
            _registerViewModel = registerViewModel;

            LoadCommand = new DelegateCommand(Load);
        }

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCommand { get; }

        private void Load()
        {
            CurrentViewModel = _registerViewModel as ObservableObject;
        }


    }
}