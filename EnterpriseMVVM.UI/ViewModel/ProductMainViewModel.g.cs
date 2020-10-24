using Prism.Events;
using EnterpriseMVVM.Model;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Data.Repositories;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Print;
using EnterpriseMVVM.UI.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace EnterpriseMVVM.UI.ViewModel
{
    public interface IProductMainViewModel : IEntityViewModel
    {
    }
    public class ProductMainViewModel : MainViewModelBase, IProductMainViewModel
    {
        private IProductDetailViewModel _detailViewModel;
        private Func<IProductDetailViewModel> _productDetailViewModelCreator;
        private IProductRepository _productRepository;
        private ObservableCollection<Product> products;

        public ProductMainViewModel(IEventAggregator eventAggregator,
                                     IMessageDialogService messageDialogService,
                                     Func<IProductDetailViewModel> productDetailViewModelCreator,
                                     IProductNavigationViewModel productNavigationViewModel,
                                     IProductRepository productRepository)
                                     : base(eventAggregator, messageDialogService)
        {
            Title = "Products";
            Id = -1;

            _productDetailViewModelCreator = productDetailViewModelCreator;
            NavigationViewModel = productNavigationViewModel;

            _productRepository = productRepository;
        }
        public IProductDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }
        public IProductNavigationViewModel NavigationViewModel { get; }
        public override async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync(ListSortDirection.Ascending);
        }
        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProductDetailViewModel))
            {
                DetailViewModel = null;
                IsFocused = true;
            }
        }
        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProductDetailViewModel))
            {
                IsFocused = true;
            }
        }
        protected override async void OnAscendingExecute()
        {
            await NavigationViewModel.LoadAsync(ListSortDirection.Ascending);
        }
        protected override void OnCloseEntityViewExecute()
        {
            if (DetailViewModel != null && DetailViewModel.IsChanged)
            {
                var result = MessageDialogService.ShowOKCancelDialog("Info", "You've made changes, close anyway?");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            DetailViewModel = null;
            EventAggregator.GetEvent<AfterEntityClosedEvent>().Publish(
                new AfterEntityClosedEventArgs
                {
                    Id = this.Id,
                    ViewModelName = this.GetType().Name
                });
        }
        protected override void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs() { ViewModelName = viewModelType.Name });
            EventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs
            {
                Id = this.Id,
                ViewModelName = this.GetType().Name
            });
        }
        protected override async void OnDescendingSortExecute()
        {
            await NavigationViewModel.LoadAsync(ListSortDirection.Descending);
        }
        protected override async void OnFindExecute()
        {
            await NavigationViewModel.LoadAsync(ListSortDirection.Ascending, FindText);
        }
        protected override async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (args.ViewModelName == nameof(ProductDetailViewModel))
            {
                if (DetailViewModel != null && DetailViewModel.IsChanged)
                {
                    var result = MessageDialogService.ShowOKCancelDialog("Info", "You've made changes, navigate anyway?");
                    if (result == MessageDialogResult.Cancel)
                    {
                        return;
                    }
                }
                DetailViewModel = _productDetailViewModelCreator();
                await DetailViewModel.LoadAsync(args.Id);
            }
        }
        protected override async void OnPrintExecute()
        {
            products = new ObservableCollection<Product>(await _productRepository.GetAllAsync());

            if (products.Count != 0)
            {
                PrintDialog printDialog = new PrintDialog { PrintQueue = LocalPrintServer.GetDefaultPrintQueue() };
                printDialog.PrintDocument(GetProductPaginator(printDialog), "Products");
            }
        }
        protected override async void OnPrintPreviewExecute()
        {
            products = new ObservableCollection<Product>(await _productRepository.GetAllAsync());

            if (products.Count != 0)
            {
                string tempFileName = Path.GetTempFileName();
                File.Delete(tempFileName);
                using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
                {
                    PrintDialog printDialog = new PrintDialog();
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                    writer.Write(GetProductPaginator(printDialog));
                    MessageDialogService.ShowPrintPreviewDialog(xpsDocument.GetFixedDocumentSequence());
                }
            }
        }
        private ProductPaginator GetProductPaginator(PrintDialog printDialog)
        {
            return new ProductPaginator(products, new Typeface("Segoe UI Light"), 12, 96 * 0.75,
                                new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
        }
    }
}