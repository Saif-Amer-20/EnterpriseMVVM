using Prism.Events;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Data.Lookups;
using EnterpriseMVVM.UI.Event;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.ViewModel
{
    public interface ICategoryNavigationViewModel
    {
        Task LoadAsync(ListSortDirection sortDirection, string findText = "");
    }
    public class CategoryNavigationViewModel : ICategoryNavigationViewModel
    {
        private IEventAggregator _eventAggregator;
        private ICategoryLookupDataService _categoryLookupService;

        public ObservableCollection<NavigationItemViewModel> Items { get; set; }

        public CategoryNavigationViewModel(IEventAggregator eventAggregator,
                                           ICategoryLookupDataService categoryLookupDataService)

        {
            _eventAggregator = eventAggregator;
            _categoryLookupService = categoryLookupDataService;

            Items = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync(ListSortDirection sortDirection, string findText = "")
        {
            var lookup = await _categoryLookupService.GetCategoryLookupAsync(sortDirection, findText);
            Items.Clear();
            foreach (var item in lookup)
            {
                Items.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(CategoryDetailViewModel), _eventAggregator));
            }
        }

        public void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                var lookupItem = Items.SingleOrDefault(l => l.Id == args.Id && l.DetailViewModelName == args.ViewModelName);
                if (lookupItem == null)
                {
                    Items.Add(new NavigationItemViewModel(args.Id,
                        args.DisplayMember,
                        args.ViewModelName,
                        _eventAggregator));
                }
                else
                {
                    lookupItem.DisplayMember = args.DisplayMember;
                }
            }
        }

        public void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(CategoryDetailViewModel))
            {
                var item = Items.SingleOrDefault(i => i.Id == args.Id);
                if (item != null)
                {
                    Items.Remove(item);
                }
            }
        }

    }
}
