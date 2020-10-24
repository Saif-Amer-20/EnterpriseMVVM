using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Base
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int id);
        bool IsChanged { get; }
        int Id { get; }
    }
}
