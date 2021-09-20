using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using Messenger.Client.Events;
using Messenger.Domain;
using Prism.Commands;
using Prism.Events;

namespace Messenger.Client.MVVM.ViewModels
{
    public class MainWindowViewModel:ObservableObject, IMainWindowViewModel
    {
        private readonly IRegisterViewModel _registerViewModel;
        private readonly ILoginViewModel _loginViewModel;
        private readonly IMessengerViewModel _messengerViewModel;
        private readonly IEventAggregator _eventAggregator;
        private ObservableObject _currentViewModel;
        public MainWindowViewModel(IRegisterViewModel registerViewModel, ILoginViewModel loginViewModel, IMessengerViewModel messengerViewModel, IEventAggregator eventAggregator)
        {
            _registerViewModel = registerViewModel;
            _loginViewModel = loginViewModel;
            _messengerViewModel = messengerViewModel;
            _eventAggregator = eventAggregator;

            LoadCommand = new DelegateCommand(Load);

            _eventAggregator.GetEvent<OpenLoginViewEvent>().Subscribe(SwitchViewModel);
            _eventAggregator.GetEvent<OpenRegisterViewEvent>().Subscribe(SwitchViewModel);
            _eventAggregator.GetEvent<OpenMessengerViewEvent>().Subscribe(OpenMessengerView);
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
        
        private void SwitchViewModel(string viewModelName)
        {
            switch (viewModelName)
            {
                case nameof(LoginViewModel):
                    CurrentViewModel = _loginViewModel as ObservableObject;
                    break;
                case nameof(RegisterViewModel):
                    CurrentViewModel = _registerViewModel as ObservableObject;
                    break;
            }
        }

        private void OpenMessengerView(User loggedUser)
        {
            _messengerViewModel.LoggedUser = loggedUser;
            CurrentViewModel = _messengerViewModel as ObservableObject;
        }
    }
}