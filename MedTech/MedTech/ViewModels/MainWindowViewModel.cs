using MedTech.Managers;
using MedTech.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using Timer = System.Timers.Timer;

namespace MedTech.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly MessageManager messageManager;
        private readonly ICollectionView messagesView;

        public MainWindowViewModel()
        {
            messageManager = new MessageManager();
            messagesView = CollectionViewSource.GetDefaultView(messageManager.Messages);

            Timer timer = new(2000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public ObservableCollection<Message> Messages => messageManager.Messages;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                messageManager.AddMessageAuto();
            });
        }

        public void ApplyFilter(string filterText)
        {
            if (messagesView != null)
            {
                messagesView.Filter = null;
                messagesView.Filter = msg => MessageFilter(msg, filterText);
            }
        }

        private static bool MessageFilter(object item, string filterText)
        {
            if (item is Message msg)
            {
                return msg.Text.Contains(filterText, StringComparison.CurrentCultureIgnoreCase);
            }
            return false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
