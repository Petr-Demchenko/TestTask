using MedTech.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MedTech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e) => viewModel.ApplyFilter(tbFilter.Text);
    }
}