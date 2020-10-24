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

    public interface IProductNavigationViewModel
    {
        Task LoadAsync(ListSortDirection sortDirection, string findText = "");
    }

    public class ProductNavigationViewModel : IProductNavigationViewModel
    {
        private IEventAggregator _eventAggregator;
        private IProductLookupDataService _productLookupService;

        public ObservableCollection<NavigationItemViewModel> Items { get; set; }

        public ProductNavigationViewModel(IEventAggregator eventAggregator,
                                           IProductLookupDataService productLookupDataService)

        {
            _eventAggregator = eventAggregator;
            _productLookupService = productLookupDataService;

            Items = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }


        public async Task LoadAsync(ListSortDirection sortDirection, string findText = "")
        {
            var lookup = await _productLookupService.GetProductLookupAsync(sortDirection, findText);
            Items.Clear();
            foreach (var item in lookup)
            {
                Items.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(ProductDetailViewModel), _eventAggregator));
            }
        }

        public void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProductDetailViewModel))
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
            if (args.ViewModelName == nameof(ProductDetailViewModel))
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
