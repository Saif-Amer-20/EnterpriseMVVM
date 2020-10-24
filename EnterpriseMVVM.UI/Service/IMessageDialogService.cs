using System.Windows.Documents;

namespace EnterpriseMVVM.UI.Service
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOKCancelDialog(string title, string message, bool showOKButton = true, bool showCancelButton = true);
        void ShowPrintPreviewDialog(IDocumentPaginatorSource document);
    }

}