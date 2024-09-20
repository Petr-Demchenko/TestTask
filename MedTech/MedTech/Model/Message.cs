using System.ComponentModel;

namespace MedTech.Model
{
    /// <summary>
    /// Данные представлены объектом, который включает в себя: 
    ///1.  CreationTimeUtc  Дату и время создания записи 
    ///2.  Text             Текст сообщения
    /// </summary>
    internal class Message
    {
        private DateTime creationTimeUtc;
        public DateTime CreationTimeUtc
        {
            get { return creationTimeUtc; }
            set
            {
                creationTimeUtc = value;
                OnPropertyChanged("CreationTimeUtc");
            }
        }

        public required string Text { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
