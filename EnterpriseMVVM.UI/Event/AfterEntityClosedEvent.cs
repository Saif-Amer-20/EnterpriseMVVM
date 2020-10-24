using Prism.Events;

namespace EnterpriseMVVM.UI.Event
{
    public class AfterEntityClosedEvent : PubSubEvent<AfterEntityClosedEventArgs>
    {
    }
    public class AfterEntityClosedEventArgs
    {
        public int Id { get; set; }
        public string ViewModelName { get; set; }
    }
}
