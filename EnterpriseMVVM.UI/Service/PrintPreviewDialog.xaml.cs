using System.Windows;
using System.Windows.Documents;

namespace EnterpriseMVVM.UI.Service
{
    public partial class PrintPreviewDialog : Window
    {
        public PrintPreviewDialog(IDocumentPaginatorSource document)
        {
            InitializeComponent();
            documentViewer.Document = document;
        }
    }
}
