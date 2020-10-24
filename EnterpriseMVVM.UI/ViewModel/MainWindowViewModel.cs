using Autofac.Features.Indexed;
using EnterpriseMVVM.UI.Base;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Service;
using Mohsenmou.MVVM.Core;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EnterpriseMVVM.UI.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IIndex<string, IEntityViewModel> _entityViewModelCreator;
        private IEntityViewModel _selectedEntityViewModel;

        public MainWindowViewModel(IEventAggregator eventAggregator,
                                   IMessageDialogService messageDialogService,
                                   IIndex<string, IEntityViewModel> entityViewModelCreator)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _entityViewModelCreator = entityViewModelCreator;

            EntityViewModels = new ObservableCollection<IEntityViewModel>();
            OpenSingleEntityViewCommand = new DelegateCommand<Type>(OnOpenSingleEntityViewExecute);

            _eventAggregator.GetEvent<AfterEntityClosedEvent>().Subscribe(AfterEntityClosed);
        }
        public ObservableCollection<IEntityViewModel> EntityViewModels { get; }
        public IEntityViewModel SelectedEntityViewModel
        {
            get { return _selectedEntityViewModel; }
            set
            {
                _selectedEntityViewModel = value;
                OnPropertyChanged();
            }
        }
        public ICommand OpenSingleEntityViewCommand { get; }
        private async void OnOpenEntityView(OpenDetailViewEventArgs args)
        {
            try
            {
                var entityViewModel = EntityViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);
                if (entityViewModel == null)
                {
                    entityViewModel = _entityViewModelCreator[args.ViewModelName];
                    await entityViewModel.LoadAsync();
                    if (EntityViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName) == null)
                    {
                        EntityViewModels.Add(entityViewModel);
                    }
                }
                SelectedEntityViewModel = entityViewModel;
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowOKCancelDialog("Error", ex.Message, true, false);
            }
        }
        private void OnOpenSingleEntityViewExecute(Type viewModelType)
        {
            OnOpenEntityView(new OpenDetailViewEventArgs { Id = -1, ViewModelName = viewModelType.Name });
        }
        private void OnCloseEntityViewModel(int id, string viewModelName)
        {
            try
            {
                var entityViewModel = EntityViewModels.SingleOrDefault(vm => vm.Id == id && vm.GetType().Name == viewModelName);
                if (entityViewModel != null)
                {
                    EntityViewModels.Remove(entityViewModel);
                }
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowOKCancelDialog("Error", ex.Message, true, false);
            }
        }
        private void AfterEntityClosed(AfterEntityClosedEventArgs args)
        {
            OnCloseEntityViewModel(args.Id, args.ViewModelName);
        }
    }
}
