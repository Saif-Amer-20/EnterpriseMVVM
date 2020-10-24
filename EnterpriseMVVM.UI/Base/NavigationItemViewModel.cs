using Prism.Commands;
using Prism.Events;
using Mohsenmou.MVVM.Core;
using EnterpriseMVVM.UI.Event;
using System.Windows.Input;

namespace EnterpriseMVVM.UI.Base
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private string _detailViewModelName;
        private IEventAggregator _eventAggregator;

        public ICommand OpenDetailViewCommand { get; }
        public int Id { get; }

        public NavigationItemViewModel(int id,
                                       string displayMember,
                                       string detailViewModelName,
                                       IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _eventAggregator = eventAggregator;
            DetailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public string DetailViewModelName
        {
            get { return _detailViewModelName; }
            set
            {
                _detailViewModelName = value;
                OnPropertyChanged();
            }
        }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs
            {
                Id = Id,
                ViewModelName = _detailViewModelName
            });
        }

    }
}
