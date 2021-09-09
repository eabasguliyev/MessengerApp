using System.Windows;

namespace Messenger.Client.MVVM.Views.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public void ShowInfoDialog(string content, string caption)
        {
            MessageBox.Show(content, caption);
        }
    }
}