using System.Windows;
using System.Windows.Documents;

namespace EnterpriseMVVM.UI.Service
{
    public class MessageDialogService : IMessageDialogService
    {

        public MessageDialogResult ShowOKCancelDialog(string title, string message, bool showOKButton = true, bool showCancelButton = true)
        {
            return new OKCancelDialog(title, message, showOKButton, showCancelButton)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = App.Current.MainWindow
            }.ShowDialog().GetValueOrDefault()
            ? MessageDialogResult.OK
            : MessageDialogResult.Cancel;
        }
        public void ShowPrintPreviewDialog(IDocumentPaginatorSource document)
        {
            var dialog = new PrintPreviewDialog(document)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = App.Current.MainWindow
            }.ShowDialog();
        }
    }
    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}
