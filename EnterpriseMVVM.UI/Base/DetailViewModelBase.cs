using Prism.Commands;
using Prism.Events;
using Mohsenmou.MVVM.Core;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Service;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseMVVM.UI.Base
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {

        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private bool _isChanged;
        private int _id;
        private bool _isFocused;

        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ResetCommand { get; }

        public abstract Task LoadAsync(int id);
        protected abstract void OnSaveExecute();
        protected abstract bool OnSaveCanExecute();
        protected abstract void OnResetExecute();
        protected abstract bool OnResetCanExecute();
        protected abstract void OnDeleteExecute();
        protected abstract void OnOpenDetailView(OpenDetailViewEventArgs args);

        public DetailViewModelBase(IEventAggregator eventAggregator,
                                   IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;

            EventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            ResetCommand = new DelegateCommand(OnResetExecute, OnResetCanExecute);
        }

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public int Id
        {
            get { return _id; }
            protected set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public bool IsFocused
        {
            get { return _isFocused; }
            set
            {
                _isFocused = value;
                OnPropertyChanged();
            }
        }

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(new AfterDetailDeletedEventArgs
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(new AfterDetailSavedEventArgs
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }

    }
}
