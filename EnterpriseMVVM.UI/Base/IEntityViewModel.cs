using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Base
{
    public interface IEntityViewModel
    {
        Task LoadAsync();
        int Id { get; }
    }
}
