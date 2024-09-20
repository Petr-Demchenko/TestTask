using MedTech.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using Timer = System.Timers.Timer;

namespace MedTech.Managers
{
    internal class MessageManager
    {
        private Timer timer;
        private ObservableCollection<Message> messages;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Коллекция данных объектов должна создаваться в памяти при запуске приложения, и по таймеру каждые 2 секунды в данную коллекцию должен добавляться новый объект
        /// </summary>
        public MessageManager()
        {
            messages = new ObservableCollection<Message>();
            for (int i = 1; i < 15; i++)
            {
                AddMessage($"Начальный элемент {i}");
            }

            timer = new Timer(2000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AddMessageAuto();
                OnPropertyChanged(nameof(Messages));
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddMessage(string message)
        {
            messages.Add(new Message { CreationTimeUtc = DateTime.UtcNow, Text = message });
        }

        internal void AddMessageAuto()
        {
            AddMessage($"Самарское время {DateTime.UtcNow.AddHours(4).ToString("HH:mm:ss")}");
        }
    }
}
