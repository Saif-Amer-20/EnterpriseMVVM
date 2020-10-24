using Prism.Commands;
using Prism.Events;
using EnterpriseMVVM.UI.Event;
using EnterpriseMVVM.UI.Service;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Mohsenmou.MVVM.Core;

namespace EnterpriseMVVM.UI.Base
{
    public abstract class MainViewModelBase : ViewModelBase, IEntityViewModel
    {
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private int _id;
        private string _title;
        private string _findText;
        private bool _isFocused;
        private bool _isFocusedFindTextBox;

        public MainViewModelBase(IEventAggregator eventAggregator,
                                 IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;

            EventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            CloseEntityViewCommand = new DelegateCommand(OnCloseEntityViewExecute);
            PrintCommand = new DelegateCommand(OnPrintExecute);
            PrintPreviewCommand = new DelegateCommand(OnPrintPreviewExecute);
            AscendingSortCommand = new DelegateCommand(OnAscendingExecute);
            DescendingSortCommand = new DelegateCommand(OnDescendingSortExecute);
            FindCommand = new DelegateCommand(OnFindExecute);
            FindTextBoxFocusCommand = new DelegateCommand(OnFindTextBoxFocusExecute);
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
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
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
        public bool IsFocusedFindTextBox
        {
            get { return _isFocusedFindTextBox; }
            set { _isFocusedFindTextBox = value; OnPropertyChanged(); }
        }
        public ICommand CreateNewDetailCommand { get; }
        public ICommand CloseEntityViewCommand { get; }
        public ICommand PrintCommand { get; set; }
        public ICommand PrintPreviewCommand { get; }
        public ICommand AscendingSortCommand { get; }
        public ICommand DescendingSortCommand { get; }
        public ICommand FindCommand { get; }
        public ICommand FindTextBoxFocusCommand { get; }
        public string FindText
        {
            get { return _findText; }
            set
            {
                _findText = value;
                OnPropertyChanged();
            }
        }
        public abstract Task LoadAsync();
        protected abstract void OnOpenDetailView(OpenDetailViewEventArgs args);
        protected abstract void AfterDetailDeleted(AfterDetailDeletedEventArgs args);
        protected abstract void AfterDetailSaved(AfterDetailSavedEventArgs args);
        protected abstract void OnCreateNewDetailExecute(Type viewModelType);
        protected abstract void OnCloseEntityViewExecute();
        protected abstract void OnPrintExecute();
        protected abstract void OnPrintPreviewExecute();
        protected abstract void OnAscendingExecute();
        protected abstract void OnDescendingSortExecute();
        protected abstract void OnFindExecute();
        private void OnFindTextBoxFocusExecute()
        {
            IsFocusedFindTextBox = true;
        }
    }
}
