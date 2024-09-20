using MedTech.Managers;
using MedTech.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MedTech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            messageManager = new MessageManager();
            messageManager.PropertyChanged += MessageManager_PropertyChanged;

            DataContext = messageManager;
        }

        private MessageManager messageManager;

        private void MessageManager_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Messages")
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    dgMessages.Items.Refresh();
                });
            }
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = tbFilter.Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(messageManager.Messages);
            view.Filter = item =>
            {
                if (item is Message msg)
                {
                    return msg.Text.ToLower().Contains(filterText);
                }
                return false;
            };
            view.Refresh();
        }
    }
}