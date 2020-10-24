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
    public interface ICategoryMainViewModel : IEntityViewModel
    {
    }
    public class CategoryMainViewModel : MainViewModelBase, ICategoryMainViewModel
    {
        private Func<ICategoryDetailViewModel> _categoryDetailViewModelCreator;
        private ICategoryRepository _categoryRepository;
        private ICategoryDetailViewModel _detailViewModel;
        private ObservableCollection<Category> categories;

        public CategoryMainViewModel(IEventAggregator eventAggregator,
                                     IMessageDialogService messageDialogService,
                                     Func<ICategoryDetailViewModel> categoryDetailViewModelCreator,
                                     ICategoryNavigationViewModel categoryNavigationViewModel,
                                     ICategoryRepository categoryRepository)
                                     : base(eventAggregator, messageDialogService)
        {
            Title = "Categories";
            Id = -1;

            _categoryDetailViewModelCreator = categoryDetailViewModelCreator;
            NavigationViewModel = categoryNavigationViewModel;
            _categoryRepository = categoryRepository;
        }
        public ICategoryDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }
        public ICategoryNavigationViewModel NavigationViewModel { get; }
        public override async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync(ListSortDirection.Ascending);
        }
        protected override void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                DetailViewModel = null;
                IsFocused = true;
            }
        }
        protected override void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
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
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                if (DetailViewModel != null && DetailViewModel.IsChanged)
                {
                    var result = MessageDialogService.ShowOKCancelDialog("Info", "You've made changes, navigate anyway?");
                    if (result == MessageDialogResult.Cancel)
                    {
                        return;
                    }
                }
                DetailViewModel = _categoryDetailViewModelCreator();
                await DetailViewModel.LoadAsync(args.Id);
            }
        }
        protected async override void OnPrintExecute()
        {
            categories = new ObservableCollection<Category>(await _categoryRepository.GetAllAsync());

            if (categories.Count != 0)
            {
                PrintDialog printDialog = new PrintDialog { PrintQueue = LocalPrintServer.GetDefaultPrintQueue() };
                printDialog.PrintDocument(GetCategoryPaginator(printDialog), "Categories");
            }
        }
        protected override async void OnPrintPreviewExecute()
        {
            categories = new ObservableCollection<Category>(await _categoryRepository.GetAllAsync());

            if (categories.Count != 0)
            {
                string tempFileName = Path.GetTempFileName();
                File.Delete(tempFileName);
                using (XpsDocument xpsDocument = new XpsDocument(tempFileName, FileAccess.ReadWrite))
                {
                    PrintDialog printDialog = new PrintDialog();
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                    writer.Write(GetCategoryPaginator(printDialog));
                    MessageDialogService.ShowPrintPreviewDialog(xpsDocument.GetFixedDocumentSequence());
                }
            }
        }
        private CategoryPaginator GetCategoryPaginator(PrintDialog printDialog)
        {
            return new CategoryPaginator(categories, new Typeface("Segoe UI Light"), 12, 96 * 0.75,
                                new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
        }
    }
}