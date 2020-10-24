using EnterpriseMVVM.Model;
using Mohsenmou.MVVM.Core;
using System.Threading.Tasks;

namespace EnterpriseMVVM.UI.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsReferencedByProductAsync(int categoryId);
    }
}