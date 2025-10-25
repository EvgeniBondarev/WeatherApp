using System.Windows;
using WeatherApp.ViewModels;

namespace WeatherApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
