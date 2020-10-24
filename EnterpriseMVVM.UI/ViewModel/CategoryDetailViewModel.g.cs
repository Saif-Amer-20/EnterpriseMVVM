using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using EnterpriseMVVM.Model;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Data.Repositories;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Service;
using EnterpriseMVVM.UI.Wrapper;

namespace EnterpriseMVVM.UI.ViewModel
{
    public interface ICategoryDetailViewModel
    {
        int Id { get; }
        bool IsChanged { get; }
        Task LoadAsync(int id);
    }
    public class CategoryDetailViewModel : DetailViewModelBase, ICategoryDetailViewModel
    {
        private CategoryWrapper _category;
        private ICategoryRepository _categoryRepository;
        public CategoryDetailViewModel(IEventAggregator eventAggregator,
                                       IMessageDialogService messageDialogService,
                                       ICategoryRepository categoryRepository)
                                       : base(eventAggregator, messageDialogService)
        {
            _categoryRepository = categoryRepository;
        }
        public CategoryWrapper Category
        {
            get { return _category; }
            private set
            {
                _category = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int id)
        {
            var category = id > 0
                ? await _categoryRepository.GetByIdAsync(id)
                : CreateNewCategory();
            Id = id;
            InitializeCategory(category);
        }
        protected override async void OnDeleteExecute()
        {
            if (await _categoryRepository.IsReferencedByProductAsync(Category.CategoryId))
            {
                MessageDialogService.ShowOKCancelDialog("Error", "This category referenced by a product", true, false);
                return;
            }
            var result = MessageDialogService.ShowOKCancelDialog("Delete", "Delete selected category?");
            if (result == MessageDialogResult.OK)
            {
                _categoryRepository.Remove(Category.Model);
                await _categoryRepository.SaveAsync();
                RaiseDetailDeletedEvent(Category.CategoryId);
            }
        }
        protected override void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryMainViewModel))
            {
                IsFocused = true;
            }
        }
        protected override bool OnResetCanExecute()
        {
            return Category != null
                && Category.IsChanged;
        }
        protected override void OnResetExecute()
        {
            if (Category != null)
            {
                Category.RejectChanges();
            }
        }
        protected override bool OnSaveCanExecute()
        {
            return Category != null
                && Category.IsChanged
                && Category.IsValid;
        }
        protected override async void OnSaveExecute()
        {
            await _categoryRepository.SaveAsync();
            Category.AcceptChanges();
            IsChanged = Category.IsChanged;
            RaiseDetailSavedEvent(Category.CategoryId, Category.Name);
        }
        private Category CreateNewCategory()
        {
            var category = new Category();
            _categoryRepository.Add(category);
            return category;
        }
        private void InitializeCategory(Category category)
        {
            Category = new CategoryWrapper(category);
            Category.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Category.IsChanged) || e.PropertyName == nameof(Category.IsValid))
                {
                    IsChanged = Category.IsChanged;
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                    ((DelegateCommand)ResetCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)ResetCommand).RaiseCanExecuteChanged();
        }
    }
}
