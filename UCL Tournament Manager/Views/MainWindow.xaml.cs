using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using UCL_Tournament_Manager.ViewModels;

namespace UCL_Tournament_Manager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<MainWindowViewModel>();
            DataContext = viewModel;
        }
    }
}