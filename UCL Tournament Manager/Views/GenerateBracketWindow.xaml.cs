using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UCL_Tournament_Manager.ViewModels;

namespace UCL_Tournament_Manager.Views
{
    public partial class GenerateBracketWindow : Window
    {
        public GenerateBracketWindow()
        {
            InitializeComponent();
            var viewModel = ((App)Application.Current).ServiceProvider.GetRequiredService<GenerateBracketViewModel>();
            viewModel.NavigateBack = NavigateBack;
            DataContext = viewModel;
        }

        private void NavigateBack()
        {
            this.Close();
            var mainWindow = ((App)Application.Current).ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
