using Prism.Commands;
using Prism.Events;
using EnterpriseMVVM.Model;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Data.Lookups;
using EnterpriseMVVM.UI.Data.Repositories;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Service;
using EnterpriseMVVM.UI.Wrapper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.ViewModel
{
    public interface IProductDetailViewModel
    {
        int Id { get; }
        bool IsChanged { get; }
        Task LoadAsync(int id);
    }
    public class ProductDetailViewModel : DetailViewModelBase, IProductDetailViewModel
    {
        private ICategoryLookupDataService _categoryLookupDataService;
        private ProductWrapper _product;
        private IProductRepository _productRepository;
        public ProductDetailViewModel(IEventAggregator eventAggregator,
                                       IMessageDialogService messageDialogService,
                                       IProductRepository productRepository,
                                       ICategoryLookupDataService categoryLookupDataService)
                                       : base(eventAggregator, messageDialogService)
        {
            _productRepository = productRepository;
            _categoryLookupDataService = categoryLookupDataService;

            Categories = new ObservableCollection<LookupItem>();

            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public ObservableCollection<LookupItem> Categories { get; }
        public ProductWrapper Product
        {
            get { return _product; }
            private set
            {
                _product = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int id)
        {
            var product = id > 0
                ? await _productRepository.GetByIdAsync(id)
                : CreateNewProduct();
            Id = id;
            InitializeProduct(product);

            await LoadCategoriesLookupAsync();
        }
        protected override async void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog("Delete", "Delete selected product?");
            if (result == MessageDialogResult.OK)
            {
                _productRepository.Remove(Product.Model);
                await _productRepository.SaveAsync();
                RaiseDetailDeletedEvent(Product.ProductId);
            }
        }
        protected override void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (args.ViewModelName == nameof(ProductMainViewModel))
            {
                IsFocused = true;
            }
        }
        protected override bool OnResetCanExecute()
        {
            return Product != null
                && Product.IsChanged;
        }
        protected override void OnResetExecute()
        {
            if (Product != null)
            {
                Product.RejectChanges();
            }
        }
        protected override bool OnSaveCanExecute()
        {
            return Product != null
                && Product.IsChanged
                && Product.IsValid;
        }
        protected override async void OnSaveExecute()
        {
            await _productRepository.SaveAsync();
            Product.AcceptChanges();
            IsChanged = Product.IsChanged;
            RaiseDetailSavedEvent(Product.ProductId, Product.Name);
        }
        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                await LoadCategoriesLookupAsync();
            }
        }
        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                _productRepository.ReloadCategoryAsync(Product.ProductId);
                await LoadCategoriesLookupAsync();
            }
        }
        private Product CreateNewProduct()
        {
            var product = new Product();
            _productRepository.Add(product);
            return product;
        }
        private void InitializeProduct(Product product)
        {
            Product = new ProductWrapper(product);
            Product.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Product.IsChanged) || e.PropertyName == nameof(Product.IsValid))
                {
                    IsChanged = Product.IsChanged;
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)ResetCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ResetCommand).RaiseCanExecuteChanged();
        }
        private async Task LoadCategoriesLookupAsync()
        {
            Categories.Clear();
            var lookupCategory = await _categoryLookupDataService.GetCategoryLookupAsync(ListSortDirection.Ascending);
            foreach (var lookupItemCategory in lookupCategory)
            {
                Categories.Add(lookupItemCategory);
            }
        }
    }
}