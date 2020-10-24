using EnterpriseMVVM.UI.ViewModel;
using System.Windows;

namespace EnterpriseMVVM.UI
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _mainWindowViewModel;

        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel;
        }
    }
}
