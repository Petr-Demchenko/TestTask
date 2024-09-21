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
        private readonly ObservableCollection<Message> messages;

        /// <summary>
        /// Коллекция данных объектов должна создаваться в памяти при запуске приложения, и по таймеру каждые 2 секунды в данную коллекцию должен добавляться новый объект
        /// </summary>
        public MessageManager()
        {
            messages = [];
            for (int i = 1; i < 15; i++)
            {
                AddMessage($"Начальный элемент {i}");
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
        }

        internal void AddMessage(string message)
        {
            messages.Add(new Message { CreationTimeUtc = DateTime.UtcNow, Text = message });
        }

        internal void AddMessageAuto()
        {
            AddMessage($"Самарское время {DateTime.UtcNow.AddHours(4):HH:mm:ss}");
        }
    }
}
