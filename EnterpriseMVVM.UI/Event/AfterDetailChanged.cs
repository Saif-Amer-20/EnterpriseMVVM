using Prism.Events;
namespace EnterpriseMVVM.UI.Event
{
    public class AfterDetailChangedEvent : PubSubEvent<AfterDetailChangedEventArgs>
    {
    }
    public class AfterDetailChangedEventArgs
    {
        public int Id { get; set; }
        public bool HasChanges { get; set; }
        public string ViewModelName { get; set; }
    }
}
