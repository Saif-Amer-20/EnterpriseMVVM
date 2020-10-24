using System.Windows;

namespace EnterpriseMVVM.UI.Service
{
    public partial class OKCancelDialog : Window
    {
        public OKCancelDialog(string title, string message, bool showOKButton = true, bool showCancelButton = true)
        {
            InitializeComponent();
            Title = title;
            textBlock.Text = message;

            okButton.Visibility = (showOKButton == true) ? Visibility.Visible : Visibility.Collapsed;
            cancelButton.Visibility = (showCancelButton == true) ? Visibility.Visible : Visibility.Collapsed;
        }
        private void OkButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;
        private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
